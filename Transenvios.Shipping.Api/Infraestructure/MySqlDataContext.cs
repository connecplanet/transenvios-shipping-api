using Microsoft.EntityFrameworkCore;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public class MySqlDataContext : DataContext
    {
        public MySqlDataContext(IConfiguration configuration) : base(configuration) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = Configuration.GetConnectionString("ApiDatabase");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UserConfiguration();
            modelBuilder.CitiesConfiguration();
            modelBuilder.RouteConfiguration();
            modelBuilder.ClientConfiguration();
            modelBuilder.DriverConfiguration();
            modelBuilder.ShipmentOrderConfiguration();
            modelBuilder.ShipmentOrderItemConfiguration();
        }
    }
}
