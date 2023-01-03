namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities
{
    public enum PaymentStates
    {
        UnPaid = 0,
        Paid = 1
    }

    public enum ShipmentStates
    {
        Ordered,
        Collecting,
        InWarehouse,
        OnRoute,
        Delivered,
        Cancelled
    }
}
