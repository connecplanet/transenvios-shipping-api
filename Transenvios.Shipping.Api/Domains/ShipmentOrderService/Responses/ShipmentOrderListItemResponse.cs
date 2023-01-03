namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class ShipmentOrderListItemResponse
    {
        public int OrderId { get; set; }
        public string ApplicantName { get; set; }
        public string Phone { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string PaymentState { get; set; }
        public string TransporterName { get; set; }
        public string TransporterId { get; set; }
        public string ShipmentState { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
