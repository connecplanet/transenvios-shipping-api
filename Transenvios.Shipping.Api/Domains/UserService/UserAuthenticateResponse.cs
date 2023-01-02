using System.Text.Json.Serialization;

namespace Transenvios.Shipping.Api.Domains.UserService
{
    public class UserAuthenticateResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Token { get; set; }
        public string? Name => $"{LastName}, {FirstName}";
        public string? Avatar { get; set; }
        public string? Status { get; set; }
    }
}
