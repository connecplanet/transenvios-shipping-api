namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public class ShipmentOrderItem: ShipmentOrderItemRequest
    {
        public Guid Id { get; set; }
        public int IdOrder { get; set; }
    }
}
