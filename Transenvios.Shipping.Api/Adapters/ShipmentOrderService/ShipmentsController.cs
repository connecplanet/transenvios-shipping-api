using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Adapters.UserService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService;

namespace Transenvios.Shipping.Api.Adapters.ShipmentOrderService
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        private readonly ShipmentOrderProcessor _orderProcessor;

        public ShipmentsController(
            ShipmentOrderProcessor orderProcessor)
        {
            _orderProcessor = orderProcessor ?? throw new ArgumentNullException(nameof(orderProcessor));
        }

        [HttpPost("Calculate")]
        public async Task<ActionResult<ShipmentOrderResponse>> CalculateShipmentPaymentAsync(ShipmentOrderRequest order)
        {
            var response = await _orderProcessor.CalculateShipmentChargesAsync(order);

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                return new BadRequestObjectResult(response.ErrorMessage);
            }

            return Ok(response);
        }

        [HttpPost()]
        public async Task<ActionResult<ShipmentOrderResponse>> SubmitShipmentOrderAsync(ShipmentOrderRequest? order)
        {
            var response = await _orderProcessor.SaveShipmentChargesAsync(order);

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                return new BadRequestObjectResult(response.ErrorMessage);
            }

            return Ok(response);
        }

        [HttpGet("Catalogs")]
        public async Task<ActionResult<CatalogResponse>> GetCatalogAsync()
        {
            var catalog = await _orderProcessor.GetShipmentCatalogAsync();
            return Ok(catalog);
        }
    }
}