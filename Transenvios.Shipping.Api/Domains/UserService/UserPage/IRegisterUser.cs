namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public interface IRegisterUser
    {
        Task<int> RegisterAsync(User data);
    }
}
