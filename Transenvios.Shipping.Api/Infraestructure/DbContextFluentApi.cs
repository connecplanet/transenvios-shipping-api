using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public static class DbContextFluentApi
    {
        public static void UserConfiguration(this ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(500);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.CountryCode).HasMaxLength(5);
                entity.Property(e => e.Phone).HasMaxLength(10);
                entity.Property(e => e.Role).HasMaxLength(2);
            });

        }
        public static void CitiesConfiguration(this ModelBuilder builder)
        {
            builder.Entity<Citie>(entity =>
            {
                entity.ToTable("Cities");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(5);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Active).IsRequired();
                entity.HasIndex(e => e.Active).IsUnique();
            });
        }
    }
}
