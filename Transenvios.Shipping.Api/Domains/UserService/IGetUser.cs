namespace Transenvios.Shipping.Api.Domains.UserService
{
    public interface IGetUser
    {
        Task<IList<User>> GetAllAsync();
        Task<bool> Exists(string email);
        Task<User> GetAsync(string email);

    }
}
