using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;

namespace Transenvios.Shipping.Api.Adapters.CityService
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IList<ShipmentCity>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
