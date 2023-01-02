namespace Transenvios.Shipping.Api.Domains.UserService
{
    public interface IRegisterUser
    {
        Task<int> RegisterAsync(User data);
    }
}
