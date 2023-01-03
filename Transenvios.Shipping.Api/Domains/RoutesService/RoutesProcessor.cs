using AutoMapper;
using Transenvios.Shipping.Api.Adapters.RouteService;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.RoutesService
{

    public class RoutesProcessor
    {
        private readonly IMapper _mapper;
        private readonly IRouteStorage _routes;

        public RoutesProcessor(IRouteStorage routes, IMapper mapper)
        {
            _routes = routes ?? throw new ArgumentNullException(nameof(routes));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IList<ShipmentRoute>> GetAllAsync()
        {
            var response = await _routes.GetAllAsync();
            return response;
        }
        public async Task<ShipmentRoute> GetAsync(Guid id)
        {
            var route = await _routes.GetAsync(id);
            if (route == null)
            {
                throw new KeyNotFoundException("Route not found");
            }
            return route;
        }
        public async Task<RouteStateResponse> AddAsync(RouteCreateUpdateRequest model)
        {
            try
            {
                var result = model.FromCityCode != null && model.ToCityCode != null && await _routes.Exists(model.FromCityCode, model.ToCityCode);
                if (result)
                {
                    throw new AppException($"Rout '{model.FromCityCode}' and '{model.ToCityCode}' is already registered");
                }
                var route = _mapper.Map<ShipmentRoute>(model);
                var items = await _routes.AddAsync(route);

                return new RouteStateResponse
                {
                    Id = route.Id,
                    Items = items,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new RouteStateResponse
                {
                    Message = ex.GetBaseException().Message
                };
            }
        }

        public async Task<RouteStateResponse> UpdateAsync(Guid id, RouteCreateUpdateRequest model)
        {
            try
            {
                var currentClient = await GetAsync(id);
                _mapper.Map(model, currentClient);
                var items = await _routes.UpdateAsync(currentClient);

                return new RouteStateResponse
                {
                    Id = currentClient.Id,
                    Items = items,
                    Message = "Routes updated successfully"
                };
            }
            catch (Exception ex)
            {
                throw new AppException($"Error '{ex.InnerException}' ");
            }
        }

        public async Task<RouteStateResponse> DeleteAsync(Guid id)
        {
            var user = await GetAsync(id);
            var items = await _routes.DeleteAsync(user);

            return new RouteStateResponse
            {
                Id = user.Id,
                Items = items,
                Message = "Routes deleted successfully"
            };
        }

    }
}
