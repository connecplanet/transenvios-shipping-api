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

        public async Task<decimal> CalculateShipmentChargesAsync(ShipmentOrderRequest order)
        {
            throw new NotImplementedException();
            // _calculateShipmentCharges.CalculateShipmentCharges()
        }
    }
}
