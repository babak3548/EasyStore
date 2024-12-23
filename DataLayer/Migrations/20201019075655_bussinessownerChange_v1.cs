using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class bussinessownerChange_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gmail",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.DropColumn(
                name: "Skype",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.DropColumn(
                name: "Yahoo",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Shapping",
                table: "BusinessOwner",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstaOrOtherSocial",
                schema: "Shapping",
                table: "BusinessOwner",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsApp",
                schema: "Shapping",
                table: "BusinessOwner",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.DropColumn(
                name: "InstaOrOtherSocial",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.DropColumn(
                name: "WhatsApp",
                schema: "Shapping",
                table: "BusinessOwner");

            migrationBuilder.AddColumn<string>(
                name: "Gmail",
                schema: "Shapping",
                table: "BusinessOwner",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skype",
                schema: "Shapping",
                table: "BusinessOwner",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Yahoo",
                schema: "Shapping",
                table: "BusinessOwner",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
