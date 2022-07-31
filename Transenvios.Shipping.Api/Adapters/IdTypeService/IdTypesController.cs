using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Transenvios.Shipping.Api.Adapters.IdTypeService
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdTypesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IList<KeyValuePair<string, string>>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
