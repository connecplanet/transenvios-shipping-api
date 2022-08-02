using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.CatalogService.ShipmentRoutePage
{
    public class ShipmentRouteMediator : IGetCatalog<ShipmentRoute>
    {
        private readonly DataContext _context;

        public ShipmentRouteMediator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<ShipmentRoute>> GetAllAsync()
        {
            return await _context.Routes.ToListAsync();
        }
    }
}
