using System.Text.Json.Serialization;

namespace Transenvios.Shipping.Api.Domains.DriverService
{
    public class DriverDto
    {
        public Guid? Id { get; set; } = null;
        [JsonIgnore]
        public string? FirstName { get; set; } = null;
        [JsonIgnore]
        public string? LastName { get; set; } = null;
        public string? FullName => $"{FirstName} {LastName}";
    }
}
