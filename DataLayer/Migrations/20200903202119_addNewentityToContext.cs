using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addNewentityToContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                schema: "Shapping",
                table: "Product",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "Date",
            //    schema: "Accsess",
            //    table: "Content",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Date",
            //    schema: "Accsess",
            //    table: "Content");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                schema: "Shapping",
                table: "Product",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
