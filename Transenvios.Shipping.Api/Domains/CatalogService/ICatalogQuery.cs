namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public interface ICatalogQuery<T> where T : BaseEntity<Guid>
    {
        Task<IList<T>> GetAllAsync();
    }
}