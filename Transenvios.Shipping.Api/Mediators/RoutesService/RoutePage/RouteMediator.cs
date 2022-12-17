using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage;
using Transenvios.Shipping.Api.Domains.RoutesService.RoutesPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.RoutesService.RoutePage
{
    public class RouteMediator: IRoutes
    {

        private readonly DataContext _context;
        public RouteMediator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsRout(string fromCityCode, string toCityCode)
        {

            return await _context.Routes.AnyAsync(x => x.FromCityCode == fromCityCode && x.ToCityCode == toCityCode);
        }

        public async Task<IList<ShipmentRoute>> GetAllAsync()
        {
            return await _context.Routes.ToListAsync();
        }

        public Task<ShipmentRoute> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ShipmentRoute> GetByIdAsync(Guid id)
        {

            return await _context.Routes.FindAsync(id);
        }

        public async Task<int> RegisterAsync(ShipmentRoute route)
        {
            await _context.Routes.AddAsync(route);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(ShipmentRoute route)
        {
            _context.Routes.Remove(route);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(ShipmentRoute route)
        {
            _context.Routes.Update(route);
            return await _context.SaveChangesAsync();
        }
    }
}
