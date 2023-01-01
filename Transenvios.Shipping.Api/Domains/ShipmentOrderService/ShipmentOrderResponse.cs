namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public class ShipmentOrderResponse
    {
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal Total { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
