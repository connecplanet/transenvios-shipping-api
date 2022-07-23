using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Citie>? Cities { get; set; }

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
