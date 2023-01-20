using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests
{
    public class ShipmentUpdateRequest
    {
        public long Id { get; set; }
        public PaymentStates PaymentState { get; set; }
        public ShipmentStates ShipmentState { get; set; }
        public Guid? TransporterId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
