using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class change_invoice_system_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounting_Invoice",
                schema: "Accounting",
                table: "Accounting");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Marketer",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Province",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Accounting_FK_Invoice",
                schema: "Accounting",
                table: "Accounting");

            migrationBuilder.DropColumn(
                name: "CommentForMarketer",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Date",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DeliveryCode",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DeliveryCodedUnPerfect",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "MoneySum",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "PaymentSum",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Shipping",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ShippingNumber",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Time",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "type",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "TypeSell",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "TypeShipping",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "VoteBusinessOwner",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "VoteMarketer",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "writer",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "FK_Invoice",
                schema: "Accounting",
                table: "Accounting");

            migrationBuilder.AlterColumn<decimal>(
                name: "Vat",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "ShippingCompany",
                schema: "Accounting",
                table: "Invoice",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentToCountinue",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentBankCode",
                schema: "Accounting",
                table: "Invoice",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NoteForUser",
                schema: "Accounting",
                table: "Invoice",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NoteForBusinessOwner",
                schema: "Accounting",
                table: "Invoice",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FK_Province",
                schema: "Accounting",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FK_Marketer",
                schema: "Accounting",
                table: "Invoice",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                schema: "Accounting",
                table: "Invoice",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HistoryStateAndDescription",
                schema: "Accounting",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                schema: "Accounting",
                table: "Invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                schema: "Accounting",
                table: "Invoice",
                type: "datetime2(7)",
                maxLength: 10,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "RejectedCost",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "SendingDate",
                schema: "Accounting",
                table: "Invoice",
                type: "datetime2(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                schema: "Accounting",
                table: "Invoice",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "TimeBankPayInfo",
                schema: "Accounting",
                table: "Invoice",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSumProductPrice",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TracingShippingNumber",
                schema: "Accounting",
                table: "Invoice",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HistoryStateAndDescription",
                schema: "Accounting",
                table: "Bridge_Invoice_Product",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "InvoiceDetilasState",
                schema: "Accounting",
                table: "Bridge_Invoice_Product",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                schema: "Accounting",
                table: "Accounting",
                type: "datetime2(7)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                schema: "Accounting",
                table: "Accounting",
                type: "datetime2(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "PaymentLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    InvoiceId = table.Column<int>(nullable: true),
                    AccountingId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    PaymentBankCode = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentLogs_Accounting_AccountingId",
                        column: x => x.AccountingId,
                        principalSchema: "Accounting",
                        principalTable: "Accounting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentLogs_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "Accounting",
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentLogs_User_UserId1",
                        column: x => x.UserId1,
                        principalSchema: "Accsess",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLogs_AccountingId",
                table: "PaymentLogs",
                column: "AccountingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLogs_InvoiceId",
                table: "PaymentLogs",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLogs_UserId1",
                table: "PaymentLogs",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Marketer",
                schema: "Accounting",
                table: "Invoice",
                column: "FK_Marketer",
                principalSchema: "Shapping",
                principalTable: "Marketer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Province",
                schema: "Accounting",
                table: "Invoice",
                column: "FK_Province",
                principalSchema: "Miscellaneous",
                principalTable: "Province",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Marketer",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Province",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropTable(
                name: "PaymentLogs");

            migrationBuilder.DropColumn(
                name: "HistoryStateAndDescription",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "RegisterDate",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "RejectedCost",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "SendingDate",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "TimeBankPayInfo",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "TotalSumProductPrice",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "TracingShippingNumber",
                schema: "Accounting",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "HistoryStateAndDescription",
                schema: "Accounting",
                table: "Bridge_Invoice_Product");

            migrationBuilder.DropColumn(
                name: "InvoiceDetilasState",
                schema: "Accounting",
                table: "Bridge_Invoice_Product");

            migrationBuilder.DropColumn(
                name: "RegisterDate",
                schema: "Accounting",
                table: "Accounting");

            migrationBuilder.AlterColumn<decimal>(
                name: "Vat",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)");

            migrationBuilder.AlterColumn<short>(
                name: "ShippingCompany",
                schema: "Accounting",
                table: "Invoice",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentToCountinue",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)");

            migrationBuilder.AlterColumn<short>(
                name: "PaymentBankCode",
                schema: "Accounting",
                table: "Invoice",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "NoteForUser",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NoteForBusinessOwner",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FK_Province",
                schema: "Accounting",
                table: "Invoice",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FK_Marketer",
                schema: "Accounting",
                table: "Invoice",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentForMarketer",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Date",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryCode",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryCodedUnPerfect",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(132)",
                maxLength: 132,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MoneySum",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentSum",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Shipping",
                schema: "Accounting",
                table: "Invoice",
                type: "decimal(18, 0)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingNumber",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "State",
                schema: "Accounting",
                table: "Invoice",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                schema: "Accounting",
                table: "Invoice",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "type",
                schema: "Accounting",
                table: "Invoice",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "TypeSell",
                schema: "Accounting",
                table: "Invoice",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "TypeShipping",
                schema: "Accounting",
                table: "Invoice",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "VoteBusinessOwner",
                schema: "Accounting",
                table: "Invoice",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "VoteMarketer",
                schema: "Accounting",
                table: "Invoice",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "writer",
                schema: "Accounting",
                table: "Invoice",
                type: "smallint",
                nullable: true,
                comment: "کد اولین رکورد مربوط به شکایت را نگه می دارد و این کد فقط توسط ایجاد کننده شکایت قابل پاک شدن است");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                schema: "Accounting",
                table: "Accounting",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<int>(
                name: "FK_Invoice",
                schema: "Accounting",
                table: "Accounting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Accounting_FK_Invoice",
                schema: "Accounting",
                table: "Accounting",
                column: "FK_Invoice");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounting_Invoice",
                schema: "Accounting",
                table: "Accounting",
                column: "FK_Invoice",
                principalSchema: "Accounting",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Marketer",
                schema: "Accounting",
                table: "Invoice",
                column: "FK_Marketer",
                principalSchema: "Shapping",
                principalTable: "Marketer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Province",
                schema: "Accounting",
                table: "Invoice",
                column: "FK_Province",
                principalSchema: "Miscellaneous",
                principalTable: "Province",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
