using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addfiledToBrigeInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "Accounting",
                table: "Invoice",
                type: "datetime2(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "BeforDiscountPrice",
                schema: "Accounting",
                table: "Bridge_Invoice_Product",
                type: "decimal(18, 0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BuyPrice",
                schema: "Accounting",
                table: "Bridge_Invoice_Product",
                type: "decimal(18, 0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Colore",
                schema: "Accounting",
                table: "Bridge_Invoice_Product",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "Accounting",
                table: "Bridge_Invoice_Product",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "Accounting",
                table: "Bridge_Invoice_Product",
                type: "decimal(18, 0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "BeforDiscountPrice",
                schema: "Accounting",
                table: "Bridge_Invoice_Product");

            migrationBuilder.DropColumn(
                name: "BuyPrice",
                schema: "Accounting",
                table: "Bridge_Invoice_Product");

            migrationBuilder.DropColumn(
                name: "Colore",
                schema: "Accounting",
                table: "Bridge_Invoice_Product");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "Accounting",
                table: "Bridge_Invoice_Product");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "Accounting",
                table: "Bridge_Invoice_Product");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
