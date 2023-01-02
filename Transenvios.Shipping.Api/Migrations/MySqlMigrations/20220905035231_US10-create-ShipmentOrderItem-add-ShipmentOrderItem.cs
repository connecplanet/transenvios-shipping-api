using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transenvios.Shipping.Api.Migrations.MySqlMigrations
{
    public partial class US10createShipmentOrderItemaddShipmentOrderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShipmentOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Weight = table.Column<decimal>(type: "decimal(20,2)", maxLength: 5, nullable: false),
                    Height = table.Column<decimal>(type: "decimal(20,2)", maxLength: 5, nullable: false),
                    Length = table.Column<decimal>(type: "decimal(20,2)", maxLength: 5, nullable: false),
                    Width = table.Column<decimal>(type: "decimal(20,2)", maxLength: 100, nullable: false),
                    InsuredAmount = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    IsFragile = table.Column<bool>(type: "tinyint", nullable: false),
                    IsUrgent = table.Column<bool>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentOrderItems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentOrderItems_Id",
                table: "ShipmentOrderItems",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipmentOrderItems");
        }
    }
}
