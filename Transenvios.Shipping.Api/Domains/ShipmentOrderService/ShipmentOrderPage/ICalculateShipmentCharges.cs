using Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public interface ICalculateShipmentCharges
    {
        decimal CalculateChargeByWeight(ShipmentRoute route, decimal weight);
        decimal CalculateChargeByVolume(ShipmentRoute route, decimal height, decimal length, decimal width);
        decimal CalculateInitialPayment(ShipmentRoute route, ShipmentOrderItem item);
        decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItem item, decimal basePrice);
        ShipmentOrderResponse CalculateShipmentCharges(ShipmentRoute route, ShipmentOrderRequest order);
        Task<int> SaveShipmentChargesAsync(ShipmentOrderRequest order);
    }
}
