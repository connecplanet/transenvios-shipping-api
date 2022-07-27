using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrderMediator : ICalculateShipmentPrice
    {
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

        public decimal CalculateInitialPrice(ShipmentRoute route, ShipmentOrderItem package)
        {
            var priceByWeight = CalculatePriceByWeight(route, package.Weight);
            var priceByVolume = CalculatePriceByVolume(route, package.Height, package.Length, package.Weight);
            return priceByWeight > priceByVolume ? priceByWeight : priceByVolume;
        }

        public decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItem package, ShipmentConfig config, decimal initialPrice)
        {
            var insuredAmount = package.InsuredValue > 0 
                ? package.InsuredValue * config.InsuredAmountRatio / 100M 
                : 0;
            var urgentAmount = package.Urgent
                ? initialPrice * ( 1 + config.UrgentAmountRatio / 100M)
                : 0;
            var fragileAmount = package.Fragile
                ? initialPrice * (1 + config.FragileAmountRatio / 100M)
                : 0;
            return insuredAmount + urgentAmount + fragileAmount;
        }
    }
}