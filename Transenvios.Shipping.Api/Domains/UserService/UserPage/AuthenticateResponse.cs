using System.Text.Json.Serialization;

namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Token { get; set; }
    }
}
