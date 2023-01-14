using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.ClientService
{
    public class ClientMediator : IClientMediator
    {
        private readonly IDbContext _context;

        public ClientMediator(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Exists(string email)
        {
            return await _context.Clients!.AnyAsync(x => x.Email == email);
        }

        public async Task<IList<Client>> GetAllAsync()
        {
            return await _context.Clients!.ToListAsync();
        }

        public async Task<Client> GetAsync(Guid id)
        {
            return (await _context.Clients!.FindAsync(id))!;
        }

        public async Task<Client> GetAsync(string email)
        {
            return (await _context.Clients!.SingleOrDefaultAsync(x => x.Email == email))!;
        }

        public async Task<int> AddAsync(Client client)
        {
            await _context.Clients!.AddAsync(client);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Client client)
        {
            _context.Clients!.Remove(client);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Client client)
        {
            _context.Clients!.Update(client); 
            return await _context.SaveChangesAsync();
        }
    }
}
