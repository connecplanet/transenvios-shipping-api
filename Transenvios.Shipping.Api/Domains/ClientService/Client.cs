using System.Text.Json.Serialization;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;

namespace Transenvios.Shipping.Api.Domains.ClientService
{
    /// <summary>
    /// Customer Users
    /// </summary>
    public class Client : BaseEntity<Guid>
    {
        public string? DocumentType { get; set; }
        public string? DocumentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public UserRoles? Role { get; set; }
        public bool? Active { get; set; }
        [JsonIgnore]
        public string? PasswordHash { get; set; }
    }
}

