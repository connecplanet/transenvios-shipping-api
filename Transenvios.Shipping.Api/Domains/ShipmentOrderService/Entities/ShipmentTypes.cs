namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities
{
    public enum PaymentStates
    {
        UnPaid,
        Paid
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
    public enum UserRoles
    {
        Customer,
        Employee
    }
}
