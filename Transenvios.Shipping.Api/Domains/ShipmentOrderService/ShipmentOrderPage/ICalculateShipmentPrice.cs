using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public interface ICalculateShipmentPrice
    {
        decimal CalculatePriceByWeight(ShipmentRoute route, decimal weight);
        decimal CalculatePriceByVolume(ShipmentRoute route, decimal height, decimal length, decimal width);
        decimal CalculateInitialPrice(ShipmentRoute route, ShipmentOrderItem package);
        decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItem package, ShipmentConfig config, decimal initialPrice);
    }
}
