namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class ShipmentOrderListPaginationResponse
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
    }
}
