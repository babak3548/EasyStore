using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class BrigeProductCategoryProperty_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrigeProductCotegory",
                schema: "Shapping");

            migrationBuilder.CreateTable(
                name: "BrigeProductCategoryProperty",
                schema: "Shapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    value = table.Column<string>(maxLength: 500, nullable: true),
                    FkCategoryProperty = table.Column<int>(nullable: false),
                    FkProduct = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrigeProductCategoryProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrigeProductCategoryProperty_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Shapping",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrigeProductCategoryProperty_CategoryProperty_FkCategoryProperty",
                        column: x => x.FkCategoryProperty,
                        principalSchema: "Shapping",
                        principalTable: "CategoryProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrigeProductCategoryProperty_Product_FkProduct",
                        column: x => x.FkProduct,
                        principalSchema: "Shapping",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCategoryProperty_CategoryId",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCategoryProperty_FkCategoryProperty",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty",
                column: "FkCategoryProperty");

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCategoryProperty_FkProduct",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty",
                column: "FkProduct");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrigeProductCategoryProperty",
                schema: "Shapping");

            migrationBuilder.CreateTable(
                name: "BrigeProductCotegory",
                schema: "Shapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    FkCategoryProperty = table.Column<int>(type: "int", nullable: false),
                    FkProduct = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrigeProductCotegory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrigeProductCotegory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Shapping",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrigeProductCotegory_CategoryProperty_FkCategoryProperty",
                        column: x => x.FkCategoryProperty,
                        principalSchema: "Shapping",
                        principalTable: "CategoryProperty",
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
                name: "IX_BrigeProductCotegory_CategoryId",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCotegory_FkCategoryProperty",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "FkCategoryProperty");

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCotegory_FkProduct",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "FkProduct");
        }
    }
}
