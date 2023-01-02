namespace Transenvios.Shipping.Api.Domains.DriverService
{
    public interface IDriverStorage
    {
        Task<IList<Driver>> GetAllAsync();
        Task<Driver> GetAsync(Guid id);
        Task<int> UpdateAsync(Driver driver);
        Task<int> AddAsync(Driver driver);
        Task<bool> Exists(string email);
        Task<bool> Exists(long? document);
        Task<int> DeleteAsync(Driver driver);
    }
}