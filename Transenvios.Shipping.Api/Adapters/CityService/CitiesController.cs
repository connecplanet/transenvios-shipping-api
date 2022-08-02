using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;

namespace Transenvios.Shipping.Api.Adapters.CityService
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ShipmentCityProcessor _processor;

        public CitiesController(ShipmentCityProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor)); ;
        }


        [HttpGet]
        public async Task<ActionResult<IList<ShipmentCity>>> GetAllAsync()
        {
            var city = await _processor.GetShipmentCityAsync();
            return Ok(city);
        }

    }
}
