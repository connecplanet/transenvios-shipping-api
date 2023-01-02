using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Domains.UserService;

namespace Transenvios.Shipping.Api.Adapters.ClientsPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientProcessor _clientProcessor;
        
        public ClientsController(
           ClientProcessor clientProcessor
            )
        {
            _clientProcessor = clientProcessor ?? throw new ArgumentNullException(nameof(clientProcessor));
        }

        [HttpPost] 
        public async Task<ActionResult<UserStateResponse>> AddAsync(ClientUpdateRequest model)
        {
            var response = await _clientProcessor.AddAsync(model);
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<ActionResult<IList<Client>>> GetAllAsync()
        {
            var client = await _clientProcessor.GetAllAsync();
            return Ok(client);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetAsync(Guid id)
        {
            var client = await _clientProcessor.GetAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CatalogStateResponse>> UpdateAsync(Guid id, ClientUpdateRequest model)
        {
            var response = await _clientProcessor.UpdateAsync(id, model);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CatalogStateResponse>> DeleteAsync(Guid id)
        {
            var response = await _clientProcessor.DeleteAsync(id);
            return Ok(response);
        }
    }
}
