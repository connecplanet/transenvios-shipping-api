using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Transenvios.Shipping.Api.Adapters.CountryService
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IList<KeyValuePair<string, string>>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
