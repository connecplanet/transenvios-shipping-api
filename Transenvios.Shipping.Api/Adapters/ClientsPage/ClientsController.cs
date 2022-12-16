using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Adapters.UserService.UserPage;
using Transenvios.Shipping.Api.Domains.ClientService.ClientPage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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


        [AllowAnonymous, HttpPost] // sign-up
        public async Task<ActionResult<UserStateResponse>> RegisterAsync(ClientUpdateRequest model)
        {
            var response = await _clientProcessor.RegisterAsync(model);
            return Ok(response);
        }


        // GET: api/<ClientsController>
        [HttpGet]
        public async Task<ActionResult<IList<Client>>> GetAllAsync()
        {
            var client = await _clientProcessor.GetAllAsync();
            return Ok(client);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetByIdAsync(Guid id)
        {
            var client = await _clientProcessor.GetClientAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClientStateResponse>> UpdateAsync(Guid id, ClientUpdateRequest model)
        {
            var response = await _clientProcessor.UpdateAsync(id, model);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ClientStateResponse>> DeleteAsync(Guid id)
        {
            var response = await _clientProcessor.DeleteAsync(id);
            return Ok(response);
        }
    }
}
