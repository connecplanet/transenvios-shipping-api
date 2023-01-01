namespace Transenvios.Shipping.Api.Domains.UserService
{
    public interface IPasswordReset
    {
        Task<UserStateResponse> PasswordResetAsync(string email, string newPassword);
    }
}
