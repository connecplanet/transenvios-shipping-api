using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transenvios.Shipping.Api.Migrations.MySqlMigrations
{
    public partial class CP20addpeopletoshipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "char(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2)",
                oldMaxLength: 2,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransporterId",
                table: "ShipmentOrders",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<sbyte>(
                name: "ShipmentState",
                table: "ShipmentOrders",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PickUpCityId",
                table: "ShipmentOrders",
                type: "varchar(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PickUpAddress",
                table: "ShipmentOrders",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<sbyte>(
                name: "PaymentState",
                table: "ShipmentOrders",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "InitialPrice",
                table: "ShipmentOrders",
                type: "decimal(20,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,2)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "DropOffAddress",
                table: "ShipmentOrders",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicantId",
                table: "ShipmentOrders",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDate",
                table: "ShipmentOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "ShipmentOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifyUserId",
                table: "ShipmentOrders",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<sbyte>(
                name: "RecipientCountryCode",
                table: "ShipmentOrders",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientDocumentId",
                table: "ShipmentOrders",
                type: "varchar(20)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RecipientDocumentType",
                table: "ShipmentOrders",
                type: "varchar(5)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RecipientEmail",
                table: "ShipmentOrders",
                type: "varchar(500)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RecipientFirstName",
                table: "ShipmentOrders",
                type: "varchar(200)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RecipientLastName",
                table: "ShipmentOrders",
                type: "varchar(200)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RecipientPhone",
                table: "ShipmentOrders",
                type: "varchar(10)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<sbyte>(
                name: "SenderCountryCode",
                table: "ShipmentOrders",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderDocumentId",
                table: "ShipmentOrders",
                type: "varchar(20)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SenderDocumentType",
                table: "ShipmentOrders",
                type: "varchar(5)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SenderEmail",
                table: "ShipmentOrders",
                type: "varchar(500)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SenderFirstName",
                table: "ShipmentOrders",
                type: "varchar(200)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SenderLastName",
                table: "ShipmentOrders",
                type: "varchar(200)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SenderPhone",
                table: "ShipmentOrders",
                type: "varchar(10)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsUrgent",
                table: "ShipmentOrderItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFragile",
                table: "ShipmentOrderItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<decimal>(
                name: "InsuredAmount",
                table: "ShipmentOrderItems",
                type: "decimal(20,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,2)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ShipmentOrderItems",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "Routes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<Guid>(
                name: "PickUpCityId",
                table: "Drivers",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(36)",
                oldMaxLength: 36)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PickUpAddress",
                table: "Drivers",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentId",
                table: "Drivers",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 10,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<sbyte>(
                name: "CountryCode",
                table: "Drivers",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Clients",
                type: "char(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2)",
                oldMaxLength: 2,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentId",
                table: "Clients",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 10,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<sbyte>(
                name: "CountryCode",
                table: "Clients",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "Cities",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "ApplicationDate",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "ModifyUserId",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "RecipientCountryCode",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "RecipientDocumentId",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "RecipientDocumentType",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "RecipientEmail",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "RecipientFirstName",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "RecipientLastName",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "RecipientPhone",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "SenderCountryCode",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "SenderDocumentId",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "SenderDocumentType",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "SenderEmail",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "SenderFirstName",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "SenderLastName",
                table: "ShipmentOrders");

            migrationBuilder.DropColumn(
                name: "SenderPhone",
                table: "ShipmentOrders");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "varchar(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ShipmentOrders",
                keyColumn: "TransporterId",
                keyValue: null,
                column: "TransporterId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "TransporterId",
                table: "ShipmentOrders",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "ShipmentState",
                table: "ShipmentOrders",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PickUpCityId",
                table: "ShipmentOrders",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(36)",
                oldMaxLength: 36)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PickUpAddress",
                table: "ShipmentOrders",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentState",
                table: "ShipmentOrders",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "InitialPrice",
                table: "ShipmentOrders",
                type: "decimal(20,2)",
                maxLength: 10,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DropOffAddress",
                table: "ShipmentOrders",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsUrgent",
                table: "ShipmentOrderItems",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFragile",
                table: "ShipmentOrderItems",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<decimal>(
                name: "InsuredAmount",
                table: "ShipmentOrderItems",
                type: "decimal(20,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ShipmentOrderItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "Routes",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "PickUpCityId",
                table: "Drivers",
                type: "varchar(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "PickUpAddress",
                table: "Drivers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "DocumentId",
                table: "Drivers",
                type: "bigint",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "CountryCode",
                table: "Drivers",
                type: "bigint",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(sbyte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Clients",
                type: "varchar(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldMaxLength: 2,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "DocumentId",
                table: "Clients",
                type: "bigint",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "CountryCode",
                table: "Clients",
                type: "bigint",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(sbyte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "Cities",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
