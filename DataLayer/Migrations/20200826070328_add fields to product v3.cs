using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addfieldstoproductv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
    name: "Image3",
    schema: "Shapping",
    table: "Product",
    maxLength: 100,
    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image4",
                schema: "Shapping",
                table: "Product",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RankSelling",
                schema: "Shapping",
                table: "Product",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
