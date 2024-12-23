using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class dimoncion_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellOrBuy",
                schema: "Shapping",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "Dimansion",
                schema: "Shapping",
                table: "Product",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimansion",
                schema: "Shapping",
                table: "Product");

            migrationBuilder.AddColumn<bool>(
                name: "SellOrBuy",
                schema: "Shapping",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
