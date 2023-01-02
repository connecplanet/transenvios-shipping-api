namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public interface IOrderStorage
    {
        Task<ShipmentOrderResponse> SubmitOrderAsync(ShipmentOrderRequest? order);
    }
}
