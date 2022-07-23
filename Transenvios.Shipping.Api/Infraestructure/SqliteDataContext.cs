using Microsoft.EntityFrameworkCore;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public class SqliteDataContext : DataContext
    {
        public SqliteDataContext(IConfiguration configuration) : base(configuration) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("ApiDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UserConfiguration();
            modelBuilder.CitiesConfiguration();
        }
    }
}
