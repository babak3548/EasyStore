using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class type_errorv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrigeProductCotegory_Category_FkCategory",
                schema: "Shapping",
                table: "BrigeProductCotegory");

            migrationBuilder.DropIndex(
                name: "IX_BrigeProductCotegory_FkCategory",
                schema: "Shapping",
                table: "BrigeProductCotegory");

            migrationBuilder.DropColumn(
                name: "FkCategory",
                schema: "Shapping",
                table: "BrigeProductCotegory");

            migrationBuilder.DropColumn(
                name: "PartialType",
                schema: "Accsess",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "Width",
                schema: "Accsess",
                table: "Content");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FkCategoryProperty",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCotegory_CategoryId",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrigeProductCotegory_Category_CategoryId",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "CategoryId",
                principalSchema: "Shapping",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrigeProductCotegory_Category_CategoryId",
                schema: "Shapping",
                table: "BrigeProductCotegory");

            migrationBuilder.DropIndex(
                name: "IX_BrigeProductCotegory_CategoryId",
                schema: "Shapping",
                table: "BrigeProductCotegory");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "Shapping",
                table: "BrigeProductCotegory");

            migrationBuilder.DropColumn(
                name: "FkCategoryProperty",
                schema: "Shapping",
                table: "BrigeProductCotegory");

            migrationBuilder.AddColumn<int>(
                name: "FkCategory",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PartialType",
                schema: "Accsess",
                table: "Content",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Width",
                schema: "Accsess",
                table: "Content",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCotegory_FkCategory",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "FkCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_BrigeProductCotegory_Category_FkCategory",
                schema: "Shapping",
                table: "BrigeProductCotegory",
                column: "FkCategory",
                principalSchema: "Shapping",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
