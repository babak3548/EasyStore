using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addPromotion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Shapping",
                table: "Category",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PromotionProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionType = table.Column<int>(nullable: false),
                    FkProduct = table.Column<int>(nullable: false),
                    ExpireDateTime = table.Column<DateTime>(nullable: false),
                    FkCategory = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionProduct_Category_FkCategory",
                        column: x => x.FkCategory,
                        principalSchema: "Shapping",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionProduct_Product_FkProduct",
                        column: x => x.FkProduct,
                        principalSchema: "Shapping",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionProduct_FkCategory",
                table: "PromotionProduct",
                column: "FkCategory");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionProduct_FkProduct",
                table: "PromotionProduct",
                column: "FkProduct");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionProduct");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Shapping",
                table: "Category",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
