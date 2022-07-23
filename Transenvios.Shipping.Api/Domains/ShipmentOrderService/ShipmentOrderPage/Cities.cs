namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class Citie : BaseEntity<Guid>
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public bool? Active { get; set; }
    }
}
