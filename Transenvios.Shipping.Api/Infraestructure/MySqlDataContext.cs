using Microsoft.EntityFrameworkCore;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public class MySqlDataContext : DataContext
    {
        public MySqlDataContext(IConfiguration configuration) : base(configuration) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql(Configuration.GetConnectionString("ApiDatabase"), ServerVersion.AutoDetect(Configuration.GetConnectionString("ApiDatabase")));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UserConfiguration();
            modelBuilder.CitiesConfiguration();
            modelBuilder.RouteConfiguration();
            modelBuilder.ClientConfiguration();
            modelBuilder.ShipmentOrderConfiguration();
            modelBuilder.DriverConfiguration();
            modelBuilder.ShipmentOrderItemConfiguration();
        }
    }
}
