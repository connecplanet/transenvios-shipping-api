using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService.CityPage;
using Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage;
using Transenvios.Shipping.Api.Domains.ClientService.ClientPage;
using Transenvios.Shipping.Api.Domains.DriverService.DriverPage;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<City>? Cities { get; set; }
        public virtual DbSet<ShipmentRoute>? Routes { get; set; }
        public virtual DbSet<Client>? Clients { get; set; }
        public virtual DbSet<Driver>? Drivers { get; set; }
        public virtual DbSet<ShipmentOrder>? ShipmentOrders { get; set; }
        public virtual DbSet<ShipmentOrderItem>? ShipmentOrderItems { get; set; }

        
        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("ApiDatabase"));
        }
    }
}
