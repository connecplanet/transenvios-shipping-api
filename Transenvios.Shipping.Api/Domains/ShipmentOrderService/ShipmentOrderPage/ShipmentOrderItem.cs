namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrderItem
    {
        public long Weight { get; set; }

        public long Height { get; set; }

        public long Length { get; set; }

        public long Width { get; set; }

        public decimal InsuredValue { get; set; }

        public bool Fragile { get; set; }

        public bool Urgent { get; set; }
    }
}
