using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addkeyurl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_NameForUrll",
                schema: "Shapping",
                table: "Product",
                column: "NameForUrll",
                unique: true,
                filter: "[NameForUrll] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NameForUrll",
                schema: "Shapping",
                table: "Product");
        }
    }
}
