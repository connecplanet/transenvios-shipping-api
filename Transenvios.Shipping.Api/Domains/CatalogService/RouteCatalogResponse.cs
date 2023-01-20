namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public class RouteCatalogResponse
    {
        public Guid? Id { get; set; }
        public string? FromCityCode { get; set; }
        public string? ToCityCode { get; set; }
    }
}
