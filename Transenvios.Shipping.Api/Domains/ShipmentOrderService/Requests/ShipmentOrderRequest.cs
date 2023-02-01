namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests
{
    public class ShipmentOrderRequest
    {
        public ShipmentOrderRouteRequest? Route { get; set; }
        public IList<ShipmentOrderItemRequest>? Items { get; set; }
        public ShipmentOrderPersonRequest? Sender { get; set; }
        public ShipmentOrderPersonRequest? Recipient { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal Total { get; set; }
        public string? ApplicantEmail { get; set; }
    }
}
