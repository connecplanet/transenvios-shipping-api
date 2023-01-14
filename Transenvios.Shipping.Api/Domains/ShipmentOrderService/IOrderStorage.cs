using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public interface IOrderStorage
    {
        Task<ShipmentOrderSubmitResponse> SubmitOrderAsync(ShipmentOrderRequest? order);

        Task<ShipmentOrderListResponse> GetShipmentListAsync(int offset, int skip);
    }
}
