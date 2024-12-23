using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class myacountInfo_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryCityName",
                schema: "Accounting",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryCompanyName",
                schema: "Accounting",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryLastName",
                schema: "Accounting",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryMobile",
                schema: "Accounting",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryName",
                schema: "Accounting",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryPostCode",
                schema: "Accounting",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryTel",
                schema: "Accounting",
                table: "Invoice",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryCityName",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DeliveryCompanyName",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DeliveryLastName",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DeliveryMobile",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DeliveryName",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DeliveryPostCode",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DeliveryTel",
                schema: "Accounting",
                table: "Invoice");
        }
    }
}
