using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class nullabel_marketer_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Marketer",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.AlterColumn<int>(
                name: "FK_Marketer",
                schema: "Accounting",
                table: "Invoice",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Marketer",
                schema: "Accounting",
                table: "Invoice",
                column: "FK_Marketer",
                principalSchema: "Shapping",
                principalTable: "Marketer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Marketer",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.AlterColumn<int>(
                name: "FK_Marketer",
                schema: "Accounting",
                table: "Invoice",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Marketer",
                schema: "Accounting",
                table: "Invoice",
                column: "FK_Marketer",
                principalSchema: "Shapping",
                principalTable: "Marketer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
