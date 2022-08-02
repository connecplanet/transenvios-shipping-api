using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.CodeConfigurationOrderService.CodeConfigurationOrderPage
{
    public class CodeConfigurationOrderMediator : IGetShipmentCity
    {
        private readonly DataContext _context;

        public CodeConfigurationOrderMediator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<ShipmentCity>> GetCityAllAsync()
        {
            return await _context.Cities.ToListAsync();
        }
    }
}
