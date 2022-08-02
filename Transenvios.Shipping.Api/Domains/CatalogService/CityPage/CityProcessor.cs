
namespace Transenvios.Shipping.Api.Domains.CatalogService.CityPage
{
    public class CityProcessor
    {
        private readonly IGetCityCatalog _getShipmentCity;
        
        public CityProcessor(IGetCityCatalog getShipmentCity)
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
