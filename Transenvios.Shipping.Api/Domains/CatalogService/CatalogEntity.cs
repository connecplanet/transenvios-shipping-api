namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public class CatalogEntity : BaseEntity<Guid>
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? Active { get; set; }
    }
}
