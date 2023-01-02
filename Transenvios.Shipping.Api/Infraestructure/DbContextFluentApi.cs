using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService;
using Transenvios.Shipping.Api.Domains.UserService;

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
                entity.Property(e => e.CountryCode).HasMaxLength(5);
                entity.Property(e => e.Phone).HasMaxLength(10);
                entity.Property(e => e.DocumentId).HasMaxLength(10);
                entity.Property(e => e.DocumentId).IsUnicode();
                entity.Property(e => e.DocumentType).HasMaxLength(5);
                entity.Property(e => e.PickUpCityId).IsRequired().HasMaxLength(36);
                entity.Property(e => e.PickUpAddress).IsRequired().HasMaxLength(100);
            });

        }

        public static void ShipmentOrderConfiguration(this ModelBuilder builder)
        {
            builder.Entity<ShipmentOrder>(entity =>
            {
                entity.ToTable("ShipmentOrders");

                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.Id).HasColumnType("int").IsRequired();
                
                entity.Property(e => e.PickUpCityId).HasColumnType("varchar(5)").IsRequired().HasMaxLength(36);
                entity.Property(e => e.DropOffCityId).HasColumnType("varchar(5)").IsRequired().HasMaxLength(5);

                entity.Property(e => e.PickUpAddress).HasColumnType("varchar(200)").IsRequired().HasMaxLength(200);
                entity.Property(e => e.DropOffAddress).HasColumnType("varchar(200)").IsRequired().HasMaxLength(200);

                entity.Property(e => e.InitialPrice).HasColumnType("decimal");
                entity.Property(e => e.Taxes).HasColumnType("decimal");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal");

                entity.Property(e => e.PaymentState).HasColumnType("tinyint");
                entity.Property(e => e.ShipmentState).HasColumnType("tinyint");

                entity.Property(e => e.TransporterId).HasColumnType("char(36)");
                entity.Property(e => e.ApplicantId).HasColumnType("char(36)");
                entity.Property(e => e.ModifyUserId).HasColumnType("char(36)");

                entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.SenderDocumentType).HasColumnType("varchar(5)");
                entity.Property(e => e.SenderDocumentId).HasColumnType("int");
                entity.Property(e => e.SenderFirstName).HasColumnType("varchar(200)");
                entity.Property(e => e.SenderLastName).HasColumnType("varchar(200)");
                entity.Property(e => e.SenderEmail).HasColumnType("varchar(500)");
                entity.Property(e => e.SenderCountryCode).HasColumnType("tinyint");
                entity.Property(e => e.SenderPhone).HasColumnType("varchar(10)");

                entity.Property(e => e.RecipientDocumentType).HasColumnType("varchar(5)");
                entity.Property(e => e.RecipientDocumentId).HasColumnType("int");
                entity.Property(e => e.RecipientFirstName).HasColumnType("varchar(200)");
                entity.Property(e => e.RecipientLastName).HasColumnType("varchar(200)");
                entity.Property(e => e.RecipientEmail).HasColumnType("varchar(500)");
                entity.Property(e => e.RecipientCountryCode).HasColumnType("tinyint");
                entity.Property(e => e.RecipientPhone).HasColumnType("varchar(10)");
            });
        }

        public static void ShipmentOrderItemConfiguration(this ModelBuilder builder)
        {
            builder.Entity<ShipmentOrderItem>(entity =>
            {
                entity.ToTable("ShipmentOrderItems");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.Id).HasColumnType("char(36)");
                entity.Property(e => e.IdOrder).HasColumnType("int").IsRequired();

                entity.Property(e => e.Weight).HasColumnType("decimal").IsRequired();
                entity.Property(e => e.Height).HasColumnType("decimal").IsRequired();
                entity.Property(e => e.Length).HasColumnType("decimal").IsRequired();
                entity.Property(e => e.Width).HasColumnType("decimal").IsRequired();
                entity.Property(e => e.InsuredAmount).HasColumnType("decimal");

                entity.Property(e => e.IsFragile).HasColumnType("boolean").IsRequired();
                entity.Property(e => e.IsUrgent).HasColumnType("boolean").IsRequired();
            });
        }
    }
}
