using Transenvios.Shipping.Api.Domains.UserService.UserPage;

namespace Transenvios.Shipping.Api.Domains.ClientService.ClientPage
{
    public interface IClients
    {
        Task<IList<Client>> GetAllAsync();
        Task<bool> ExistsEmail(string email);
        Task<Client> GetByEmailAsync(string email);

        Task<int> UpdateAsync(Client client);

        Task<int> RemoveAsync(Client client);

        Task<int> RegisterAsync(Client client);

        Task<Client> GetByIdAsync(Guid id);

    }
}
