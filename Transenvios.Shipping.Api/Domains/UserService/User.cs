﻿using System.Text.Json.Serialization;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;

namespace Transenvios.Shipping.Api.Domains.UserService
{
    /// <summary>
    /// Administrator Users
    /// </summary>
    public class User : Person
    {
        [JsonIgnore]
        public List<ShipmentOrder>? Shipments { get; set; }
    }
}
