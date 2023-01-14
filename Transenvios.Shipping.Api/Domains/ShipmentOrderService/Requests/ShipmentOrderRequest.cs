namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests
{
    public class ShipmentOrderRequest
    {
        public ShipmentOrderRouteRequest? Route { get; set; }

        public IList<ShipmentOrderItemRequest>? Items { get; set; }

        public ShipmentOrderPersonRequest? Sender { get; set; }

        public ShipmentOrderPersonRequest? Recipient { get; set; }

        public long BasePrice { get; set; }

        public long Taxes { get; set; }

        public long Total { get; set; }
        public string? ApplicantEmail { get; set; }
    }
}
