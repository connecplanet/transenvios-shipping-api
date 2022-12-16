using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.ClientService.ClientPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.ClientService.ClientPage
{
    public class ClientMediator : IClients
    {

        private readonly DataContext _context;


        public ClientMediator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public Task<bool> ExistsEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public Task<Client> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
