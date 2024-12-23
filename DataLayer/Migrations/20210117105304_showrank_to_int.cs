using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class showrank_to_int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RankShow",
                schema: "Shapping",
                table: "Product",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "RankShow",
                schema: "Shapping",
                table: "Product",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
