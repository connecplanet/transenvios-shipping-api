using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities
{
    public class ShipmentOrderItem : ShipmentOrderItemRequest
    {
        public Guid Id { get; set; }
        public int IdOrder { get; set; }
    }
}
