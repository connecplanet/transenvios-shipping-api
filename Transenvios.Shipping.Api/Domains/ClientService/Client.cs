using System.Text.Json.Serialization;

namespace Transenvios.Shipping.Api.Domains.ClientService
{
    public class Client : BaseEntity<Guid>
    {
        public string? DocumentType { get; set; }
        public long? DocumentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int? CountryCode { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public string? PasswordHash { get; set; }
    }
}

