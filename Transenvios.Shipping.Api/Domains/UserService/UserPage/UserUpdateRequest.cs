namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public class UserUpdateRequest
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? countryCode { get; set; }
        public string? phone { get; set; }

    }
}
