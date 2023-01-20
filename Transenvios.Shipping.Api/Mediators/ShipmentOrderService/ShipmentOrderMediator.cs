using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.ShipmentOrderService
{
    public class ShipmentOrderMediator : IOrderChargesCalculator, IOrderStorage
    {
        private readonly ShipmentSettings _settings;
        private readonly IDbContext _context;
        private readonly ICatalogStorage<City> _getCity;
        private readonly IClientMediator _clientMediator;

        public ShipmentOrderMediator(
            IOptions<AppSettings> appSettings,
            IDbContext context,
            ICatalogStorage<City> getCity,
            IClientMediator clientMediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _settings = appSettings.Value.Shipment ?? throw new ArgumentNullException(nameof(appSettings));
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

        public async Task<ShipmentOrderSubmitResponse> SubmitAsync(ShipmentOrderRequest? order)
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

                orderResponse.Id = shipmentOrder.Id;
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

        public async Task<ShipmentOrderListResponse> GetAllAsync(DateTime startDate, DateTime endDate)
        {
            var totalRows = await _context.ShipmentOrders!
                .Where(o => o.ApplicationDate >= startDate && o.ApplicationDate < endDate)
                .CountAsync();
            var response = new ShipmentOrderListResponse
            {
                Pagination = new ShipmentOrderListPaginationResponse
                {
                    Total = totalRows,
                    Filter = startDate.ToString("yyyy-MM-dd")
                }
            };

            var orders = await _context.ShipmentOrders!
                .Include(b => b.Customer)
                .Include(b => b.PickUpCity)
                .Include(b => b.DropOffCity)
                .Include(b => b.Transporter)
                .Where(o => o.ApplicationDate >= startDate && o.ApplicationDate < endDate)
                .Select(order => new ShipmentOrderListItemResponse
                {
                    Id = order.Id,
                    ApplicationDate = order.ApplicationDate.ToString("yyyy-MM-dd"),
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

        public async Task<ShipmentOrderEditResponse?> GetOneAsync(long id)
        {
            var order = await GetShipmentOrderEditHeader(id);
            await GetShipmentOrderEditPackages(id, order);

            return order;
        }

        public async Task<ShipmentOrder?> GetAsync(long id)
        {
            return (await _context.ShipmentOrders!.FindAsync(id))!;
        }

        public async Task<int> UpdateAsync(ShipmentOrder order)
        {
            _context.ShipmentOrders!.Update(order);
            return await _context.SaveChangesAsync();
        }

        private async Task GetShipmentOrderEditPackages(long id, ShipmentOrderEditResponse? order)
        {
            if (order != null)
            {
                order.Packages = await _context.ShipmentOrderItems!
                    .Where(o => o.OrderId == id)
                    .Select(item => new ShipmentOrderItemEditResponse
                    {
                        Height = $"{item.Height:#,###} cm",
                        Width = $"{item.Width:#,###} cm",
                        Length = $"{item.Length:#,###} cm",
                        Weight = $"{item.Weight:#,###} Kg",
                        IsFragile = item.IsFragile,
                        Quantity = 1
                    }).ToListAsync();
            }
        }

        private async Task<ShipmentOrderEditResponse?> GetShipmentOrderEditHeader(long id)
        {
            var order = await _context.ShipmentOrders!
                .Include(b => b.Customer)
                .Include(b => b.PickUpCity)
                .Include(b => b.DropOffCity)
                .Include(b => b.Transporter)
                .Where(o => o.Id == id)
                .Select(order => new ShipmentOrderEditResponse
                {
                    OrderId = order.Id,
                    ApplicationDate = order.ApplicationDate.ToString("dd-MM-yyyy"),
                    PaymentState = order.PaymentState,
                    ShipmentState = order.ShipmentState,
                    TransporterId = order.TransporterId,
                    InitialPrice = order.InitialPrice!.Value.ToString("#,###"),
                    Taxes = order.Taxes.ToString("#,###"),
                    TotalPrice = order.TotalPrice.ToString("#,###"),
                    Customer = new PersonResponse
                    {
                        DocumentType = order.Customer!.DocumentType,
                        DocumentId = order.Customer!.DocumentId,
                        FirstName = order.Customer!.FirstName,
                        LastName = order.Customer!.LastName,
                        Phone = order.Customer!.Phone,
                        Email = order.Customer!.Email
                    },
                    Sender = new PersonShipmentResponse
                    {
                        DocumentType = order.SenderDocumentType,
                        DocumentId = order.SenderDocumentId,
                        FirstName = order.SenderFirstName,
                        LastName = order.SenderLastName,
                        Phone = order.SenderPhone,
                        Email = order.SenderEmail,
                        CityName = order.PickUpCity!.Name,
                        Address = order.PickUpAddress
                    },
                    Recipient = new PersonShipmentResponse
                    {
                        DocumentType = order.RecipientDocumentType,
                        DocumentId = order.RecipientDocumentId,
                        FirstName = order.RecipientFirstName,
                        LastName = order.RecipientLastName,
                        Phone = order.RecipientPhone,
                        Email = order.RecipientEmail,
                        CityName = order.DropOffCity!.Name,
                        Address = order.DropOffAddress
                    }
                })
                .FirstOrDefaultAsync();
            return order;
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

            if (string.Equals(customer.DocumentType ?? string.Empty, customerDocumentType ?? string.Empty) &&
                string.Equals(customer.DocumentId ?? string.Empty, customerDocumentId ?? string.Empty))
            {
                return 0;
            }

            customer.DocumentType = customerDocumentType;
            customer.DocumentId = customerDocumentId;
            return await _clientMediator.UpdateAsync(customer);
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
                SenderDocumentId = order.Sender?.DocumentId!.ToString(),
                SenderFirstName = order.Sender?.FirstName,
                SenderLastName = order.Sender?.LastName,
                SenderEmail = order.Sender?.Email,
                SenderCountryCode = order.Sender?.CountryCode!.ToString(),
                SenderPhone = order.Sender?.Phone,
                RecipientDocumentType = order.Recipient?.DocumentType,
                RecipientDocumentId = order.Recipient?.DocumentId!.ToString(),
                RecipientFirstName = order.Recipient?.FirstName,
                RecipientLastName = order.Recipient?.LastName,
                RecipientEmail = order.Recipient?.Email,
                RecipientCountryCode = order.Recipient?.CountryCode!.ToString(),
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