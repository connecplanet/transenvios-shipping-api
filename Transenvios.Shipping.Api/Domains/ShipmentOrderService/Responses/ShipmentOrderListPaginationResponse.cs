namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class ShipmentOrderListPaginationResponse
    {
        public string? Filter { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
    }
}
