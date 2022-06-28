using Transenvios.Shipping.Api.Domains.UserService.UserPage;

namespace Transenvios.Shipping.Api.Domains.UserService.AuthorizationEntity
{
    public interface IGetAuthorizeUser
    {
        Task<User> GetByIdAsync(Guid id);
    }
}
