using System.Net;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses
{
    public class ShipmentOrderResponse
    {
        public long? Id { get; set; }
        public HttpStatusCode ResultCode { get; set; } = HttpStatusCode.OK;
        public string? ErrorMessage { get; set; }

        public ShipmentOrderResponse Configure(HttpStatusCode resultCode, string? errorMessage)
        {
            ResultCode = resultCode;
            ErrorMessage = errorMessage;
            return this;
        }
    }
}
