using System.Text.Json.Serialization;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;

namespace Transenvios.Shipping.Api.Domains.DriverService
{
    public class Driver : BaseEntity<Guid>
    {
        public string? DocumentType { get; set; }
        public string? DocumentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public string? PickUpAddress { get; set; }
        public Guid PickUpCityId { get; set; }
        public bool? Active { get; set; }
        [JsonIgnore]
        public City? PickUpCity { get; set; }
        [JsonIgnore]
        public List<ShipmentOrder>? Shipments { get; set; }
    }
}
