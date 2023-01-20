namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class ShipmentOrderListItemResponse
    {
        public long? Id { get; set; }
        public string? ApplicationDate { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? FromCity { get; set; }
        public string? ToCity { get; set; }
        public string? PaymentState { get; set; }
        public string? TransporterName { get; set; }
        public Guid? TransporterId { get; set; }
        public string? ShipmentState { get; set; }
        public string? ShipmentPrice { get; set; }
    }
}
