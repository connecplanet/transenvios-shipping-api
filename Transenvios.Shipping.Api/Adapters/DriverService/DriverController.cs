using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Adapters.UserService;
using Transenvios.Shipping.Api.Domains.DriverService;

namespace Transenvios.Shipping.Api.Adapters.DriverService
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly DriverProcessor _processor;

        public DriversController(DriverProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [HttpGet]
        public async Task<ActionResult<IList<Driver>>> GetAllAsync()
        {
            var drivers = await _processor.GetAsync();
            return Ok(drivers);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DriverStateResponse>> UpdateAsync(Guid id, Driver model)
        {
            var response = await _processor.UpdateAsync(id, model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<DriverStateResponse>> AddAsync(DriverRequest modelDto)
        {
            var response = await _processor.AddAsync(modelDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DriverStateResponse>> DeleteAsync(Guid id)
        {
            var response = await _processor.DeleteAsync(id);
            return Ok(response);
        }
    }
}