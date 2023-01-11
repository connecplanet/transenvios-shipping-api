using System.Text.Json.Serialization;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;

namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public class City : CatalogEntity
    {
        [JsonIgnore]
        public ICollection<Driver>? Drivers { get; set; }
        [JsonIgnore]
        public ICollection<ShipmentOrder>? ShipmentOrderPickUpCity { get; set; }
        [JsonIgnore]
        public ICollection<ShipmentOrder>? ShipmentOrderDropOffCity { get; set; }
    }
}