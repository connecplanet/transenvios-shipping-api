using System.Net;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.RoutesService;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public class ShipmentOrderProcessor
    {
        private readonly IOrderChargesCalculator _chargesCalculator;
        private readonly IRouteStorage _routeStorage;
        private readonly ICatalogStorage<City> _cityStorage;
        private readonly ICatalogQuery<Country> _countryStorage;
        private readonly ICatalogQuery<IdType> _idTypeStorage;
        private readonly IOrderStorage _orderStorage;

        public ShipmentOrderProcessor(
            IOrderChargesCalculator calculateShipmentCharges,
            IRouteStorage routeStorage,
            ICatalogStorage<City> cityStorage,
            ICatalogQuery<Country> countryStorage,
            ICatalogQuery<IdType> idTypeStorage,
            IOrderStorage orderStorage)
        {
            _chargesCalculator = calculateShipmentCharges ?? throw new ArgumentNullException(nameof(calculateShipmentCharges));
            _routeStorage = routeStorage ?? throw new ArgumentNullException(nameof(routeStorage));
            _cityStorage = cityStorage ?? throw new ArgumentNullException(nameof(cityStorage));
            _countryStorage = countryStorage ?? throw new ArgumentNullException(nameof(countryStorage));
            _idTypeStorage = idTypeStorage ?? throw new ArgumentNullException(nameof(idTypeStorage));
            _orderStorage = orderStorage ?? throw new ArgumentNullException(nameof(orderStorage));
        }

        public async Task<ShipmentOrderResponse> CalculateAsync(ShipmentOrderRequest? order)
        {
            if (order == null)
            {
                return new ShipmentOrderResponse
                {
                    ResultCode = HttpStatusCode.BadRequest,
                    ErrorMessage = "Order parameter is null"
                };
            }

            var fromCityCode = order.Route?.PickUp?.CityCode ?? string.Empty;
            var toCityCode = order.Route?.DropOff?.CityCode ?? string.Empty;
            var route = await _routeStorage.GetAsync(fromCityCode, toCityCode);

            if (route == null)
            {
                return new ShipmentOrderResponse
                {
                    ResultCode = HttpStatusCode.NotFound,
                    ErrorMessage = $"Route {fromCityCode} to {toCityCode} not found"
                };
            }
            
            var calculation = _chargesCalculator.CalculateCharges(route, order);
            return calculation;
        }

        public async Task<CatalogResponse> GetCatalogAsync()
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

        public async Task<ShipmentOrderResponse> SubmitOrderAsync(ShipmentOrderRequest? order)
        {
            var response = await CalculateAsync(order);

            if (response.ResultCode != HttpStatusCode.OK)
            {
                return response;
            }

            return await _orderStorage.SubmitOrderAsync(order);
        }
    }
}
