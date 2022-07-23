namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentRoute : BaseEntity<Guid>
    {
        public string? FromCity { get; set; }
        public string? ToCity { get; set; }
        public decimal? InitialPriceKg { get; set; }
        public decimal? AdditionalPriceKg { get; set; }
        public decimal? PriceCm3 { get; set; }
        public bool? Active { get; set; }
    }
}
