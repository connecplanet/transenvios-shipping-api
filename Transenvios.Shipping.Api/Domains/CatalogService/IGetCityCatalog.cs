namespace Transenvios.Shipping.Api.Domains.CatalogService.CityPage
{
    public interface IGetCityCatalog
    {
        Task<IList<City>> GetAllAsync();
    }
}