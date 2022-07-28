using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public interface ICalculateShipmentPrice
    {
        decimal CalculatePriceByWeight(ShipmentRoute route, decimal weight);
        decimal CalculatePriceByVolume(ShipmentRoute route, decimal height, decimal length, decimal width);
        decimal CalculateInitialPrice(ShipmentRoute route, ShipmentOrderItem orderItem);
        decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItem orderItem, decimal initialPrice);
        ShipmentOrderResponse CalculatePriceService(ShipmentRoute route, ShipmentOrderRequest order);
    }
}
