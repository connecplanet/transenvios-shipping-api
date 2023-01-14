namespace Transenvios.Shipping.Api.Domains.UserService
{
    public interface IRemoveUser
    {
        Task<int> DeleteAsync(User user);
    }
}
