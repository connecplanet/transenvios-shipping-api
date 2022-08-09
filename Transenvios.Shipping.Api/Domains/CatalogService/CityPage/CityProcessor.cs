
namespace Transenvios.Shipping.Api.Domains.CatalogService.CityPage
{
    public class CityProcessor
    {
        private readonly IGetCatalog<City> _getShipmentCity;
        
        public CityProcessor(IGetCatalog<City> getShipmentCity)
        {
            _getShipmentCity = getShipmentCity 
                ?? throw new ArgumentNullException(nameof(getShipmentCity));
        }

        public async Task<IList<City>> GetCitiesAsync()
        {
            return await _getShipmentCity.GetAllAsync();
        }
    }
}
