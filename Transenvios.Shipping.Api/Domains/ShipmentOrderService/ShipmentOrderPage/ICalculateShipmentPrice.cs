namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public interface ICalculateShipmentPrice
    {
        decimal CalculateChargesByWeight(ShipmentRoute route, decimal weight);
        decimal CalculateChargesByVolume(ShipmentRoute route, decimal height, decimal length, decimal width);
        decimal CalculateBasePrice(ShipmentRoute route, ShipmentOrderItem item);
        decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItem item, decimal basePrice);
        ShipmentOrderResponse CalculatePriceService(ShipmentRoute route, ShipmentOrderRequest order);
    }
}
