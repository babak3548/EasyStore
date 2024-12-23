using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class BrigeProductCategory_V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvailableColors",
                schema: "Shapping",
                table: "Product",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BrigeProductCotegory",
                schema: "Shapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    value = table.Column<string>(maxLength: 500, nullable: true),
                    FkCategory = table.Column<int>(nullable: false),
                    FkProduct = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrigeProductCotegory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrigeProductCotegory_Category_FkCategory",
                        column: x => x.FkCategory,
                        principalSchema: "Shapping",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrigeProductCotegory_Product_FkProduct",
                        column: x => x.FkProduct,
                        principalSchema: "Shapping",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCotegory_FkCategory",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "FkCategory");

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCotegory_FkProduct",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "FkProduct");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrigeProductCotegory",
                schema: "Shapping");

            migrationBuilder.DropColumn(
                name: "AvailableColors",
                schema: "Shapping",
                table: "Product");
        }
    }
}
