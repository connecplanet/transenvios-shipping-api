using System.Text.Json.Serialization;

namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public class User: BaseEntity<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        [JsonIgnore]
        public string? PasswordHash { get; set; }
    }
}
