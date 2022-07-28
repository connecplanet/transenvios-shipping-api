﻿namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrderItem
    {
        public decimal? Weight { get; set; }

        public decimal? Height { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public decimal? InsuredValue { get; set; }

        public bool IsFragile { get; set; }

        public bool IsUrgent { get; set; }
    }
}
