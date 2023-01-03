using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.RoutesService;

namespace Transenvios.Shipping.Api.Adapters.RouteService
{

    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentRouteController : ControllerBase
    {
        private readonly RoutesProcessor _processor;

        public ShipmentRouteController(RoutesProcessor routesProcessor)
        {
            _processor = routesProcessor ?? throw new ArgumentNullException(nameof(routesProcessor));
        }

        [HttpGet]
        public async Task<ActionResult<IList<ShipmentRoute>>> GetAllAsync()
        {
            var client = await _processor.GetAllAsync();
            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult<RouteStateResponse>> AddAsync(RouteCreateUpdateRequest model)
        {
            var response = await _processor.AddAsync(model);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CatalogStateResponse>> UpdateAsync(Guid id, RouteCreateUpdateRequest model)
        {
            var response = await _processor.UpdateAsync(id, model);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RouteStateResponse>> DeleteAsync(Guid id)
        {
            var response = await _processor.DeleteAsync(id);
            return Ok(response);
        }
    }
}