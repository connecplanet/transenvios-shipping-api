namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrderProcessor
    {
        private readonly ICalculateShipmentPrice _priceByWeight;

        public ShipmentOrderProcessor(ICalculateShipmentPrice priceByWeight)
        {
            _priceByWeight = priceByWeight ?? throw new ArgumentNullException(nameof(priceByWeight));
        }

        public async Task<decimal> CalculatePriceAsync(ShipmentOrderRequest request)
        {
            throw new NotImplementedException();
            // _priceByWeight.CalculateChargesByWeight()
        }
    }
}
