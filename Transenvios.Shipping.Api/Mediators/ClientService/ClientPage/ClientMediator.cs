using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.ClientService.ClientPage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
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


        public async Task<bool> ExistsEmail(string email)
        {

            return await _context.Clients.AnyAsync(x => x.Email == email);
        }

        public async Task<IList<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<Client> GetByEmailAsync(string email)
        {
            return await _context.Clients.SingleOrDefaultAsync(x => x.Email == email);

        }

        public async Task<int> RegisterAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(Client client)
        {
            _context.Clients.Remove(client);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Client client)
        {
            _context.Clients.Update(client);
            return await _context.SaveChangesAsync();
        }

        

    }
}
