using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class removeFiledVoteNegative : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteNegative",
                schema: "Miscellaneous",
                table: "Comment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoteNegative",
                schema: "Miscellaneous",
                table: "Comment",
                type: "int",
                nullable: true);
        }
    }
}
