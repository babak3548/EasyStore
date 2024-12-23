using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addfiledToProduct_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Product_Category",
            //    schema: "Shapping",
            //    table: "Product");

            migrationBuilder.AlterColumn<int>(
                name: "FK_Category",
                schema: "Shapping",
                table: "Product",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                schema: "Shapping",
                table: "Category",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Product_Category",
            //    schema: "Shapping",
            //    table: "Product",
            //    column: "FK_Category",
            //    principalSchema: "Shapping",
            //    principalTable: "Category",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category",
                schema: "Shapping",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Active",
                schema: "Shapping",
                table: "Category");

            migrationBuilder.AlterColumn<int>(
                name: "FK_Category",
                schema: "Shapping",
                table: "Product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category",
                schema: "Shapping",
                table: "Product",
                column: "FK_Category",
                principalSchema: "Shapping",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
