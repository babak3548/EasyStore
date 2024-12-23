using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class changeContent_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Position",
                schema: "Accsess",
                table: "Content",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContentType",
                schema: "Accsess",
                table: "Content",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BanerImageAdress",
                schema: "Accsess",
                table: "Content",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoAdress",
                schema: "Accsess",
                table: "Content",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BanerImageAdress",
                schema: "Accsess",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "VideoAdress",
                schema: "Accsess",
                table: "Content");

            migrationBuilder.AlterColumn<short>(
                name: "Position",
                schema: "Accsess",
                table: "Content",
                type: "smallint",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<short>(
                name: "ContentType",
                schema: "Accsess",
                table: "Content",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
