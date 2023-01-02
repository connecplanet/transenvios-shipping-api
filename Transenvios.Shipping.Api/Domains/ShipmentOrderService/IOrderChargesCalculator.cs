using Transenvios.Shipping.Api.Domains.CatalogService;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public interface IOrderChargesCalculator
    {
        decimal CalculateChargeByWeight(ShipmentRoute route, decimal weight);
        decimal CalculateChargeByVolume(ShipmentRoute route, decimal height, decimal length, decimal width);
        decimal CalculateInitialPayment(ShipmentRoute route, ShipmentOrderItemRequest item);
        decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItemRequest item, decimal basePrice);
        ShipmentOrderResponse CalculateCharges(ShipmentRoute route, ShipmentOrderRequest? order);
    }
}
