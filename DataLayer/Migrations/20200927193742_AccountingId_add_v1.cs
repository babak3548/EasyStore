using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class AccountingId_add_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentLogs_Accounting_AccountingId",
                table: "PaymentLogs");

            migrationBuilder.DropIndex(
                name: "IX_PaymentLogs_AccountingId",
                table: "PaymentLogs");

            migrationBuilder.AlterColumn<int>(
                name: "AccountingId",
                table: "PaymentLogs",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLogs_AccountingId",
                table: "PaymentLogs",
                column: "AccountingId",
                unique: true,
                filter: "[AccountingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentLogs_Accounting_AccountingId",
                table: "PaymentLogs",
                column: "AccountingId",
                principalSchema: "Accounting",
                principalTable: "Accounting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentLogs_Accounting_AccountingId",
                table: "PaymentLogs");

            migrationBuilder.DropIndex(
                name: "IX_PaymentLogs_AccountingId",
                table: "PaymentLogs");

            migrationBuilder.AlterColumn<int>(
                name: "AccountingId",
                table: "PaymentLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLogs_AccountingId",
                table: "PaymentLogs",
                column: "AccountingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentLogs_Accounting_AccountingId",
                table: "PaymentLogs",
                column: "AccountingId",
                principalSchema: "Accounting",
                principalTable: "Accounting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
