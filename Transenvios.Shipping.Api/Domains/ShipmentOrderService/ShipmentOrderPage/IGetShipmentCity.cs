namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public interface IGetShipmentCity
    {
        Task<IList<ShipmentCity>> GetCityAllAsync();
    }
}
