namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public interface IPasswordReset
    {
        Task<UserStateResponse> PasswordResetAsync(string email, string newPassword);
    }
}
