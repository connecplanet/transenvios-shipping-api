namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage
{
    public enum PaymentStates
    {
        UnPaid=0,
        Paid=1
    }

    public enum ShipmentStates
    {
        None,
        Collecting,
        InWarehouse,
        OnRoute,
        Delivered
    }
}
