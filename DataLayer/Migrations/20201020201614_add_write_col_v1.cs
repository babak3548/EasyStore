using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class add_write_col_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                schema: "Accsess",
                table: "Content");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                schema: "Accsess",
                table: "Content",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "Accsess",
                table: "Content",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Writer",
                schema: "Accsess",
                table: "Content",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterDate",
                schema: "Accsess",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "Accsess",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "Writer",
                schema: "Accsess",
                table: "Content");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                schema: "Accsess",
                table: "Content",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
