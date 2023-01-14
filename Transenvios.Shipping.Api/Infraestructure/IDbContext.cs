using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;
using Transenvios.Shipping.Api.Domains.UserService;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public interface IDbContext
    {
        DbSet<User>? Users { get; set; }
        DbSet<City>? Cities { get; set; }
        DbSet<ShipmentRoute>? Routes { get; set; }
        DbSet<Client>? Clients { get; set; }
        DbSet<Driver>? Drivers { get; set; }
        DbSet<ShipmentOrder>? ShipmentOrders { get; set; }
        DbSet<ShipmentOrderItem>? ShipmentOrderItems { get; set; }
        Task<int> SaveChangesAsync();
        void Migrate(IWebHostEnvironment environment);
    }
}
