using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class ShipmentOrderEditResponse
    {
        public long? OrderId { get; set; }
        public string? ApplicationDate { get; set; }
        public PaymentStates PaymentState { get; set; }
        public ShipmentStates ShipmentState { get; set; }
        public Guid? TransporterId { get; set; }
        public string? InitialPrice { get; set; }
        public string? Taxes { get; set; }
        public string? TotalPrice { get; set; }
        public PersonResponse? Customer { get; set; }
        public PersonShipmentResponse? Sender { get; set; }
        public PersonShipmentResponse? Recipient { get; set; }
        public List<ShipmentOrderItemEditResponse>? Packages { get; set; }
    }
}
