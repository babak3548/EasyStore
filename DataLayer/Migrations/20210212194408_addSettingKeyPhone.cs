using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addSettingKeyPhone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            string str = @"
CREATE UNIQUE NONCLUSTERED INDEX IX_Setting_Name_groupName ON Miscellaneous.Setting
	(
	Name,
	GroupName
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
";
            migrationBuilder.Sql(str);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
