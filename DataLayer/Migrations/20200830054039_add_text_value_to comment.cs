using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class add_text_value_tocomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                schema: "Miscellaneous",
                table: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                schema: "Miscellaneous",
                table: "Comment",
                maxLength: 600,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                schema: "Miscellaneous",
                table: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                schema: "Miscellaneous",
                table: "Comment",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "");
        }
    }
}
