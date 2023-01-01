using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService;
using Transenvios.Shipping.Api.Domains.UserService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.ShipmentOrderService
{
    public class ShipmentOrderMediator : ICalculateShipmentCharges
    {
        private readonly ShipmentSettings _settings;
        private readonly DataContext _context;
        private readonly IGetUser _getUser;

        public ShipmentOrderMediator(
            IOptions<AppSettings> appSettings,
            DataContext context,
            IGetUser getUser)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _settings = appSettings.Value.Shipment ?? throw new ArgumentNullException(nameof(appSettings));
            _getUser = getUser ?? throw new ArgumentNullException();
        }

        public decimal CalculateChargeByWeight(ShipmentRoute route, decimal weight)
        {
            decimal price = (route.InitialKiloPrice ?? 0) +
                (weight - 1) * (route.AdditionalKiloPrice ?? 0);
            return price;
        }

        public decimal CalculateChargeByVolume(ShipmentRoute route, decimal height, decimal length, decimal width)
        {
            decimal volume = height * length * width;
            decimal price = volume * (route.PriceCm3 ?? 0);
            return price;
        }

        public decimal CalculateInitialPayment(ShipmentRoute route, ShipmentOrderItem item)
        {
            var chargesByWeight = CalculateChargeByWeight(route, item.Weight ?? 0);
            var chargesByVolume = CalculateChargeByVolume(
                route, item.Height ?? 0, item.Length ?? 0, item.Width ?? 0);
            return chargesByWeight > chargesByVolume ? chargesByWeight : chargesByVolume;
        }

        public decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItem item, decimal initialCharge)
        {
            var insuredAmount = item.InsuredAmount > 0 ? item.InsuredAmount * (_settings.InsuredAmountRatio / 100M) : 0;
            var urgentAmount = item.IsUrgent ? initialCharge * (_settings.UrgentAmountRatio / 100M) : 0;
            var fragileAmount = item.IsFragile ? initialCharge * (_settings.FragileAmountRatio / 100M) : 0;
            return (insuredAmount ?? 0) + urgentAmount + fragileAmount;
        }

        public ShipmentOrderResponse CalculateShipmentCharges(ShipmentRoute route, ShipmentOrderRequest order)
        {
            var priceService = new ShipmentOrderResponse
            {
                BasePrice = 0M,
                Taxes = 0M,
                Total = 0M
            };

            if (order.Items == null)
            {
                return priceService;
            }

            foreach (var orderItem in order.Items)
            {
                var basePrice = CalculateInitialPayment(route, orderItem);
                var additionalCharges = CalculateAdditionalCharges(route, orderItem, basePrice);
                priceService.BasePrice += basePrice + additionalCharges;
            }

            priceService.Taxes = priceService.BasePrice * (_settings.TaxAmountRatio / 100M);
            priceService.Total = priceService.BasePrice + priceService.Taxes;

            return priceService;
        }

        private async Task<Guid> GetApplicantId(string? email)
        {
            var applicant = await _getUser?.GetByEmailAsync(email ?? string.Empty);
            if (applicant == null)
            {
                throw new ArgumentException("Applicant Not Found");
            }

            return applicant.Id;
        }


        public async Task<ShipmentOrderResponse> SaveShipmentChargesAsync(ShipmentOrderRequest? order)
        {
            var orderResponse = new ShipmentOrderResponse();

            try
            {
                if (order?.Sender == null || order.Recipient == null)
                {
                    throw new ArgumentException("Sender or Recipient is null");
                }

                var applicantId = await GetApplicantId(order.ApplicantEmail);

                orderResponse.BasePrice = order.BasePrice;
                orderResponse.Taxes = order.Taxes;
                orderResponse.Total = order.Total;

                var shipmentOrder = await SaveOrderHeader(order, applicantId);

                await SaveOrderItems(order, shipmentOrder.Id);
                await _context.SaveChangesAsync();
            }
            catch (Exception error)
            {
                orderResponse.ErrorMessage = error.GetBaseException().Message;
            }

            return orderResponse;
        }

        private async Task SaveOrderItems(ShipmentOrderRequest order, int orderId)
        {
            var shipmentOrderItems = await GetByShipmentOrderItemsAsync();
            var items = order.Items.Select(detail =>
                    new ShipmentOrderItem
                    {
                        IdOrder = orderId,
                        Width = detail.Width,
                        Weight = detail.Weight,
                        Height = detail.Height,
                        IsUrgent = detail.IsUrgent,
                        IsFragile = detail.IsFragile,
                        Length = detail.Length,
                        InsuredAmount = detail.InsuredAmount
                    })
                .ToList();

            await _context.ShipmentOrderItems.AddRangeAsync(items);
        }

        private async Task<ShipmentOrder> SaveOrderHeader(ShipmentOrderRequest order, Guid applicantId)
        {
            var orderId = await GetByIdShipmentOrdersAsync();
            var shipmentOrder = new ShipmentOrder
            {
                Id = orderId,
                PickUpCityId = order.Route?.PickUp?.CityCode,
                PickUpAddress = order.Route?.PickUp?.Address,
                DropOffCityId = order.Route?.DropOff?.CityCode,
                DropOffAddress = order.Route?.DropOff?.Address,
                InitialPrice = order.BasePrice,
                Taxes = order.Taxes,
                TotalPrice = order.Total,
                PaymentState = Convert.ToString((int)PaymentStates.UnPaid),
                ShipmentState = Convert.ToString((int)ShipmentStates.None),
                TransporterId = string.Empty,
                ApplicantId = applicantId.ToString(),
                ApplicationDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                ModifyUserId = applicantId.ToString(),
                SenderDocumentType = order.Sender?.DocumentType,
                SenderDocumentId = order.Sender?.DocumentId,
                SenderFirstName = order.Sender?.FirstName,
                SenderLastName = order.Sender?.LastName,
                SenderEmail = order.Sender?.Email,
                SenderCountryCode = order.Sender?.CountryCode,
                SenderPhone = order.Sender?.Phone,
                RecipientDocumentType = order.Recipient?.DocumentType,
                RecipientDocumentId = order.Recipient?.DocumentId,
                RecipientFirstName = order.Recipient?.FirstName,
                RecipientLastName = order.Recipient?.LastName,
                RecipientEmail = order.Recipient?.Email,
                RecipientCountryCode = order.Recipient?.CountryCode,
                RecipientPhone = order.Recipient?.Phone
            };

            if (_context.ShipmentOrders == null)
            {
                throw new ArgumentException("Shipment Order context is invalid");
            }

            await _context.ShipmentOrders.AddAsync(shipmentOrder);

            return shipmentOrder;
        }

        public async Task<int> GetByIdShipmentOrdersAsync()
        {
            var valueMax = 0;
            if (_context.ShipmentOrders != null)
            {
                valueMax = await _context.ShipmentOrders.MaxAsync(e => e.Id);
            }
            return valueMax == 0 ? 1 : valueMax + 1;
        }
        public async Task<Guid> GetByShipmentOrderItemsAsync()
        {
            return Guid.NewGuid();
        }
    }
}