using System.Globalization;
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
                    ErrorMessage = $"Route {fromCityCode} to {toCityCode} does not found"
                };
            }
            
            var calculation = _chargesCalculator.CalculateCharges(route, order);
            return calculation;
        }

        public async Task<CatalogResponse> GetCatalogAsync()
        {
            var countries = (await _countryMediator.GetAllAsync())
                .Where(c => c.Active == true)
                .Select(c => new ItemCatalogResponse
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name
                }).ToList();

            var idTypes = (await _idTypeMediator.GetAllAsync())
                .Where(c => c.Active == true)
                .Select(c => new ItemCatalogResponse
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name
                }).ToList();

            var cities = (await _cityMediator.GetAllAsync())
                .Where(c => c.Active == true)
                .Select(c => new ItemCatalogResponse
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name
                }).ToList();

            var catalog = new CatalogResponse
            {
                Routes = await _routeStorage.GetAllAsync(true),
                Cities = cities,
                Countries = countries,
                IdTypes = idTypes
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

            return await _orderMediator.SubmitAsync(order);
        }

        public async Task<ShipmentOrderListResponse> GetShipmentOrders(string filterMonth)
        {
            DateTime filterDate;
            var validFormats = new[] {
                "yyyyMMdd",
                "MM/dd/yyyy", "MM-dd-yyyy",
                "yyyy/MM/dd", "yyyy-MM-dd",
                "MM/dd/yyyy HH:mm:ss", "MM-dd-yyyy HH:mm:ss",
                "MM/dd/yyyy hh:mm tt", "MM-dd-yyyy hh:mm tt",
                "yyyy-MM-dd HH:mm:ss, fff", "yyyy/MM/dd HH:mm:ss, fff"
            };

            var provider = new CultureInfo("en-US");

            try
            {
                filterDate = DateTime.ParseExact(filterMonth, validFormats, provider);
            }
            catch (FormatException)
            {
                filterDate = DateTime.Today;
            }

            var startDate = new DateTime(filterDate.Year, filterDate.Month, 1);
            var endDate = startDate.AddMonths(1);

            return await _orderMediator.GetAllAsync(startDate, endDate);
        }

        public async Task<ShipmentOrderEditResponse?> GetShipmentAsync(long id)
        {
            return await _orderMediator.GetOneAsync(id);
        }

        public async Task<ShipmentOrderResponse> UpdateOrderAsync(ShipmentUpdateRequest? orderRequest)
        {
            var orderResponse = new ShipmentOrderResponse
            {
                ResultCode = HttpStatusCode.NotFound,
                ErrorMessage = $"Order {orderRequest!.Id} does not found"
            };

            var orderEntry = await _orderMediator.GetAsync(orderRequest!.Id);
            if (orderEntry == null)
            {
                return orderResponse;
            }

            orderEntry.PaymentState = orderRequest.PaymentState;
            orderEntry.ShipmentState = orderRequest.ShipmentState;
            orderEntry.TransporterId = orderRequest.TransporterId;
            orderEntry.ModifyUserId = orderRequest.EmployeeId;
            orderEntry.ApplicationDate = DateTime.Now;

            var updates = await _orderMediator.UpdateAsync(orderEntry);

            if (updates > 0)
            {
                orderResponse.ResultCode = HttpStatusCode.OK;
                orderResponse.ErrorMessage = null;
            }
            else
            {
                orderResponse.ResultCode = HttpStatusCode.NotModified;
                orderResponse.ErrorMessage = $"Order {orderRequest!.Id} does not modified";
            }

            return orderResponse;
        }
    }
}
