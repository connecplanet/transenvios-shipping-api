namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class ShipmentOrderListResponse
    {
        public ShipmentOrderListPaginationResponse Pagination { get; set; }
        public IList<ShipmentOrderListItemResponse> Items { get; set; }
    }
}