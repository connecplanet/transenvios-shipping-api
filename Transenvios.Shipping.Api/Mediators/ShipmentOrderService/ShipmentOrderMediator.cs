using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses;
using Transenvios.Shipping.Api.Domains.UserService;
using Transenvios.Shipping.Api.Infraestructure;
using Transenvios.Shipping.Api.Mediators.ClientService;

namespace Transenvios.Shipping.Api.Mediators.ShipmentOrderService
{
    public class ShipmentOrderMediator : IOrderChargesCalculator, IOrderStorage
    {
        private readonly ShipmentSettings _settings;
        private readonly IDbContext _context;
        private readonly IGetUser _getUser;
        private readonly ICatalogStorage<City> _getCity;
        private readonly IClientMediator _clientMediator;

        public ShipmentOrderMediator(
            IOptions<AppSettings> appSettings,
            IDbContext context,
            IGetUser getUser,
            ICatalogStorage<City> getCity,
            IClientMediator clientMediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _settings = appSettings.Value.Shipment ?? throw new ArgumentNullException(nameof(appSettings));
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _getCity = getCity ?? throw new ArgumentNullException(nameof(getCity));
            _clientMediator = clientMediator ?? throw new ArgumentNullException(nameof(clientMediator));
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

        public async Task<ShipmentOrderSubmitResponse> SubmitOrderAsync(ShipmentOrderRequest? order)
        {
            var orderResponse = new ShipmentOrderSubmitResponse();

            try
            {
                if (order?.Sender == null || order.Recipient == null)
                {
                    throw new ArgumentException("Sender or Recipient is null");
                }

                var customer = await GetCustomer(order);

                orderResponse.BasePrice = order.BasePrice;
                orderResponse.Taxes = order.Taxes;
                orderResponse.Total = order.Total;

                var shipmentOrder = await SaveOrderHeader(order, customer.Id);
                await SaveOrderItems(order, shipmentOrder.Id);
                var itemsAffected = await _context.SaveChangesAsync();

                itemsAffected += await UpdateCustomer(order, customer);

                orderResponse.OrderId = shipmentOrder.Id;
                orderResponse.Items = itemsAffected;
            }
            catch (Exception error)
            {
                orderResponse.ErrorMessage = error.GetBaseException().Message;
            }

            return orderResponse;
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

        public async Task<ShipmentOrderListResponse> GetShipmentListAsync(int offset, int limit)
        {
            var currentPage = offset == 0 ? 1 : offset % limit;
            var totalRows = await _context.ShipmentOrders!.CountAsync();
            var response = new ShipmentOrderListResponse
            {
                Pagination = new ShipmentOrderListPaginationResponse
                {
                    Total = totalRows,
                    Page = currentPage,
                    Limit = limit
                }
            };

            var orders = await _context.ShipmentOrders!
                .Include(b => b.Customer)
                .Include(b => b.PickUpCity)
                .Include(b => b.DropOffCity)
                .Include(b => b.Transporter)
                .Skip(offset)
                .Take(limit)
                .Select(order => new ShipmentOrderListItemResponse
                {
                    OrderId = order.Id,
                    CustomerName = $"{order.Customer!.LastName}, {order.Customer!.FirstName}",
                    Phone = order.Customer!.Phone!,
                    FromCity = order.PickUpCity!.Name!,
                    ToCity = order.DropOffCity!.Name!,
                    PaymentState = order.PaymentState.GetEnumDescription(),
                    TransporterId = order.TransporterId,
                    TransporterName = order.Transporter != null 
                        ? $"{order.Transporter!.LastName}, {order.Transporter!.FirstName}"
                        : null,
                    ShipmentState = order.ShipmentState.GetEnumDescription(),
                    ShipmentPrice = order.TotalPrice.ToString("#,###")
                })
                .ToListAsync();

            response.Items = orders;
            response.Pagination.Count = orders.Count;

            return response;
        }

        private async Task<Client> GetCustomer(ShipmentOrderRequest order)
        {
            if (order?.Sender == null || order.Recipient == null)
            {
                throw new ArgumentException("Sender or Recipient is null");
            }

            var customerEmail = order.ApplicantEmail;
            if (string.IsNullOrEmpty(customerEmail))
            {
                if (order.Sender!.IsClient)
                {
                    customerEmail = order.Sender!.Email;
                }
                else if (order.Recipient!.IsClient)
                {
                    customerEmail = order.Recipient!.Email;
                }
            }

            if (string.IsNullOrEmpty(customerEmail))
            {
                throw new ArgumentException("Customer email is required");
            }

            var customer = await _clientMediator?.GetAsync(customerEmail)!;
            if (customer == null)
            {
                throw new ArgumentException("Customer Not Found");
            }

            return customer;
        }

        private async Task<int> UpdateCustomer(ShipmentOrderRequest order, Client customer)
        {
            var customerDocumentType = string.Empty;
            var customerDocumentId = string.Empty;

            if (order.Sender!.IsClient)
            {
                customerDocumentType = order.Sender.DocumentType;
                customerDocumentId = order.Sender.DocumentId;
            }
            else if (order.Recipient!.IsClient)
            {
                customerDocumentType = order.Recipient.DocumentType;
                customerDocumentId = order.Recipient.DocumentId;
            }

            if (!string.Equals(customer.DocumentType ?? string.Empty, customerDocumentType ?? string.Empty) ||
                !string.Equals(customer.DocumentId ?? string.Empty, customerDocumentId ?? string.Empty))
            {
                customer.DocumentType = customerDocumentType;
                customer.DocumentId = customerDocumentId;
                return await _clientMediator.UpdateAsync(customer);
            }

            return 0;
        }

        private async Task<ShipmentOrder> SaveOrderHeader(ShipmentOrderRequest order, Guid customerId)
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
                CustomerId = customerId,
                ApplicationDate = DateTime.Now,
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