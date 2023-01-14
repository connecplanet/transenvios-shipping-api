using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public interface IOrderChargesCalculator
    {
        decimal CalculateChargeByWeight(ShipmentRoute route, decimal weight);
        decimal CalculateChargeByVolume(ShipmentRoute route, decimal height, decimal length, decimal width);
        decimal CalculateInitialPayment(ShipmentRoute route, ShipmentOrderItemRequest item);
        decimal CalculateAdditionalCharges(ShipmentRoute route, ShipmentOrderItemRequest item, decimal basePrice);
        ShipmentOrderSubmitResponse CalculateCharges(ShipmentRoute route, ShipmentOrderRequest? order);
    }
}
