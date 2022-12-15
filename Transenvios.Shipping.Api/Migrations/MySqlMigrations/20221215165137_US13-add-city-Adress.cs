using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transenvios.Shipping.Api.Migrations.MySqlMigrations
{
    public partial class US13addcityAdress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        
            migrationBuilder.AddColumn<string>(
                name: "PickUpAddress",
                table: "Drivers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PickUpCityId",
                table: "Drivers",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickUpAddress",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PickUpCityId",
                table: "Drivers");


        }
    }
}
