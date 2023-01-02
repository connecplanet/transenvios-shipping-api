namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public enum PaymentStates
    {
        UnPaid = 0,
        Paid = 1
    }

    public enum ShipmentStates
    {
        Created,
        Collecting,
        InWarehouse,
        OnRoute,
        Delivered,
        Cancelled
    }
}
