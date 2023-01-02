using AutoMapper;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.ClientService
{
    public class ClientProcessor
    {
        private readonly IClientStorage _clients;

        private readonly IMapper _mapper;

        public ClientProcessor(IClientStorage clients,
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
        public async Task<Client> GetAsync(Guid id)
        {
            var client = await _clients.GetAsync(id);
            if (client == null)
            {
                throw new KeyNotFoundException("Client not found");
            }
            return client;
        }

        public async Task<CatalogStateResponse> UpdateAsync(Guid id, ClientUpdateRequest model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    throw new AppException("Email is required.");
                }

                var currentClient = await GetAsync(id);
                var emailClient = await _clients.GetAsync(model.Email);

                if (emailClient != null && currentClient.Id != emailClient.Id)
                {
                    throw new AppException($"Email '{model.Email}' is already taken.");
                }

                _mapper.Map(model, currentClient);
                var items = await _clients.UpdateAsync(currentClient);

                return new CatalogStateResponse
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

        public async Task<CatalogStateResponse> DeleteAsync(Guid id)
        {
            var user = await GetAsync(id);
            var items = await _clients.DeleteAsync(user);

            return new CatalogStateResponse
            {
                Id = user.Id,
                Items = items,
                Message = "Client deleted successfully"
            };
        }

        public async Task<CatalogStateResponse> AddAsync(ClientUpdateRequest model)
        {
            try
            {
                var result = model.Email != null && await _clients.Exists(model.Email);

                if (result)
                {
                    throw new AppException($"Email '{model.Email}' is already registered");
                }

                var client = _mapper.Map<Client>(model);
                var items = await _clients.AddAsync(client);

                return new CatalogStateResponse
                {
                    Id = client.Id,
                    Items = items,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new CatalogStateResponse
                {
                    Message = ex.GetBaseException().Message
                };
            }
        }
    }
}
