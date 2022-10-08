using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService.CityPage;
using Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage;
using Transenvios.Shipping.Api.Domains.ClientService.ClientPage;
using Transenvios.Shipping.Api.Domains.DriverService.DriverPage;
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
            builder.Entity<City>(entity =>
            {
                entity.ToTable("Cities");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(5);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Active).IsRequired();
            });
        }

        public static void RouteConfiguration(this ModelBuilder builder)
        {
            builder.Entity<ShipmentRoute>(entity =>
            {
                entity.ToTable("Routes");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.FromCityCode).IsRequired().HasMaxLength(5);
                entity.Property(e => e.ToCityCode).IsRequired().HasMaxLength(5);
                entity.HasIndex(e => new { e.FromCityCode, e.ToCityCode }).IsUnique();
                entity.Property(e => e.InitialKiloPrice);
                entity.Property(e => e.AdditionalKiloPrice);
                entity.Property(e => e.PriceCm3);
                entity.Property(e => e.Active).IsRequired();
            });
        }

        public static void ClientConfiguration(this ModelBuilder builder)
        {
            builder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CountryCode).HasMaxLength(5);
                entity.Property(e => e.Phone).HasMaxLength(10);
                entity.Property(e => e.DocumentId).HasMaxLength(10);
                entity.Property(e => e.DocumentId).IsUnicode();
                entity.Property(e => e.DocumentType).HasMaxLength(5);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.Role).HasMaxLength(2);
            });
        }
        public static void DriverConfiguration(this ModelBuilder builder)
        {
            builder.Entity<Driver>(entity =>
            {
                entity.ToTable("Drivers");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(500);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.CountryCode).HasMaxLength(5);
                entity.Property(e => e.Phone).HasMaxLength(10);
                entity.Property(e => e.DocumentId).HasMaxLength(10);
                entity.Property(e => e.DocumentId).IsUnicode();
                entity.Property(e => e.DocumentType).HasMaxLength(5);
            });

        }

        
            public static void ShipmentOrderConfiguration(this ModelBuilder builder)
        {
            builder.Entity<ShipmentOrder>(entity =>
            {
                entity.ToTable("ShipmentOrders");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.PickUpCityId).IsRequired().HasMaxLength(5);
                entity.Property(e => e.PickUpAddress).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DropOffCityId).IsRequired().HasMaxLength(5);
                entity.Property(e => e.DropOffAddress).HasMaxLength(100);
                entity.Property(e => e.InitialPrice).HasMaxLength(10);
                entity.Property(e => e.Taxes).HasMaxLength(10);
                entity.Property(e => e.TotalPrice).HasMaxLength(10);
                entity.Property(e => e.PaymentState).HasMaxLength(5);
                entity.Property(e => e.ShipmentState).HasMaxLength(5);
                entity.Property(e => e.TransporterId).HasMaxLength(5);
            });
        }

        
        public static void ShipmentOrderItemConfiguration(this ModelBuilder builder)
        {
            builder.Entity<ShipmentOrderItem>(entity =>
            {
                entity.ToTable("ShipmentOrderItems");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.Weight).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Height).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Length).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Width).IsRequired().HasMaxLength(100);
                entity.Property(e => e.InsuredAmount).IsRequired();
                entity.Property(e => e.IsFragile).IsRequired();
            });
        }

    }
}
