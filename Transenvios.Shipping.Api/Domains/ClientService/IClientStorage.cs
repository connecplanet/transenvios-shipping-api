namespace Transenvios.Shipping.Api.Domains.ClientService
{
    public interface IClientStorage
    {
        Task<IList<Client>> GetAllAsync();
        Task<bool> Exists(string email);
        Task<Client> GetAsync(string email);
        Task<int> UpdateAsync(Client client);
        Task<int> DeleteAsync(Client client);
        Task<int> AddAsync(Client client);
        Task<Client> GetAsync(Guid id);
    }
}
