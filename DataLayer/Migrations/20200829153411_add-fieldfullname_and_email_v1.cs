using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addfieldfullname_and_email_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailOrMobile",
                schema: "Miscellaneous",
                table: "Comment",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "Miscellaneous",
                table: "Comment",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailOrMobile",
                schema: "Miscellaneous",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "Miscellaneous",
                table: "Comment");
        }
    }
}
