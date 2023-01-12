using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Domains.CatalogService;

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
            var response = await _processor.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetAsync(Guid id)
        {
            var response = await _processor.GetAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CatalogStateResponse>> AddAsync(City modelDto)
        {
            var response = await _processor.AddAsync(modelDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CatalogStateResponse>> UpdateAsync(Guid id, City modelDto)
        {
            var response = await _processor.UpdateAsync(id, modelDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CatalogStateResponse>> DeleteAsync(Guid id)
        {
            var response = await _processor.DeleteAsync(id);
            return Ok(response);
        }
    }
}
