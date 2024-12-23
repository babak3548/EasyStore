using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations.EasyStoreLogMigrations
{
    public partial class logIdentifer_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TraceIdentifier",
                table: "Log",
                type: "nvarchar(50)",
                fixedLength: true,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(20)",
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "Log",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TraceIdentifier",
                table: "Log",
                type: "nchar(20)",
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldFixedLength: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "Log",
                type: "datetime2(7)",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
