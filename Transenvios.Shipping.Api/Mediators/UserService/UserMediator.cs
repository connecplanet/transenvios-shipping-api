using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.UserService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.UserService
{
    public class UserMediator : IRegisterUser, IGetUser, IUpdateUser, IRemoveUser, IGetAuthorizeUser
    {
        private readonly IDbContext _context;

        public UserMediator(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> SignUpAsync(User user)
        {
            await _context.Users!.AddAsync(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<IList<User>> GetAllAsync()
        {
            return await _context.Users!.ToListAsync();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return (await _context.Users!.FindAsync(id))!;
        }

        public async Task<bool> Exists(string email)
        {
            return await _context.Users!.AnyAsync(x => x.Email == email);
        }

        public async Task<User> GetAsync(string email)
        {
            return (await _context.Users!.SingleOrDefaultAsync(x => x.Email == email))!;
        }

        public async Task<int> UpdateAsync(User user)
        {
            _context.Users!.Update(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(User user)
        {
            _context.Users!.Remove(user);
            return await _context.SaveChangesAsync();
        }
    }
}
