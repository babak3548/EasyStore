using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class ProductPromotion_image_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromotionProduct_Category_FkCategory",
                table: "PromotionProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PromotionProduct_Product_FkProduct",
                table: "PromotionProduct");

            migrationBuilder.AlterColumn<int>(
                name: "FkProduct",
                table: "PromotionProduct",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FkCategory",
                table: "PromotionProduct",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "PromotionProduct",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionProduct_Category_FkCategory",
                table: "PromotionProduct",
                column: "FkCategory",
                principalSchema: "Shapping",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionProduct_Product_FkProduct",
                table: "PromotionProduct",
                column: "FkProduct",
                principalSchema: "Shapping",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromotionProduct_Category_FkCategory",
                table: "PromotionProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PromotionProduct_Product_FkProduct",
                table: "PromotionProduct");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "PromotionProduct");

            migrationBuilder.AlterColumn<int>(
                name: "FkProduct",
                table: "PromotionProduct",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FkCategory",
                table: "PromotionProduct",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionProduct_Category_FkCategory",
                table: "PromotionProduct",
                column: "FkCategory",
                principalSchema: "Shapping",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionProduct_Product_FkProduct",
                table: "PromotionProduct",
                column: "FkProduct",
                principalSchema: "Shapping",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
