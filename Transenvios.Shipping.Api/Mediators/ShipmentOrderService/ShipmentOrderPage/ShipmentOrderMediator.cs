using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrderMediator : ICalculateShipmentPrice
    {
        private readonly ShipmentSettings _settings;

        public ShipmentOrderMediator(IOptions<AppSettings> appSettings)
        {
            _settings = appSettings.Value.Shipment ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public decimal CalculateChargesByWeight(ShipmentRoute route, decimal weight)
        {
            decimal price = (route.InitialKiloPrice ?? 0) + 
                ((weight - 1) * (route.AdditionalKiloPrice ?? 0));
            return price;
        }

        public decimal CalculateChargesByVolume(ShipmentRoute route, decimal height, decimal length, decimal width)
        {
            decimal volume = height * length * width;
            decimal price = volume * (route.PriceCm3 ?? 0);
            return price;
        }

        public decimal CalculateBasePrice(ShipmentRoute route, ShipmentOrderItem item)
        {
            var chargesByWeight = CalculateChargesByWeight(route, item.Weight??0);
            var chargesByVolume = CalculateChargesByVolume(
                route, item.Height??0, item.Length??0, item.Weight??0);
            return chargesByWeight > chargesByVolume ? chargesByWeight : chargesByVolume;
        }

        public decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItem item, decimal initialCharge)
        {
            var insuredAmount = item.InsuredAmount > 0 ? item.InsuredAmount * (_settings.InsuredAmountRatio / 100M) : 0;
            var urgentAmount = item.IsUrgent ? initialCharge * (_settings.UrgentAmountRatio / 100M) : 0;
            var fragileAmount = item.IsFragile ? initialCharge * (_settings.FragileAmountRatio / 100M) : 0;
            return (insuredAmount ?? 0) + urgentAmount + fragileAmount;
        }

        public ShipmentOrderResponse CalculatePriceService(ShipmentRoute route, ShipmentOrderRequest order)
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
                var basePrice = CalculateBasePrice(route, orderItem);
                var additionalCharges = CalculateAdditionalCharges(route, orderItem, basePrice);
                priceService.BasePrice += basePrice + additionalCharges;
            }
            
            priceService.Taxes = priceService.BasePrice * (_settings.TaxAmountRatio / 100M);
            priceService.Total = priceService.BasePrice + priceService.Taxes;

            return priceService;
        }
    }
}