using Transenvios.Shipping.Api.Domains.ClientService;

namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public interface ICatalogStorage<T> where T : BaseEntity<Guid>
    {
        Task<IList<T>> GetAllAsync();

        Task<T> GetAsync(Guid id);

        Task<int> AddAsync(T entity);

        Task<int> UpdateAsync(T entity);

        Task<int> DeleteAsync(T entity);

        bool Exists(Guid id);

        bool Exists(Guid id, string? code);

        Task<T> GetAsync(string code);
    }
}