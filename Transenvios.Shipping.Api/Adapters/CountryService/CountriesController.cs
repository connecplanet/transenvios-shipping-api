using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Domains.CatalogService;

namespace Transenvios.Shipping.Api.Adapters.CountryService
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICatalogQuery<Country> _query;

        public CountriesController(ICatalogQuery<Country> query)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }

        [HttpGet]
        public async Task<ActionResult<IList<Country>>> GetAllAsync()
        {
            var response = await _query.GetAllAsync();
            return Ok(response);
        }
    }
}
