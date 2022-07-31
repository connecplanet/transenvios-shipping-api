using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Adapters.UserService.AuthorizationEntity;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;

namespace Transenvios.Shipping.Api.Adapters.ShipmentOrderService.ShipmentOrderPage
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        [HttpPost("CalculateCharges")]
        public async Task<ActionResult<ShipmentOrderResponse>> CalculateShipmentPaymentAsync(ShipmentOrderRequest order)
        {
            throw new NotImplementedException();
        }

        [HttpPost()]
        public async Task<ActionResult<ShipmentOrderResponse>> SubmitShipmentOrderAsync(ShipmentOrderRequest order)
        {
            throw new NotImplementedException();
        }
    }
}