using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class bussinessOwner_change_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusinessOwner",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.AlterColumn<string>(
                name: "WordKey",
                schema: "Shapping",
                table: "BusinessOwner",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NationalCode",
                schema: "Shapping",
                table: "BusinessOwner",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "FK_User",
                schema: "Shapping",
                table: "BusinessOwner",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "Shapping",
                table: "BusinessOwner",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOwner",
                schema: "Shapping",
                table: "BusinessOwner",
                column: "FK_User",
                unique: true,
                filter: "[FK_User] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusinessOwner",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.AlterColumn<string>(
                name: "WordKey",
                schema: "Shapping",
                table: "BusinessOwner",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NationalCode",
                schema: "Shapping",
                table: "BusinessOwner",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FK_User",
                schema: "Shapping",
                table: "BusinessOwner",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "Shapping",
                table: "BusinessOwner",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessOwner",
                schema: "Shapping",
                table: "BusinessOwner",
                column: "FK_User",
                unique: true);
        }
    }
}
