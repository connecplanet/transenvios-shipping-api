namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public class ShipmentRoute : BaseEntity<Guid>
    {
        public string? FromCityCode { get; set; }
        public string? ToCityCode { get; set; }
        public decimal? InitialKiloPrice { get; set; }
        public decimal? AdditionalKiloPrice { get; set; }
        public decimal? PriceCm3 { get; set; }
        public bool? Active { get; set; } = true;
    }
}