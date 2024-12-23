using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class BrigeProductCategoryProperty_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrigeProductCategoryProperty_Category_CategoryId",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty");

            migrationBuilder.DropIndex(
                name: "IX_BrigeProductCategoryProperty_CategoryId",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrigeProductCategoryProperty_CategoryId",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrigeProductCategoryProperty_Category_CategoryId",
                schema: "Shapping",
                table: "BrigeProductCategoryProperty",
                column: "CategoryId",
                principalSchema: "Shapping",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
