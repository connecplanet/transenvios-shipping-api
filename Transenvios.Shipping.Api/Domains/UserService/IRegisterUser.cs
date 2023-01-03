namespace Transenvios.Shipping.Api.Domains.UserService
{
    public interface IRegisterUser
    {
        Task<int> SignUpAsync(User data);
    }
}
