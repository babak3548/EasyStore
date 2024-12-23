﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addfiledToProduct_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BeforDiscountPrice",
                schema: "Shapping",
                table: "Product",
                type: "decimal(18, 0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeforDiscountPrice",
                schema: "Shapping",
                table: "Product");
        }
    }
}
