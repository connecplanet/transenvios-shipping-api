namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    using Entities;
    using Requests;
    using Responses;

    public interface IOrderStorage
    {
        Task<ShipmentOrderSubmitResponse> SubmitAsync(ShipmentOrderRequest? order);
        Task<ShipmentOrderListResponse> GetAllAsync(DateTime startDate, DateTime endDate);
        Task<ShipmentOrderEditResponse?> GetOneAsync(long id);
        Task<ShipmentOrder?> GetAsync(long id);
        Task<int> UpdateAsync(ShipmentOrder order);
        Task<int> DeleteAsync(long orderId);
    }
}
