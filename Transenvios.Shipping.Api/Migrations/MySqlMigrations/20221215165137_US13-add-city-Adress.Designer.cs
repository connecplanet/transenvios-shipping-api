﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Transenvios.Shipping.Api.Infraestructure;

#nullable disable

namespace Transenvios.Shipping.Api.Migrations.MySqlMigrations
{
    [DbContext(typeof(MySqlDataContext))]
    [Migration("20221215165137_US13-add-city-Adress")]
    partial class US13addcityAdress
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Transenvios.Shipping.Api.Domains.CatalogService.CityPage.City", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .HasColumnType("tinyint");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Cities", (string)null);
                });

            modelBuilder.Entity("Transenvios.Shipping.Api.Domains.CatalogService.ShipmentRoutePage.ShipmentRoute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .HasColumnType("tinyint");

                    b.Property<decimal?>("AdditionalKiloPrice")
                        .HasColumnType("decimal(20,2)");

                    b.Property<string>("FromCityCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<decimal?>("InitialKiloPrice")
                        .HasColumnType("decimal(20,2)");

                    b.Property<decimal?>("PriceCm3")
                        .HasColumnType("decimal(20,2)");

                    b.Property<string>("ToCityCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("FromCityCode", "ToCityCode")
                        .IsUnique();

                    b.ToTable("Routes", (string)null);
                });

            modelBuilder.Entity("Transenvios.Shipping.Api.Domains.ClientService.ClientPage.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<long?>("CountryCode")
                        .HasMaxLength(5)
                        .HasColumnType("bigint");

                    b.Property<long?>("DocumentId")
                        .HasMaxLength(10)
                        .IsUnicode(true)
                        .HasColumnType("bigint");

                    b.Property<string>("DocumentType")
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("varchar(2000)");

                    b.Property<string>("Phone")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Role")
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Clients", (string)null);
                });

            modelBuilder.Entity("Transenvios.Shipping.Api.Domains.DriverService.DriverPage.Driver", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<long?>("CountryCode")
                        .HasMaxLength(5)
                        .HasColumnType("bigint");

                    b.Property<long?>("DocumentId")
                        .HasMaxLength(10)
                        .IsUnicode(true)
                        .HasColumnType("bigint");

                    b.Property<string>("DocumentType")
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Phone")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("PickUpAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PickUpCityId")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Drivers", (string)null);
                });

            modelBuilder.Entity("Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage.ShipmentOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DropOffAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("DropOffCityId")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<decimal>("InitialPrice")
                        .HasMaxLength(10)
                        .HasColumnType("decimal(20,2)");

                    b.Property<string>("PaymentState")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("PickUpAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PickUpCityId")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("ShipmentState")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<decimal>("Taxes")
                        .HasMaxLength(10)
                        .HasColumnType("decimal(20,2)");

                    b.Property<decimal>("TotalPrice")
                        .HasMaxLength(10)
                        .HasColumnType("decimal(20,2)");

                    b.Property<string>("TransporterId")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("ShipmentOrders", (string)null);
                });

            modelBuilder.Entity("Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage.ShipmentOrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal?>("Height")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("decimal(20,2)");

                    b.Property<int>("IdOrder")
                        .HasColumnType("int");

                    b.Property<decimal?>("InsuredAmount")
                        .IsRequired()
                        .HasColumnType("decimal(20,2)");

                    b.Property<bool>("IsFragile")
                        .HasColumnType("tinyint");

                    b.Property<bool>("IsUrgent")
                        .HasColumnType("tinyint");

                    b.Property<decimal?>("Length")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("decimal(20,2)");

                    b.Property<decimal?>("Weight")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("decimal(20,2)");

                    b.Property<decimal?>("Width")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("decimal(20,2)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("ShipmentOrderItems", (string)null);
                });

            modelBuilder.Entity("Transenvios.Shipping.Api.Domains.UserService.UserPage.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CountryCode")
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("varchar(2000)");

                    b.Property<string>("Phone")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Role")
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
