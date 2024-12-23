Migration command:
dotnet ef migrations add AppUser -o Data/Migrations -p SanaAp.Infrastructure -s SanaAP.web --context SanaPortalDbContext

DB Update command
dotnet ef database update -p SanaAp.Infrastructure --context SanaPortalDbContext -s SanaAp.Web

Scaffold 
dotnet ef dbcontext  scaffold "Server=172.21.4.172;Database=SanaPortal;user=dbuser;password=123" Microsoft.EntityFrameworkCore.SqlServer -c  tempcontext -p SanaAp.Infrastructure -s SanaAp.Web -o tempFolder -t User -t Role -f



-----------------------------command for Packge Manger concole-------------
 Scaffold
 Scaffold-DbContext "Server=.\SQL2;Database=ShoppingCenters;user=farid;password=123456" Microsoft.EntityFrameworkCore.SqlServer -OutputDir EF1 -Context "OnlineShopping" -DataAnnotations

 Add-Migration CbiExchangesReplicated -OutputDir Data/Migrations -Project SanaAp.Infrastructure -StartupProject SanaAP.web -context EMTradingVdashContext
--- add and update migrtion inline 
Add-Migration logTable -OutputDir Data/Migrations -Project SanaAp.Infrastructure -StartupProject SanaAP.web -context SanaPortalDbContext
Add-Migration changeTableNameMerchent -OutputDir Data/Migrations -Project SanaAp.Infrastructure -StartupProject SanaAP.web -context SanaPortalDbContext
Update-Database -Migration 20200412090338_changeTableNameMerchent_v1 -context SanaPortalDbContext

--------------------------------command for Packge Manger concole-------------
 Scaffold
 Scaffold-DbContext "Server=172.20.249.11;Database=EMTrading970819;user=EMTAdmin;password=U7YmtQ8GjT_RZ%42DrVS" Microsoft.EntityFrameworkCore.SqlServer -OutputDir tempFolder -Context "EMTradingContext" -DataAnnotations

 Add-Migration CbiExchangesReplicated -OutputDir Data/Migrations -Project SanaAp.Infrastructure -StartupProject SanaAP.web -context EMTradingVdashContext