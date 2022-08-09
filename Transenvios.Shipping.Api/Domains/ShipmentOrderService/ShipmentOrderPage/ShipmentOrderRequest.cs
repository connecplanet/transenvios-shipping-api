namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrderRequest
    {
        public ShipmentOrderRoute? Route { get; set; }

        public IList<ShipmentOrderItem>? Items { get; set; }

        public ShipmentOrderPerson? Sender { get; set; }

        public ShipmentOrderPerson? Recipient { get; set; }

        public long BasePrice { get; set; }

        public long Taxes { get; set; }

        public long Total { get; set; }
    }
}
