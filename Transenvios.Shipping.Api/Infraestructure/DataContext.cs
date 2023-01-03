using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;
using Transenvios.Shipping.Api.Domains.UserService;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public class DataContext : DbContext, IDbContext
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

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public void Migrate(IWebHostEnvironment environment)
        {
            Database.Migrate();
            DbInitializer.Initialize(this, environment);
        }
    }
}
