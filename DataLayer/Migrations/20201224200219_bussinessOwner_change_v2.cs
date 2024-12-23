using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class bussinessOwner_change_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalcTypeShipping",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.DropColumn(
                name: "typeShippings",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.AddColumn<int>(
                name: "TypeShippingBussinessOwner",
                schema: "Shapping",
                table: "BusinessOwner",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeShippingBussinessOwner",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.AddColumn<int>(
                name: "CalcTypeShipping",
                schema: "Shapping",
                table: "BusinessOwner",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "typeShippings",
                schema: "Shapping",
                table: "BusinessOwner",
                type: "int",
                nullable: true);
        }
    }
}
