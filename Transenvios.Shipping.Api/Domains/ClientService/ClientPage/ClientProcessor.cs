using Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;

namespace Transenvios.Shipping.Api.Domains. ClientService.ClientPage
{
    public class ClientProcessor
    {
        private readonly IClients _clients;

        public ClientProcessor(IClients clients)
        {
            _clients = clients ??
                throw new ArgumentNullException(nameof(clients));
        }


        public async Task<IList<Client>> GetAllAsync()
        {
            var response = await _clients.GetAllAsync();
            return response;
        }

    }
}
