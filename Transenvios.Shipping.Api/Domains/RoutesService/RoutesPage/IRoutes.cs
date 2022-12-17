using Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage;
using Transenvios.Shipping.Api.Domains.ClientService.ClientPage;

namespace Transenvios.Shipping.Api.Domains.RoutesService.RoutesPage
{
    public interface IRoutes
    {
        Task<IList<ShipmentRoute>> GetAllAsync();
        Task<bool> ExistsRout(string fromCityCode,string toCityCode);
        Task<ShipmentRoute> GetByEmailAsync(string email);

        Task<int> UpdateAsync(ShipmentRoute client);

        Task<int> RemoveAsync(ShipmentRoute client);

        Task<int> RegisterAsync(ShipmentRoute client);

        Task<ShipmentRoute> GetByIdAsync(Guid id);
    }
}
