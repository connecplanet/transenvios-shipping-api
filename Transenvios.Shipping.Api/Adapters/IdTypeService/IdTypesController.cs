using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Domains.CatalogService;

namespace Transenvios.Shipping.Api.Adapters.IdTypeService
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdTypesController : ControllerBase
    {
        private readonly ICatalogQuery<IdType> _query;

        public IdTypesController(ICatalogQuery<IdType> query)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }

        [HttpGet]
        public async Task<ActionResult<IList<Country>>> GetAllAsync()
        {
            var items = await _query.GetAllAsync();
            return Ok(items);
        }
    }
}
