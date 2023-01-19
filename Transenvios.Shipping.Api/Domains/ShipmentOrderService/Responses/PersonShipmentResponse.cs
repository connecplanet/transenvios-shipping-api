namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class PersonShipmentResponse: PersonResponse
    {
        public string? CityName { get; set; }
        public string? Address { get; set; }
    }
}
