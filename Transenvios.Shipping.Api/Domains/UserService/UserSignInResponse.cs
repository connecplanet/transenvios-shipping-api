namespace Transenvios.Shipping.Api.Domains.UserService
{
    public class UserSignInResponse
    {
        public UserAuthenticateResponse? User { get; set; }

        public string? AccessToken { get; set; }
    }
}
