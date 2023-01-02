using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.CatalogService
{
    public class CityMediator : ICatalogStorage<City>
    {
        private readonly DataContext _context;

        public CityMediator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<City>> GetAllAsync()
        {
            return await _context.Cities!.ToListAsync();
        }

        public async Task<City> GetAsync(Guid id)
        {
            return await _context.Cities!.FindAsync(id);
        }

        public async Task<int> AddAsync(City entity)
        {
            await _context.Cities!.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(City entity)
        {
            _context.Cities!.Update(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(City entity)
        {
            _context.Cities!.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public bool Exists(Guid id)
        {
            return _context.Cities!.Any(e => e.Id == id);
        }

        public bool Exists(Guid id, string code)
        {
            return _context.Cities!.Any(e => e.Id != id && e.Code == code);
        }

        public async Task<City> GetAsync(string code)
        {
            return (await _context.Cities!.Where(e => e.Code == code).FirstOrDefaultAsync())!;
        }
    }
}
