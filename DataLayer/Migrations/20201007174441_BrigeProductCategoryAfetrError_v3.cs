using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class BrigeProductCategoryAfetrError_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryProperty",
                schema: "Shapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Discription = table.Column<string>(maxLength: 500, nullable: true),
                    FKCategory = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryProperty_Category_FKCategory",
                        column: x => x.FKCategory,
                        principalSchema: "Shapping",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCotegory_FkCategoryProperty",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "FkCategoryProperty");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProperty_FKCategory",
                schema: "Shapping",
                table: "CategoryProperty",
                column: "FKCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_BrigeProductCotegory_CategoryProperty_FkCategoryProperty",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "FkCategoryProperty",
                principalSchema: "Shapping",
                principalTable: "CategoryProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrigeProductCotegory_CategoryProperty_FkCategoryProperty",
                schema: "Shapping",
                table: "BrigeProductCotegory");

            migrationBuilder.DropTable(
                name: "CategoryProperty",
                schema: "Shapping");

            migrationBuilder.DropIndex(
                name: "IX_BrigeProductCotegory_FkCategoryProperty",
                schema: "Shapping",
                table: "BrigeProductCotegory");
        }
    }
}
