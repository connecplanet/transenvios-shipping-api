using Transenvios.Shipping.Api.Domains.CatalogService;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public interface ICalculateShipmentCharges
    {
        decimal CalculateChargeByWeight(ShipmentRoute route, decimal weight);
        decimal CalculateChargeByVolume(ShipmentRoute route, decimal height, decimal length, decimal width);
        decimal CalculateInitialPayment(ShipmentRoute route, ShipmentOrderItem item);
        decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItem item, decimal basePrice);
        ShipmentOrderResponse CalculateShipmentCharges(ShipmentRoute route, ShipmentOrderRequest order);
        Task<ShipmentOrderResponse> SaveShipmentChargesAsync(ShipmentOrderRequest? order);
    }
}
