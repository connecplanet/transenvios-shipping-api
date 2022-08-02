using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.CatalogService.CityPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.CodeConfigurationOrderService.CodeConfigurationOrderPage
{
    public class CityMediator : IGetCatalog<City>
    {
        private readonly DataContext _context;

        public CityMediator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<City>> GetAllAsync()
        {
            return await _context.Cities.ToListAsync();
        }
    }
}
