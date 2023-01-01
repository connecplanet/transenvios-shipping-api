namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public class ShipmentOrderItem
    {
        public Guid Id { get; set; }
        public int IdOrder { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? InsuredAmount { get; set; }
        public bool IsFragile { get; set; }
        public bool IsUrgent { get; set; }
    }
}
