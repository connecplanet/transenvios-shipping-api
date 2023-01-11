using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.DriverService
{
    public class DriverMediator : IDriverStorage
    {
        private readonly IDbContext _context;

        public DriverMediator(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<Driver>> GetAllAsync()
        {
            return await _context.Drivers.ToListAsync();
        }
        
        public async Task<int> UpdateAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            return await _context.SaveChangesAsync();
        }

        public async Task<Driver> GetAsync(Guid id)
        {
            return await _context.Drivers.AsNoTracking().FirstOrDefaultAsync(driver => driver.Id.Equals(id));
        }

        public async Task<bool> Exists(string email)
        {
            return await _context.Drivers.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> Exists(long? document)
        {
            var documentId = document.ToString();
            return await _context.Drivers.AnyAsync(x => x.DocumentId == documentId);
        }

        public async Task<int> AddAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Driver driver)
        {
            _context.Drivers.Remove(driver);
            return await _context.SaveChangesAsync();
        }
    }
}
