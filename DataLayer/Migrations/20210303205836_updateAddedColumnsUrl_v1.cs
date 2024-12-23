using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class updateAddedColumnsUrl_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string str = @"  update [Shapping].Category
  set TitlePage= [Name] ;

  update [Shapping].[Product] 
 set [NameForUrll] = [Name]";

            migrationBuilder.Sql(str);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
