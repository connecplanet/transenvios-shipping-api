using System.Net;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class ShipmentOrderSubmitResponse: ShipmentOrderResponse
    {
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal Total { get; set; }
        public int? Items { get; set; }
    }
}
