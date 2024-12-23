using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addColumnToCategoryAndProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameForUrll",
                schema: "Shapping",
                table: "Product",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitlePage",
                schema: "Shapping",
                table: "Category",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameForUrll",
                schema: "Shapping",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TitlePage",
                schema: "Shapping",
                table: "Category");
        }
    }
}
