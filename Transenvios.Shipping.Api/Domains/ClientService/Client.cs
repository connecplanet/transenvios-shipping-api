using System.Text.Json.Serialization;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;

namespace Transenvios.Shipping.Api.Domains.ClientService
{
    /// <summary>
    /// Customer Users
    /// </summary>
    public class Client : Person
    {
        [JsonIgnore]
        public List<ShipmentOrder>? Shipments { get; set; }
    }
}