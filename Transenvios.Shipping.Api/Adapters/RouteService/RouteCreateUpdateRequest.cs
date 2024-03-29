﻿namespace Transenvios.Shipping.Api.Adapters.RouteService
{
    public class RouteCreateUpdateRequest
    {
        public string? FromCityCode { get; set; }
        public string? ToCityCode { get; set; }
        public decimal? InitialKiloPrice { get; set; }
        public decimal? AdditionalKiloPrice { get; set; }
        public decimal? PriceCm3 { get; set; }
        public bool? Active { get; set; } = true;
    }
}
