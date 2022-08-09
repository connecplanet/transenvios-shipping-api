using Transenvios.Shipping.Api.Domains.CatalogService.CityPage;
using Transenvios.Shipping.Api.Domains.CatalogService.CountryPage;
using Transenvios.Shipping.Api.Domains.CatalogService.IdTypePage;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class CatalogResponse
    {
        public IList<City>? Cities { get; set; }
        public IList<Country>? Countries { get; set; }
        public IList<IdType>? IdTypes { get; set; }
    }
}
