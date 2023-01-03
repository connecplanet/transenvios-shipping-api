using System.Net;
using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Adapters.UserService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses;

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
        public async Task<ActionResult<ShipmentOrderSubmitResponse>> CalculateChargesAsync(ShipmentOrderRequest? order)
        {
            var response = await _orderProcessor.CalculateAsync(order);

            return !string.IsNullOrEmpty(response.ErrorMessage)
                ? response.ResultCode switch
                {
                    HttpStatusCode.NotFound => NotFound(response.ErrorMessage),
                    _ => new BadRequestObjectResult(response.ErrorMessage)
                }
                : Ok(response);
        }

        [HttpPost()]
        public async Task<ActionResult<ShipmentOrderSubmitResponse>> SubmitOrderAsync(ShipmentOrderRequest? order)
        {
            var response = await _orderProcessor.SubmitOrderAsync(order);

            return !string.IsNullOrEmpty(response.ErrorMessage)
                ? response.ResultCode switch
                {
                    HttpStatusCode.NotFound => NotFound(response.ErrorMessage),
                    _ => new BadRequestObjectResult(response.ErrorMessage)
                }
                : Ok(response);
        }

        [HttpGet("Catalogs")]
        public async Task<ActionResult<CatalogResponse>> GetCatalogAsync()
        {
            var catalog = await _orderProcessor.GetCatalogAsync();
            return Ok(catalog);
        }

        [HttpGet()]
        public async Task<ActionResult<ShipmentOrderListResponse>> GetShipmentListAsync()
        {
            throw new NotImplementedException();
        }
    }
}