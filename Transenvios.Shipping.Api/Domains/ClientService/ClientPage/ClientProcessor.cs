using AutoMapper;
using Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Domains.UserService.AuthorizationEntity;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains. ClientService.ClientPage
{
    public class ClientProcessor
    {
        private readonly IClients _clients;

        private readonly IMapper _mapper;

        public ClientProcessor(IClients clients,
            IMapper mapper)
        {
            _clients = clients ??
                throw new ArgumentNullException(nameof(clients));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<IList<Client>> GetAllAsync()
        {
            var response = await _clients.GetAllAsync();
                return response;
        }
        public async Task<Client> GetClientAsync(Guid id)
        {
            var client = await _clients.GetByIdAsync(id);
            if (client == null)
            {
                throw new KeyNotFoundException("Client not found");
            }
            return client;
        }

        public async Task<ClientStateResponse> UpdateAsync(Guid id, ClientUpdateRequest model)
        {
            try
            {

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                throw new AppException("Email is required.");
            }

            var currentClient = await GetClientAsync(id);
            var emailClient = await _clients.GetByEmailAsync(model.Email);

            if (emailClient != null && currentClient.Id != emailClient.Id)
            {
                throw new AppException($"Email '{model.Email}' is already taken.");
            }

            _mapper.Map(model, currentClient);
            var items = await _clients.UpdateAsync(currentClient);

            return new ClientStateResponse
            {
                Id = currentClient.Id,
                Items = items,
                Message = "Client updated successfully"
            };
            }
            catch (Exception ex)
            {
                throw new AppException($"Error '{ex.InnerException}' ");
            }
        }

        public async Task<ClientStateResponse> DeleteAsync(Guid id)
        {
            var user = await GetClientAsync(id);
            var items = await _clients.RemoveAsync(user);

            return new ClientStateResponse
            {
                Id = user.Id,
                Items = items,
                Message = "Client deleted successfully"
            };
        }

        public async Task<ClientStateResponse> RegisterAsync(ClientUpdateRequest model)
        {
            try
            {


                var result = model.Email != null && await _clients.ExistsEmail(model.Email);

                if (result)
                {
                    throw new AppException($"Email '{model.Email}' is already registered");
                }

                var client = _mapper.Map<Client>(model);
                var items = await _clients.RegisterAsync(client);

                return new ClientStateResponse
                {
                    Id = client.Id,
                    Items = items,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new ClientStateResponse
                {
                    Message = ex.GetBaseException().Message
                };
            }
        }
    }
}
