using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class CalculatedStar_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CalculatedStar",
                schema: "Shapping",
                table: "Product",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "UserStar",
                schema: "Shapping",
                table: "Product",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalculatedStar",
                schema: "Shapping",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UserStar",
                schema: "Shapping",
                table: "Product");
        }
    }
}
