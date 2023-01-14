using System.Text.Json.Serialization;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;

namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public class City : CatalogEntity
    {
        [JsonIgnore]
        public List<Driver>? Drivers { get; set; }
        [JsonIgnore]
        public List<ShipmentOrder>? PickUpCities { get; set; }
        [JsonIgnore]
        public List<ShipmentOrder>? DropOffCities { get; set; }
    }
}