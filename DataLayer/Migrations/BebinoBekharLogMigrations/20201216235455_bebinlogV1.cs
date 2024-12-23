using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations.EasyStoreLogMigrations
{
    public partial class bebinlogV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    UserInfo = table.Column<string>(maxLength: 50, nullable: true),
                    LogType = table.Column<short>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    InnerMessage = table.Column<string>(nullable: true),
                    StackTrace = table.Column<string>(nullable: true),
                    TraceIdentifier = table.Column<string>(fixedLength: true, maxLength: 20, nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");
        }
    }
}
