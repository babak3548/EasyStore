using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class provinceFK_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fk_Province",
                schema: "Miscellaneous",
                table: "Province",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Province_Fk_Province",
                schema: "Miscellaneous",
                table: "Province",
                column: "Fk_Province");

            migrationBuilder.AddForeignKey(
                name: "FK_Province_Province_Fk_Province",
                schema: "Miscellaneous",
                table: "Province",
                column: "Fk_Province",
                principalSchema: "Miscellaneous",
                principalTable: "Province",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Province_Province_Fk_Province",
                schema: "Miscellaneous",
                table: "Province");

            migrationBuilder.DropIndex(
                name: "IX_Province_Fk_Province",
                schema: "Miscellaneous",
                table: "Province");

            migrationBuilder.DropColumn(
                name: "Fk_Province",
                schema: "Miscellaneous",
                table: "Province");
        }
    }
}
