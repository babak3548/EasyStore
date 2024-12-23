using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addshippingtimTodate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "MaxShippingDay",
                schema: "Shapping",
                table: "Product",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "MinShippingDay",
                schema: "Shapping",
                table: "Product",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "PromotionProduct",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxShippingDay",
                schema: "Shapping",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MinShippingDay",
                schema: "Shapping",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "PromotionProduct");
        }
    }
}
