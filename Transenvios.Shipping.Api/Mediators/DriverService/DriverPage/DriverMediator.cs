using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Domains.DriverService.DriverPage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.DriverService.DriverPage
{
    public class DriverMediator : IDriver
    {
        private readonly DataContext _context;

        public DriverMediator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<Driver>> GetAllAsync()
        {
            return await _context.Drivers.ToListAsync();
        }
        public async Task<IList<Driver>> GetAllAsync(Guid id)
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task<int> UpdateAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            return await _context.SaveChangesAsync();
        }

        public async Task<Driver> GetByIdAsync(Guid id)
        {
            return await _context.Drivers.AsNoTracking().FirstOrDefaultAsync(driver => driver.Id.Equals(id));
        }

        public async Task<bool> ExistsEmail(string email)
        {
            return await _context.Drivers.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> ExistsDocument(long? document)
        {
            return await _context.Drivers.AnyAsync(x => x.DocumentId == document);
        }

        public async Task<int> RegisterAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(Driver driver)
        {
            _context.Drivers.Remove(driver);
            return await _context.SaveChangesAsync();
        }
    }
}
