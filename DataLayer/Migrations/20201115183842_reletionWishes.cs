using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class reletionWishes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishes_Product_ProductId",
                table: "Wishes");

            migrationBuilder.DropIndex(
                name: "IX_Wishes_ProductId",
                table: "Wishes");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Wishes");

            migrationBuilder.CreateIndex(
                name: "IX_Wishes_FkProduct",
                table: "Wishes",
                column: "FkProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishes_Product_FkProduct",
                table: "Wishes",
                column: "FkProduct",
                principalSchema: "Shapping",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishes_Product_FkProduct",
                table: "Wishes");

            migrationBuilder.DropIndex(
                name: "IX_Wishes_FkProduct",
                table: "Wishes");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Wishes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishes_ProductId",
                table: "Wishes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishes_Product_ProductId",
                table: "Wishes",
                column: "ProductId",
                principalSchema: "Shapping",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
