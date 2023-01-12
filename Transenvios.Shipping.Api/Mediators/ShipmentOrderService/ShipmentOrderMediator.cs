using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses;
using Transenvios.Shipping.Api.Domains.UserService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.ShipmentOrderService
{
    public class ShipmentOrderMediator : IOrderChargesCalculator, IOrderStorage
    {
        private readonly ShipmentSettings _settings;
        private readonly IDbContext _context;
        private readonly IGetUser _getUser;
        private readonly ICatalogStorage<City> _getCity;

        public ShipmentOrderMediator(
            IOptions<AppSettings> appSettings,
            IDbContext context,
            IGetUser getUser,
            ICatalogStorage<City> getCity)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _settings = appSettings.Value.Shipment ?? throw new ArgumentNullException(nameof(appSettings));
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _getCity = getCity ?? throw new ArgumentNullException(nameof(getCity));
        }

        public decimal CalculateChargeByWeight(ShipmentRoute route, decimal weight)
        {
            decimal price = (route.InitialKiloPrice ?? 0) +
                (weight - 1) * (route.AdditionalKiloPrice ?? 0);
            return price;
        }

        public decimal CalculateChargeByVolume(ShipmentRoute route, decimal height, decimal length, decimal width)
        {
            var volume = height * length * width;
            var price = volume * (route.PriceCm3 ?? 0);
            return price;
        }

        public decimal CalculateInitialPayment(ShipmentRoute route, ShipmentOrderItemRequest item)
        {
            var chargesByWeight = CalculateChargeByWeight(route, item.Weight ?? 0);
            var chargesByVolume = CalculateChargeByVolume(
                route, item.Height ?? 0, item.Length ?? 0, item.Width ?? 0);
            return chargesByWeight > chargesByVolume ? chargesByWeight : chargesByVolume;
        }

        public decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItemRequest item, decimal initialCharge)
        {
            var insuredAmount = item.InsuredAmount > 0 ? item.InsuredAmount * (_settings.InsuredAmountRatio / 100M) : 0;
            var urgentAmount = item.IsUrgent ? initialCharge * (_settings.UrgentAmountRatio / 100M) : 0;
            var fragileAmount = item.IsFragile ? initialCharge * (_settings.FragileAmountRatio / 100M) : 0;
            return (insuredAmount ?? 0) + urgentAmount + fragileAmount;
        }

        public ShipmentOrderSubmitResponse CalculateCharges(ShipmentRoute route, ShipmentOrderRequest? order)
        {
            var charges = new ShipmentOrderSubmitResponse
            {
                BasePrice = 0M,
                Taxes = 0M,
                Total = 0M
            };

            if (order.Items == null)
            {
                return charges;
            }

            foreach (var orderItem in order.Items)
            {
                var basePrice = CalculateInitialPayment(route, orderItem);
                var additionalCharges = CalculateAdditionalCharges(route, orderItem, basePrice);
                charges.BasePrice += basePrice + additionalCharges;
            }

            charges.Taxes = charges.BasePrice * (_settings.TaxAmountRatio / 100M);
            charges.Total = charges.BasePrice + charges.Taxes;

            charges.BasePrice = Math.Round(charges.BasePrice, 2);
            charges.Taxes = Math.Round(charges.Taxes, 2);
            charges.Total = Math.Round(charges.Total, 2);

            return charges;
        }

        private async Task<Guid> GetApplicantId(string? email)
        {
            var applicant = await _getUser?.GetAsync(email ?? string.Empty);
            if (applicant == null)
            {
                throw new ArgumentException("Applicant Not Found");
            }

            return applicant.Id;
        }

        public async Task<ShipmentOrderSubmitResponse> SubmitOrderAsync(ShipmentOrderRequest? order)
        {
            var orderResponse = new ShipmentOrderSubmitResponse();

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

        public async Task<ShipmentOrderListResponse> GetShipmentListAsync(int offset, int limit)
        {
            // await _context.Shipments.Include(x => x.)

            //var totalRows = await _context.Shipments!.CountAsync();
            //var records = await _context.Shipments.FromSqlRaw<ShipmentOrderListResponse>(
            //    @"SELECT " +
            //    "   so.id AS OrderId, " +
            //    "   CONCAT(u.LastName, ', ', u.FirstName) AS ApplicantName, " +
            //    "   CONCAT('+', u.CountryCode, ' ', u.Phone) AS Phone, " +
            //    "   pc.Name AS FromCity, " +
            //    "   dc.Name AS ToCity, " +
            //    "   CASE so.PaymentState " +
            //    "   WHEN 1 THEN 'Pagado' " +
            //    "   ELSE 'Sin Pagar' " +
            //    "   END AS PaymentState, " +
            //    "   CONCAT(d.LastName, ', ', d.FirstName) AS TransporterName, " +
            //    "   so.TransporterId, " +
            //    "   CASE so.ShipmentState " +
            //    "   WHEN 1 THEN 'Recogiendo' " +
            //    "   WHEN 2 THEN 'En bodega' " +
            //    "   WHEN 3 THEN 'En ruta' " +
            //    "   WHEN 4 THEN 'Entregado' " +
            //    "   WHEN 5 THEN 'Cancelado' " +
            //    "   ELSE 'Ordenado' " +
            //    "   END AS ShipmentState, " +
            //    "   ROUND(so.TotalPrice, 2) AS ShippingCost " +
            //    " FROM Shipments so " +
            //    "   LEFT OUTER JOIN Users u ON so.CustomerId = u.Id " +
            //    "   LEFT OUTER JOIN Cities pc ON so.PickUpCityId = pc.Code " +
            //    "   LEFT OUTER JOIN Cities dc ON so.DropOffCityId = dc.Code " +
            //    "   LEFT OUTER JOIN Drivers d ON so.TransporterId = d.Id;")
            //    .Skip(offset).Take(limit).ToArrayAsync();
            //var count = records.Length;
            //var response = new ShipmentOrderListResponse
            //{
            //    Pagination = new ShipmentOrderListPaginationResponse
            //    {
            //        Total = totalRows,
            //        Count = count,
            //        Offset = offset,
            //        Limit = limit
            //    },
            //    Items = records.Select(r => new ShipmentOrderListItemResponse
            //    {
            //        OrderId = r.Id
            //    }).ToList()
            //};

            //return response;
            throw new NotImplementedException();
        }

        public async Task<long> GetNextOrderIdAsync()
        {
            var valueMax = 0L;
            if (_context.ShipmentOrders != null && _context.ShipmentOrders.Any())
            {
                valueMax = await _context.ShipmentOrders.MaxAsync(e => e.Id);
            }
            return valueMax == 0 ? 1 : valueMax + 1;
        }

        private async Task<ShipmentOrder> SaveOrderHeader(ShipmentOrderRequest order, Guid applicantId)
        {
            var orderId = await GetNextOrderIdAsync();
            var fromCityId = await _getCity.GetAsync(order.Route?.PickUp?.CityCode ?? string.Empty);
            var toCityId = await _getCity.GetAsync(order.Route?.DropOff?.CityCode ?? string.Empty);

            if (fromCityId == null)
            {
                throw new ArgumentException($"From city '{order.Route?.PickUp?.CityCode}' was not found");
            }

            if (toCityId == null)
            {
                throw new ArgumentException($"To city '{order.Route?.DropOff?.CityCode}' was not found");
            }

            var shipmentOrder = new ShipmentOrder
            {
                Id = orderId,
                PickUpCityId = fromCityId.Id,
                PickUpAddress = order.Route?.PickUp?.Address,
                DropOffCityId = toCityId.Id,
                DropOffAddress = order.Route?.DropOff?.Address,
                InitialPrice = order.BasePrice,
                Taxes = order.Taxes,
                TotalPrice = order.Total,
                PaymentState = PaymentStates.UnPaid,
                ShipmentState = ShipmentStates.Ordered,
                TransporterId = null,
                CustomerId = applicantId,
                ApplicationDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                ModifyUserId = applicantId,
                SenderDocumentType = order.Sender?.DocumentType,
                SenderDocumentId = order.Sender?.DocumentId.ToString(),
                SenderFirstName = order.Sender?.FirstName,
                SenderLastName = order.Sender?.LastName,
                SenderEmail = order.Sender?.Email,
                SenderCountryCode = order.Sender?.CountryCode.ToString(),
                SenderPhone = order.Sender?.Phone,
                RecipientDocumentType = order.Recipient?.DocumentType,
                RecipientDocumentId = order.Recipient?.DocumentId.ToString(),
                RecipientFirstName = order.Recipient?.FirstName,
                RecipientLastName = order.Recipient?.LastName,
                RecipientEmail = order.Recipient?.Email,
                RecipientCountryCode = order.Recipient?.CountryCode.ToString(),
                RecipientPhone = order.Recipient?.Phone
            };

            if (_context.ShipmentOrders == null)
            {
                throw new ArgumentException("Shipment Order context is invalid");
            }

            await _context.ShipmentOrders.AddAsync(shipmentOrder);

            return shipmentOrder;
        }

        private async Task SaveOrderItems(ShipmentOrderRequest order, long orderId)
        {
            var items = order.Items!.Select(detail =>
                    new ShipmentOrderItem
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderId,
                        Width = detail.Width,
                        Height = detail.Height,
                        Weight = detail.Weight,
                        Length = detail.Length,
                        InsuredAmount = detail.InsuredAmount,
                        IsUrgent = detail.IsUrgent,
                        IsFragile = detail.IsFragile
                    })
                .ToList();

            await _context.ShipmentOrderItems!.AddRangeAsync(items);
        }
    }
}