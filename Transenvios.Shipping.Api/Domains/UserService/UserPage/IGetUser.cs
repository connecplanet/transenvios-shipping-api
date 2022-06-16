namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public interface IGetUser
    {
        Task<IList<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task<bool> ExistsEmail(string email);
        Task<User> GetByEmailAsync(string email);
    }
}
