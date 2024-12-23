using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class AddGroupName_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Value2",
            //    schema: "Miscellaneous",
            //    table: "Setting");

            //migrationBuilder.AddColumn<string>(
            //    name: "GroupName",
            //    schema: "Miscellaneous",
            //    table: "Setting",
            //    maxLength: 50,
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupName",
                schema: "Miscellaneous",
                table: "Setting");

            migrationBuilder.AddColumn<string>(
                name: "Value2",
                schema: "Miscellaneous",
                table: "Setting",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
