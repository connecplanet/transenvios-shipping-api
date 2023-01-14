using System.ComponentModel;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities
{
    public enum PaymentStates
    {
        [Description("Sin Pagar")]
        UnPaid,
        [Description("Pagado")]
        Paid
    }

    public enum ShipmentStates
    {
        [Description("Ordenado")]
        Ordered,
        [Description("Recogiendo")]
        Collecting,
        [Description("En Bodega")]
        InWarehouse,
        [Description("En Ruta")]
        OnRoute,
        [Description("Entregado")]
        Delivered,
        [Description("Cancelado")]
        Cancelled
    }
    public enum UserRoles
    {
        Customer,
        Employee
    }
}
