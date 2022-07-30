using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Adapters.UserService.AuthorizationEntity;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;

namespace Transenvios.Shipping.Api.Adapters.ShiopmentOrderService.ShipmentOrderPage
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        [HttpPost("Costs/Calculate")]
        public async Task<ActionResult<ShipmentOrderResponse>> CalculateServiceCostAsync(ShipmentOrderRequest order)
        {
            throw new NotImplementedException();
        }
    }
}