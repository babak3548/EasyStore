using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addqulityImageV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string str = @"INSERT        Miscellaneous.Setting(Name, Value, GroupName, FK_CategorySetting)
                          VALUES        ('phoneNum','02177656227',' ',0)

INSERT        Miscellaneous.Setting(Name, Value, GroupName, FK_CategorySetting)
VALUES        ('DisplayPhoneNum','021 77 65 62 27',' ',0)";
            migrationBuilder.Sql(str);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
