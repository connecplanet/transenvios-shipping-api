using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrderMediator : ICalculateShipmentCharges
    {
        private readonly ShipmentSettings _settings;

        private readonly DataContext _context;

        public ShipmentOrderMediator(IOptions<AppSettings> appSettings, DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _settings = appSettings.Value.Shipment ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public decimal CalculateChargeByWeight(ShipmentRoute route, decimal weight)
        {
            decimal price = (route.InitialKiloPrice ?? 0) +
                ((weight - 1) * (route.AdditionalKiloPrice ?? 0));
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


        public async Task<int> SaveShipmentChargesAsync(ShipmentOrderRequest order)
        {
            try
            {
                var priceService = new ShipmentOrderResponse();

                Domains.ClientService.ClientPage.Client data = new Domains.ClientService.ClientPage.Client();

                if (order.Sender.isClient)
                {
                    data.DocumentId = order.Sender.DocumentId;
                    data.Phone = order.Sender.Phone;
                    data.Email = order.Sender.Email;
                    data.FirstName = order.Sender.FirstName;
                    data.DocumentType = order.Sender.DocumentType;
                    data.LastName = order.Sender.LastName;
                    data.CountryCode = order.Sender.CountryCode;
                    data.PasswordHash = "prueba Sender";
                }
                else
                if (order.Recipient.isClient)
                {
                    data.DocumentId = order.Recipient.DocumentId;
                    data.Phone = order.Recipient.Phone;
                    data.Email = order.Recipient.Email;
                    data.FirstName = order.Recipient.FirstName;
                    data.DocumentType = order.Recipient.DocumentType;
                    data.LastName = order.Recipient.LastName;
                    data.CountryCode = order.Recipient.CountryCode;
                    data.PasswordHash = "prueba Sender";
                }
                data.Role = "C";

                await _context.Clients.AddAsync(data);

                int idShipmenORders = GetByIdShipmentOrdersAsync();
                ShipmentOrder shipmentOrder = new ShipmentOrder();
                shipmentOrder.Id = idShipmenORders;
                shipmentOrder.PickUpCityId = order.Route.PickUp.CityCode;
                shipmentOrder.PickUpAddress = order.Route.PickUp.Address;
                shipmentOrder.DropOffCityId = order.Route.DropOff.CityCode;
                shipmentOrder.DropOffAddress = order.Route.DropOff.Address;
                shipmentOrder.InitialPrice = order.BasePrice;
                shipmentOrder.Taxes = order.Taxes;
                shipmentOrder.TotalPrice = order.Total;
                shipmentOrder.PaymentState = "NA";
                shipmentOrder.ShipmentState = "NA";
                shipmentOrder.TransporterId = "NA";


//                shipmentOrder.ApplicantId
//shipmentOrder.ApplicationDate = DateTime.Now;
//shipmentOrder.ModifyDate = DateTime.Now;
//                shipmentOrder.ModifyUserId

shipmentOrder.SenderDocumentType = order.Sender.DocumentType;
                shipmentOrder.SenderDocumentId = Convert.ToInt32(order.Sender.DocumentId);
                shipmentOrder.SenderFirstName = order.Sender.FirstName;
                shipmentOrder.SenderLastName = order.Sender.LastName;
                shipmentOrder.SenderEmail = order.Sender.Email;
                shipmentOrder.SenderCountryCode = Convert.ToInt32(order.Sender.CountryCode);
                shipmentOrder.SenderPhone = order.Sender.Phone;


shipmentOrder.RecipientDocumentType = order.Recipient.DocumentType;
                shipmentOrder.RecipientDocumentId = Convert.ToInt32(order.Recipient.DocumentId);
                shipmentOrder.RecipientFirstName = order.Recipient.FirstName;
                shipmentOrder.RecipientLastName = order.Recipient.LastName;
                shipmentOrder.RecipientEmail = order.Recipient.Email;
                shipmentOrder.RecipientCountryCode = Convert.ToInt32(order.Recipient.CountryCode);
                shipmentOrder.RecipientPhone = order.Recipient.Phone;


                await _context.ShipmentOrders.AddAsync(shipmentOrder);

                int shipmentOrderItems = GetByShipmentOrderItemsAsync();

                List<ShipmentOrderItem> shipmentOrderItemList = new List<ShipmentOrderItem>();
                foreach (var item in order.Items)
                {
                    ShipmentOrderItem shipmentOrderItem = new ShipmentOrderItem();
                    shipmentOrderItem.IdOrder = idShipmenORders;
                    shipmentOrderItem.Width = item.Width;
                    shipmentOrderItem.Weight = item.Weight;
                    shipmentOrderItem.Height = item.Height;
                    shipmentOrderItem.IsUrgent = item.IsUrgent;
                    shipmentOrderItem.IsFragile = item.IsFragile;
                    shipmentOrderItem.Length = item.Length;
                    shipmentOrderItem.InsuredAmount = item.InsuredAmount;
                    shipmentOrderItemList.Add(shipmentOrderItem);
                }

                await _context.ShipmentOrderItems.AddRangeAsync(shipmentOrderItemList);

                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        public int GetByIdShipmentOrdersAsync()
        {
            var valueMax = _context.ShipmentOrders.Max(e => e.Id);
            if (valueMax == 0)
            {
                valueMax = 1;
            }
            else
            {
                valueMax = valueMax + 1;
            }
            return valueMax;
        }
        public int GetByShipmentOrderItemsAsync()
        {
            var valueMax = _context.ShipmentOrderItems.Max(e => e.Id);
            if (valueMax == 0)
            {
                valueMax = 1;
            }
            else
            {
                valueMax = valueMax++;
            }
            return valueMax;
        }
    }
}