namespace Transenvios.Shipping.Api.Domains.UserService
{
    public interface IUpdateUser
    {
        Task<int> UpdateAsync(User user);
    }
}
