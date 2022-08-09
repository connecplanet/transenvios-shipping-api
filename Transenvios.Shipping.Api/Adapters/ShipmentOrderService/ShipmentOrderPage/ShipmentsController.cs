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
        private readonly ShipmentOrderProcessor _processor;

        public ShipmentsController(ShipmentOrderProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor)); ;
        }

        [HttpPost("Calculate")]
        public async Task<ActionResult<ShipmentOrderResponse>> CalculateShipmentPaymentAsync(ShipmentOrderRequest order)
        {
            var response = await _processor.CalculateShipmentChargesAsync(order);

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                return new BadRequestObjectResult(response.ErrorMessage);
            }

            return Ok(response);
        }

        [HttpPost()]
        public async Task<ActionResult<ShipmentOrderResponse>> SubmitShipmentOrderAsync(ShipmentOrderRequest order)
        {
            throw new NotImplementedException();
        }

        [HttpGet("Catalogs")]
        public async Task<ActionResult<CatalogResponse>> GetCatalogAsync()
        {
            var catalog = await _processor.GetShipmentCatalogAsync();
            return Ok(catalog);
        }
    }
}