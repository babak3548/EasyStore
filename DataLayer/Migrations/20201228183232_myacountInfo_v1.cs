using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class myacountInfo_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                schema: "Accsess",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Newsletter",
                schema: "Accsess",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SpicialOffer",
                schema: "Accsess",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                schema: "Accsess",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Newsletter",
                schema: "Accsess",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SpicialOffer",
                schema: "Accsess",
                table: "User");
        }
    }
}
