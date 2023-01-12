using System.Text.Json.Serialization;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;

namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public class City : CatalogEntity
    {
        [JsonIgnore]
        public virtual ICollection<Driver>? Drivers { get; set; }
        [JsonIgnore]
        public virtual ICollection<ShipmentOrder>? PickUpOrderCities { get; set; }
        [JsonIgnore]
        public virtual ICollection<ShipmentOrder>? DropOffOrderCities { get; set; }
    }
}