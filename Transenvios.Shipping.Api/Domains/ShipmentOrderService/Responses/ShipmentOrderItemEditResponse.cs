namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class ShipmentOrderItemEditResponse
    {
        public Guid Id { get; set; }
        public string? Height { get; set; }
        public string? Length { get; set; }
        public string? Width { get; set; }
        public string? Weight { get; set; }
        public int Quantity { get; set; }
        public bool IsFragile { get; set; }
        public bool IsUrgent { get; set; }
    }
}
