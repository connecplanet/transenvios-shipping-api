using AutoMapper;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentCityProcessor
    {
        private readonly IGetShipmentCity _getShipmentCity;
        private readonly IMapper _mapper;

        public ShipmentCityProcessor(IMapper mapper,IGetShipmentCity getShipmentCity)
        {
            _getShipmentCity= getShipmentCity ??
                throw new ArgumentNullException(nameof(getShipmentCity));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IList<ShipmentCity>> GetShipmentCityAsync()
        {
            var response = await _getShipmentCity.GetCityAllAsync();
            var cities = _mapper.Map<IList<ShipmentCity>>(response);
            return cities;
        }
    }
}
