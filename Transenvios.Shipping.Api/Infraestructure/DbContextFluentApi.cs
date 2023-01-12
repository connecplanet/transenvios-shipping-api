using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transenvios.Shipping.Api.Domains;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;
using Transenvios.Shipping.Api.Domains.UserService;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public static class DbContextFluentApi
    {
        public static void CitiesConfiguration(this ModelBuilder builder)
        {
            builder.Entity<City>(entity =>
            {
                entity.ToTable("Cities");
                HasGuidKey(entity);

                entity.Property(e => e.Name).HasColumnType("varchar(100)").IsRequired();
                entity.Property(e => e.Code).HasColumnType("varchar(5)").IsRequired().HasMaxLength(5);
                entity.HasIndex(e => e.Code).IsUnique();

                entity.Property(e => e.Active).HasColumnType("bit").IsRequired();
            });
        }

        public static void DriverConfiguration(this ModelBuilder builder)
        {
            builder.Entity<Driver>(entity =>
            {
                entity.ToTable("Drivers");
                HasGuidKey(entity);

                entity.Property(e => e.DocumentType).HasColumnType("varchar(5)").HasMaxLength(5);
                entity.Property(e => e.DocumentId).HasColumnType("varchar(20)").IsUnicode();

                entity.Property(e => e.FirstName).HasColumnType("varchar(200)").IsRequired();
                entity.Property(e => e.LastName).HasColumnType("varchar(200)").IsRequired();
                entity.Property(e => e.Email).HasColumnType("varchar(500)").IsRequired();
                entity.Property(e => e.CountryCode).HasColumnType("varchar(5)");
                entity.Property(e => e.Phone).HasColumnType("varchar(10)");

                entity.Property(e => e.PickUpCityId).HasColumnType("char(36)").IsRequired().HasMaxLength(36);
                entity.Property(e => e.PickUpAddress).HasColumnType("varchar(200)").IsRequired();

                entity.Property(e => e.Active).HasColumnType("bit").IsRequired();

                entity.HasOne(d => d.PickUpCity)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.PickUpCityId)
                    .HasConstraintName("Drivers_PickUpCityId_FK"); ;
            });
        }

        public static void UserConfiguration(this ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                HasGuidKey(entity);

                entity.Property(e => e.FirstName).HasColumnType("varchar(200)").IsRequired();
                entity.Property(e => e.LastName).HasColumnType("varchar(200)").IsRequired();

                entity.Property(e => e.Email).HasColumnType("varchar(500)").IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                
                entity.Property(e => e.PasswordHash).HasColumnType("varchar(2000)").IsRequired();
                entity.Property(e => e.CountryCode).HasColumnType("varchar(5)");

                entity.Property(e => e.Phone).HasColumnType("varchar(10)");
                entity.Property(e => e.Role).HasColumnType("tinyint");

                entity.Property(e => e.DocumentType).HasColumnType("varchar(5)").HasMaxLength(5);
                entity.Property(e => e.DocumentId).HasColumnType("varchar(20)").IsUnicode();

                entity.Property(e => e.Active).HasColumnType("bit").IsRequired();
            });
        }

        public static void RouteConfiguration(this ModelBuilder builder)
        {
            builder.Entity<ShipmentRoute>(entity =>
            {
                entity.ToTable("Routes");
                HasGuidKey(entity);

                entity.Property(e => e.FromCityCode).HasColumnType("varchar(5)").IsRequired();
                entity.Property(e => e.ToCityCode).HasColumnType("varchar(5)").IsRequired();
                entity.HasIndex(e => new { e.FromCityCode, e.ToCityCode }).IsUnique();

                entity.Property(e => e.InitialKiloPrice).HasColumnType("decimal(16,2)");
                entity.Property(e => e.AdditionalKiloPrice).HasColumnType("decimal(16,2)");
                entity.Property(e => e.PriceCm3).HasColumnType("decimal(16,2)");

                entity.Property(e => e.Active).HasColumnType("bit").IsRequired();
            });
        }

        public static void ClientConfiguration(this ModelBuilder builder)
        {
            builder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");
                HasGuidKey(entity);

                entity.Property(e => e.DocumentType).HasColumnType("varchar(5)").HasMaxLength(5);
                entity.Property(e => e.DocumentId).HasColumnType("varchar(20)").IsUnicode();

                entity.Property(e => e.FirstName).HasColumnType("varchar(200)").IsRequired();
                entity.Property(e => e.LastName).HasColumnType("varchar(200)").IsRequired();
                entity.Property(e => e.Email).HasColumnType("varchar(500)").IsRequired();
                entity.Property(e => e.CountryCode).HasColumnType("tinyint");
                entity.Property(e => e.Phone).HasColumnType("varchar(10)");

                entity.Property(e => e.PasswordHash).HasColumnType("varchar(2000)").IsRequired();
                entity.Property(e => e.Role).HasColumnType("char(2)").HasMaxLength(2);
            });
        }

        public static void ShipmentOrderConfiguration(this ModelBuilder builder)
        {
            builder.Entity<ShipmentOrder>(entity =>
            {
                entity.ToTable("ShipmentOrders");

                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.Property(e => e.Id).HasColumnType("bigint").IsRequired();
                
                entity.Property(e => e.PickUpCityId).HasColumnType("char(36)").IsRequired().HasMaxLength(36);
                entity.Property(e => e.DropOffCityId).HasColumnType("char(36)").IsRequired().HasMaxLength(36);

                entity.Property(e => e.PickUpAddress).HasColumnType("varchar(200)").IsRequired().HasMaxLength(200);
                entity.Property(e => e.DropOffAddress).HasColumnType("varchar(200)").IsRequired().HasMaxLength(200);

                entity.Property(e => e.InitialPrice).HasColumnType("decimal(16,2)");
                entity.Property(e => e.Taxes).HasColumnType("decimal(16,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(16,2)");

                entity.Property(e => e.PaymentState).HasColumnType("tinyint");
                entity.Property(e => e.ShipmentState).HasColumnType("tinyint");

                entity.Property(e => e.TransporterId).HasColumnType("char(36)");

                entity.Property(e => e.CustomerId).HasColumnType("char(36)");
                
                entity.Property(e => e.ModifyUserId).HasColumnType("char(36)");

                entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.SenderDocumentType).HasColumnType("varchar(5)");
                entity.Property(e => e.SenderDocumentId).HasColumnType("varchar(20)");
                entity.Property(e => e.SenderFirstName).HasColumnType("varchar(200)");
                entity.Property(e => e.SenderLastName).HasColumnType("varchar(200)");
                entity.Property(e => e.SenderEmail).HasColumnType("varchar(500)");
                entity.Property(e => e.SenderCountryCode).HasColumnType("varchar(5)");
                entity.Property(e => e.SenderPhone).HasColumnType("varchar(10)");

                entity.Property(e => e.RecipientDocumentType).HasColumnType("varchar(5)");
                entity.Property(e => e.RecipientDocumentId).HasColumnType("varchar(20)");
                entity.Property(e => e.RecipientFirstName).HasColumnType("varchar(200)");
                entity.Property(e => e.RecipientLastName).HasColumnType("varchar(200)");
                entity.Property(e => e.RecipientEmail).HasColumnType("varchar(500)");
                entity.Property(e => e.RecipientCountryCode).HasColumnType("varchar(5)");
                entity.Property(e => e.RecipientPhone).HasColumnType("varchar(10)");

                entity
                    .HasOne(d => d.PickUpCity)
                    .WithMany(p => p.PickUpCities)
                    .HasForeignKey(d => d.PickUpCityId)
                    .HasConstraintName("ShipmentOrders_PickupCityId_FK");

                entity
                    .HasOne(d => d.DropOffCity)
                    .WithMany(p => p.DropOffCities)
                    .HasForeignKey(d => d.DropOffCityId)
                    .HasConstraintName("ShipmentOrders_DropOffCityId_FK");

                entity
                    .HasOne(d => d.Customer)
                    .WithMany(p => p.Shipments)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("ShipmentOrders_CustomerId_FK");

                entity
                    .HasOne(d => d.ModifyUser)
                    .WithMany(p => p.AdminOrders)
                    .HasForeignKey(d => d.ModifyUserId)
                    .HasConstraintName("ShipmentOrders_ModifyUserId_FK");

                entity
                    .HasOne(d => d.Transporter)
                    .WithMany(p => p.Shipments)
                    .HasForeignKey(d => d.TransporterId)
                    .HasConstraintName("ShipmentOrders_TransporterId_FK");
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
                entity.Property(e => e.OrderId).HasColumnType("bigint").IsRequired();

                entity.Property(e => e.Weight).HasColumnType("decimal(16,2)").IsRequired();
                entity.Property(e => e.Height).HasColumnType("decimal(16,2)").IsRequired();
                entity.Property(e => e.Length).HasColumnType("decimal(16,2)").IsRequired();
                entity.Property(e => e.Width).HasColumnType("decimal(16,2)").IsRequired();
                entity.Property(e => e.InsuredAmount).HasColumnType("decimal(16,2)");

                entity.Property(e => e.IsFragile).HasColumnType("bit").IsRequired();
                entity.Property(e => e.IsUrgent).HasColumnType("bit").IsRequired();

                entity
                    .HasOne(d => d.Order)
                    .WithMany(p => p.Packages)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("ShipmentOrderItems_ShipmentOrder_FK");
            });
        }

        private static void HasGuidKey<T>(EntityTypeBuilder<T> entity) where T : BaseEntity<Guid>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Id).IsUnique();
            entity.Property(e => e.Id).HasColumnType("char(36)");
        }
    }
}
