using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class BrigeProductCategoryProperty_v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
