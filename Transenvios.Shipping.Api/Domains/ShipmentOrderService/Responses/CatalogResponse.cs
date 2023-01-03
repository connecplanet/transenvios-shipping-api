using Transenvios.Shipping.Api.Domains.CatalogService;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class CatalogResponse
    {
        public IList<City>? Cities { get; set; }
        public IList<Country>? Countries { get; set; }
        public IList<IdType>? IdTypes { get; set; }
        public IList<ShipmentRoute>? Routes { get; set; }
    }
}
