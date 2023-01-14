using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.CatalogService
{
    public class ShipmentRouteMediator : ICatalogQuery<ShipmentRoute>
    {
        private readonly IDbContext _context;

        public ShipmentRouteMediator(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<ShipmentRoute>> GetAllAsync()
        {
            return await _context.Routes.ToListAsync();
        }
    }
}
