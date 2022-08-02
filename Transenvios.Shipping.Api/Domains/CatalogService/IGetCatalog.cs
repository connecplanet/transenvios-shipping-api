namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public interface IGetCatalog<T> where T : BaseEntity<Guid>
    {
        Task<IList<T>> GetAllAsync();
    }
}