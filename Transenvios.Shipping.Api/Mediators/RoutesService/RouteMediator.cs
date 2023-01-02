using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.RoutesService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.RoutesService
{
    public class RouteMediator : IRouteStorage
    {

        private readonly DataContext _context;
        public RouteMediator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Exists(string fromCityCode, string toCityCode)
        {
            return await _context.Routes!
                .AnyAsync(x => x.FromCityCode == fromCityCode && x.ToCityCode == toCityCode);
        }

        public async Task<IList<ShipmentRoute>> GetAllAsync()
        {
            var routes = await _context.Routes!.ToListAsync();
            routes.ForEach(e =>
            {
                e.InitialKiloPrice = Math.Round(e.InitialKiloPrice ?? 0, 2);
                e.AdditionalKiloPrice = Math.Round(e.AdditionalKiloPrice ?? 0, 2);
                e.PriceCm3 = Math.Round(e.PriceCm3 ?? 0, 2);
            });
            return routes;
        }

        public async Task<ShipmentRoute> GetAsync(string fromCityCode, string toCityCode)
        {
            var route = await _context.Routes!
                .Where(e => e.FromCityCode == fromCityCode && e.ToCityCode == toCityCode)
                .FirstOrDefaultAsync();

            if (route == null)
            {
                return route!;
            }

            route.InitialKiloPrice = Math.Round(route.InitialKiloPrice ?? 0, 2);
            route.AdditionalKiloPrice = Math.Round(route.AdditionalKiloPrice ?? 0, 2);
            route.PriceCm3 = Math.Round(route.PriceCm3 ?? 0, 2);

            return route;
        }

        public async Task<ShipmentRoute> GetAsync(Guid id)
        {
            return await _context.Routes!.FindAsync(id);
        }

        public async Task<int> AddAsync(ShipmentRoute route)
        {
            await _context.Routes!.AddAsync(route);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(ShipmentRoute route)
        {
            _context.Routes!.Remove(route);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(ShipmentRoute route)
        {
            _context.Routes!.Update(route);
            return await _context.SaveChangesAsync();
        }
    }
}
