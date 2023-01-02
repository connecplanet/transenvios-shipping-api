namespace Transenvios.Shipping.Api.Domains.UserService
{
    public interface IGetAuthorizeUser
    {
        Task<User> GetByIdAsync(Guid id);
    }
}
