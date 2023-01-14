namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests
{
    public class ShipmentOrderRouteRequest
    {
        public ShipmentOrderRouteCityRequest? PickUp { get; set; }

        public ShipmentOrderRouteCityRequest? DropOff { get; set; }
    }
}
