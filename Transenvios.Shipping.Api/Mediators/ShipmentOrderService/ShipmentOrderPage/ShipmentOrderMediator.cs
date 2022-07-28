using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrderMediator : ICalculateShipmentPrice
    {
        private readonly ShipmentSettings _shipmentSettings;

        public ShipmentOrderMediator(IOptions<AppSettings> appSettings)
        {
            _shipmentSettings = appSettings.Value.Shipment ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public decimal CalculatePriceByWeight(ShipmentRoute route, decimal weight)
        {
            decimal price = (route.InitialKiloPrice ?? 0) + 
                ((weight - 1) * (route.AdditionalKiloPrice ?? 0));
            return price;
        }

        public decimal CalculatePriceByVolume(ShipmentRoute route, decimal height, decimal length, decimal width)
        {
            decimal volume = height * length * width;
            decimal price = volume * (route.PriceCm3 ?? 0);
            return price;
        }

        public decimal CalculateInitialPrice(ShipmentRoute route, ShipmentOrderItem orderItem)
        {
            var priceByWeight = CalculatePriceByWeight(route, orderItem.Weight??0);
            var priceByVolume = CalculatePriceByVolume(
                route, orderItem.Height??0, orderItem.Length??0, orderItem.Weight??0);
            return priceByWeight > priceByVolume ? priceByWeight : priceByVolume;
        }

        public decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItem orderItem, decimal initialPrice)
        {
            var insuredAmount = orderItem.InsuredValue > 0 
                ? orderItem.InsuredValue * _shipmentSettings.InsuredAmountRatio / 100M 
                : 0;
            var urgentAmount = orderItem.IsUrgent
                ? initialPrice * ( 1 + _shipmentSettings.UrgentAmountRatio / 100M)
                : 0;
            var fragileAmount = orderItem.IsFragile
                ? initialPrice * (1 + _shipmentSettings.FragileAmountRatio / 100M)
                : 0;
            return (insuredAmount ?? 0) + urgentAmount + fragileAmount;
        }

        public ShipmentOrderResponse CalculatePriceService(ShipmentRoute route, ShipmentOrderRequest order)
        {
            var priceService = new ShipmentOrderResponse
            {
                InitialPrice = 0M,
                Taxes = 0M,
                Total = 0M
            };

            if (order.Items == null)
            {
                return priceService;
            }
            
            foreach (var orderItem in order.Items)
            {
                var initialPrice = CalculateInitialPrice(route, orderItem);
                var additionalPrice = CalculateAdditionalCharges(route, orderItem, initialPrice);
                priceService.InitialPrice += initialPrice + additionalPrice;
            }
            
            priceService.Taxes = priceService.InitialPrice * _shipmentSettings.TaxAmountRatio / 100M;
            priceService.Total = priceService.InitialPrice + priceService.Taxes;

            return priceService;
        }
    }
}