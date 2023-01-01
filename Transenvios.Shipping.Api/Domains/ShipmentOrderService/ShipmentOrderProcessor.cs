using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.RoutesService;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public class ShipmentOrderProcessor
    {
        private readonly ICalculateShipmentCharges _calculateShipmentCharges;
        private readonly IRouteStorage _routeStorage;
        private readonly ICatalogStorage<City> _cityStorage;
        private readonly ICatalogQuery<Country> _countryStorage;
        private readonly ICatalogQuery<IdType> _idTypeStorage;

        public ShipmentOrderProcessor(
            ICalculateShipmentCharges calculateShipmentCharges,
            IRouteStorage routeStorage,
            ICatalogStorage<City> cityStorage,
            ICatalogQuery<Country> countryStorage,
            ICatalogQuery<IdType> idTypeStorage)
        {
            _calculateShipmentCharges = calculateShipmentCharges ?? throw new ArgumentNullException(nameof(calculateShipmentCharges));
            _routeStorage = routeStorage ?? throw new ArgumentNullException(nameof(routeStorage));
            _cityStorage = cityStorage ?? throw new ArgumentNullException(nameof(cityStorage));
            _countryStorage = countryStorage ?? throw new ArgumentNullException(nameof(countryStorage));
            _idTypeStorage = idTypeStorage ?? throw new ArgumentNullException(nameof(idTypeStorage));
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

        public async Task<CatalogResponse> GetShipmentCatalogAsync()
        {
            var catalog = new CatalogResponse
            {
                Routes = await _routeStorage.GetAllAsync(),
                Cities = await _cityStorage.GetAllAsync(),
                Countries = await _countryStorage.GetAllAsync(),
                IdTypes = await _idTypeStorage.GetAllAsync()
            };

            return catalog;
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
