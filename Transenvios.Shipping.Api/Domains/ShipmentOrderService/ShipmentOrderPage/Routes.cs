namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class Routes : BaseEntity<Guid>
    {
        public string? CityFrom { get; set; }
        public string? CityTo { get; set; }
        public decimal? InitialPriceKg { get; set; }
        public decimal? AdditionalPriceKg { get; set; }
        public decimal? PriceCm3 { get; set; }
        public bool? Active { get; set; }
    }
}
