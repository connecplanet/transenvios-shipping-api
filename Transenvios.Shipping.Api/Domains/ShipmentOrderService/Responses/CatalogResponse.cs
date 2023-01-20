using Transenvios.Shipping.Api.Domains.CatalogService;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class CatalogResponse
    {
        public IList<ItemCatalogResponse>? Cities { get; set; }
        public IList<ItemCatalogResponse>? Countries { get; set; }
        public IList<ItemCatalogResponse>? IdTypes { get; set; }
        public IList<RouteCatalogResponse>? Routes { get; set; }
    }
}
