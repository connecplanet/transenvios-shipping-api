using Transenvios.Shipping.Api.Domains.DriverService.DriverPage;

namespace Transenvios.Shipping.Api.Domains.DriverService
{
    public interface IDriver
    {
        Task<IList<Driver>> GetAllAsync();
        Task<Driver> GetByIdAsync(Guid id);
        Task<int> UpdateAsync(Driver driver);
        Task<int> RegisterAsync(Driver driver);
        Task<bool> ExistsEmail(string email);
        Task<bool> ExistsDocument(long? document);
        Task<int> RemoveAsync(Driver driver);

    }
}