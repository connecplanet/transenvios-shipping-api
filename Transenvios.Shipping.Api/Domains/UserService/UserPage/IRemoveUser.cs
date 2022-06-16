namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public interface IRemoveUser
    {
        Task<int> RemoveAsync(User user);
    }
}
