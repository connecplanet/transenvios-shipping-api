namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrder
    {
        public int Id { get; set; }
        public string PickUpCityId { get; set; }
        public string PickUpAddress { get; set; }
        public string DropOffCityId { get; set; }
        public string DropOffAddress { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentState { get; set; }
        public string ShipmentState { get; set; }
        public string TransporterId { get; set; }
    }
}
