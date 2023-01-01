using Transenvios.Shipping.Api.Domains.CatalogService;

namespace Transenvios.Shipping.Api.Domains.RoutesService
{
    public interface IRouteStorage
    {
        Task<IList<ShipmentRoute>> GetAllAsync();
        Task<bool> Exists(string fromCityCode, string toCityCode);
        Task<ShipmentRoute> GetAsync(string email);
        Task<int> UpdateAsync(ShipmentRoute client);
        Task<int> DeleteAsync(ShipmentRoute client);
        Task<int> RegisterAsync(ShipmentRoute client);
        Task<ShipmentRoute> GetAsync(Guid id);
    }
}
