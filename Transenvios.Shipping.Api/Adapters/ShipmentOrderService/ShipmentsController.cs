using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        private readonly ShipmentOrderProcessor _processor;

        public ShipmentsController(
            ShipmentOrderProcessor orderProcessor)
        {
            _processor = orderProcessor ?? throw new ArgumentNullException(nameof(orderProcessor));
        }

        [HttpPost("Calculate")]
        public async Task<ActionResult<ShipmentOrderSubmitResponse>> CalculateChargesAsync(ShipmentOrderRequest? order)
        {
            var response = await _processor.CalculateAsync(order);

            return !string.IsNullOrEmpty(response.ErrorMessage)
                ? response.ResultCode switch
                {
                    HttpStatusCode.NotFound => NotFound(response.ErrorMessage),
                    _ => new BadRequestObjectResult(response.ErrorMessage)
                }
                : Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ShipmentOrderSubmitResponse>> SubmitOrderAsync(ShipmentOrderRequest? order)
        {
            var response = await _processor.SubmitOrderAsync(order);

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
            var response = await _processor.GetCatalogAsync();
            return Ok(response);
        }

        [HttpGet("{date}/Orders")]
        public async Task<IList<ShipmentOrderListItemResponse>> GetShipmentListAsync(string date)
        {
            var response = await _processor.GetShipmentOrders(date);
            return response.Items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShipmentOrderEditResponse>> GetShipmentAsync(long id)
        {
            var response = await _processor.GetShipmentAsync(id);
            return response!;
        }

        [HttpPut]
        public async Task<ActionResult<ShipmentOrderResponse>> UpdateOrderAsync(ShipmentUpdateRequest? order)
        {
            var response = await _processor.UpdateOrderAsync(order);

            return !string.IsNullOrEmpty(response.ErrorMessage)
                ? response.ResultCode switch
                {
                    HttpStatusCode.NotFound => NotFound(response.ErrorMessage),
                    _ => new BadRequestObjectResult(response.ErrorMessage)
                }
                : Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteAsync(long id)
        {
            var response = await _processor.DeleteOrderAsync(id);

            if (response == 0)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}