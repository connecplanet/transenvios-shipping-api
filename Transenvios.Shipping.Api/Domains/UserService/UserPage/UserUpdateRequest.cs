namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public class UserUpdateRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }

    }
}
