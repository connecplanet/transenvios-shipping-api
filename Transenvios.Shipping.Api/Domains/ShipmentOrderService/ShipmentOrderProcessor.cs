using System.Collections.Generic;
using System.Net;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.RoutesService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Responses;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public class ShipmentOrderProcessor
    {
        private readonly IOrderChargesCalculator _chargesCalculator;
        private readonly IRouteStorage _routeStorage;
        private readonly ICatalogStorage<City> _cityMediator;
        private readonly ICatalogQuery<Country> _countryMediator;
        private readonly ICatalogQuery<IdType> _idTypeMediator;
        private readonly IOrderStorage _orderMediator;

        public ShipmentOrderProcessor(
            IOrderChargesCalculator chargesCalculator,
            IRouteStorage routeMediator,
            ICatalogStorage<City> cityMediator,
            ICatalogQuery<Country> countryMediator,
            ICatalogQuery<IdType> idTypeMediator,
            IOrderStorage orderMediator)
        {
            _chargesCalculator = chargesCalculator ?? throw new ArgumentNullException(nameof(chargesCalculator));
            _routeStorage = routeMediator ?? throw new ArgumentNullException(nameof(routeMediator));
            _cityMediator = cityMediator ?? throw new ArgumentNullException(nameof(cityMediator));
            _countryMediator = countryMediator ?? throw new ArgumentNullException(nameof(countryMediator));
            _idTypeMediator = idTypeMediator ?? throw new ArgumentNullException(nameof(idTypeMediator));
            _orderMediator = orderMediator ?? throw new ArgumentNullException(nameof(orderMediator));
        }

        public async Task<ShipmentOrderSubmitResponse> CalculateAsync(ShipmentOrderRequest? order)
        {
            if (order == null)
            {
                return new ShipmentOrderSubmitResponse
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
                return new ShipmentOrderSubmitResponse
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
                Cities = await _cityMediator.GetAllAsync(),
                Countries = await _countryMediator.GetAllAsync(),
                IdTypes = await _idTypeMediator.GetAllAsync()
            };

            return catalog;
        }

        public async Task<ShipmentOrderSubmitResponse> SubmitOrderAsync(ShipmentOrderRequest? order)
        {
            var response = await CalculateAsync(order);

            if (response.ResultCode != HttpStatusCode.OK)
            {
                return response;
            }

            return await _orderMediator.SubmitOrderAsync(order);
        }

        public async Task<ShipmentOrderListResponse> GetShipmentOrders(int page = 0, int limit = 0)
        {
            if (page == 0)
                page = 1;

            if (limit == 0)
                limit = int.MaxValue;

            var skip = (page - 1) * limit;

            return await _orderMediator.GetShipmentListAsync(skip, limit);
        }

        public async Task<ShipmentOrderEditResponse?> GetShipmentAsync(long id)
        {
            return await _orderMediator.GetShipmentAsync(id);
        }
    }
}
