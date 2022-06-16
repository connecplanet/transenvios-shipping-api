namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public interface IUpdateUser
    {
        Task<int> UpdateAsync(User user);
    }
}
