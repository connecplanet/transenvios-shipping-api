using System.Text.Json.Serialization;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;

namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public class Person : BaseEntity<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public UserRoles? Role { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentId { get; set; }
        public bool? Active { get; set; }
        [JsonIgnore]
        public string? PasswordHash { get; set; }
    }
}
