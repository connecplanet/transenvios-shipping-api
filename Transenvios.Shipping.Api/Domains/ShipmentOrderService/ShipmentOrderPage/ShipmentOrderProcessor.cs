using Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrderProcessor
    {
        private readonly ICalculateShipmentCharges _calculateShipmentCharges;

        public ShipmentOrderProcessor(ICalculateShipmentCharges calculateShipmentCharges)
        {
            _calculateShipmentCharges = calculateShipmentCharges ??
                throw new ArgumentNullException(nameof(calculateShipmentCharges));
        }

        public async Task<ShipmentOrderResponse> CalculateShipmentChargesAsync(ShipmentOrderRequest order)
        {
            if (order == null)
            {
                return new ShipmentOrderResponse() { ErrorMessage = "Order parameter is null" };
            }

            // TODO Call RouteProcessor
            var route = new ShipmentRoute
            {
                FromCityCode = order?.Route?.PickUp?.CityCode,
                ToCityCode = order?.Route?.DropOff?.CityCode,
                InitialKiloPrice = 15000,
                AdditionalKiloPrice = 1500,
                PriceCm3 = 0.3M
            };

            var calculation = _calculateShipmentCharges.CalculateShipmentCharges(route, order);
            return calculation;
        }

        public Task<CatalogResponse> GetShipmentCatalogAsync()
        {
            // TODO return the cities if exists the shipment route
            var catalog = new CatalogResponse();
            var task = Task.FromResult(catalog);
            return task;
        }

        public async Task<ShipmentOrderResponse> SaveShipmentChargesAsync(ShipmentOrderRequest? order)
        {
            if (order == null)
            {
                return new ShipmentOrderResponse() { ErrorMessage = "Order parameter is null" };
            }

            return await _calculateShipmentCharges.SaveShipmentChargesAsync(order);
        }
    }
}
