using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Domains.CatalogService.CityPage;

namespace Transenvios.Shipping.Api.Adapters.CityService
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CityProcessor _processor;

        public CitiesController(CityProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor)); ;
        }

        [HttpGet]
        public async Task<ActionResult<IList<City>>> GetAllAsync()
        {
            var city = await _processor.GetCitiesAsync();
            return Ok(city);
        }
    }
}
