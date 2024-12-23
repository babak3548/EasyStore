using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public  class AppSetting
    {
       public static IConfiguration Configuration;
        public  string ConnectionString
        {
            get
            {
                return ConfigurationExtensions.GetConnectionString(Configuration, "ConnectionStringShoppingCenter"); 
            }
        }
        public static string EnamadLink
        {
            get
            {
                return ConfigurationExtensions.GetConnectionString(Configuration, "EnamadLink");
            }
        }
        public static string DomainName
        {
            get
            {
                return ConfigurationExtensions.GetConnectionString(Configuration, "DomainName");
            }
        }
        public static string domainNameMini
        {
            get
            {
                return ConfigurationExtensions.GetConnectionString(Configuration, "domainNameMini");
            }
        }
        //
        public  string LogFilePathShopping
        {
            get
            {
                return ConfigurationExtensions.GetConnectionString(Configuration, "LogFilePathShopping");
            }
        }
        //
        public  string ParsgreenSignture
        {
            get
            {
                return ConfigurationExtensions.GetConnectionString(Configuration, "ParsgreenSignture");
            }
        }
        public  string ParsgreenNumberLine
        {
            get
            {
                return ConfigurationExtensions.GetConnectionString(Configuration, "ParsgreenNumberLine");
            }
        }
        public string LogFileAddress
        {
            get
            {
                return ConfigurationExtensions.GetConnectionString(Configuration, "LogFileAddress");
            }
        }
        public   string BaseServerPath
        {
            get
            {
               var str =ConfigurationExtensions.GetConnectionString(Configuration, "BaseServerPath");
                return str;
            }
        }

        public string ImagePathInServer
        {
            get
            {
                var str = ConfigurationExtensions.GetConnectionString(Configuration, "ImagePathInServer");
                return str;
            }
        }

        public string ImagePathOtherFileServer
        {
            get
            {
                var str = ConfigurationExtensions.GetConnectionString(Configuration, "ImagePathOtherFileServer");
                return str;
            }
        }
        public string ImagePathInVirtual
        {
            get
            {
                var str = ConfigurationExtensions.GetConnectionString(Configuration, "ImagePathInVirtual");
                return str;
            }
        }
        public string ImagePathOtherFileVirtual
        {
            get
            {
                var str = ConfigurationExtensions.GetConnectionString(Configuration, "ImagePathOtherFileVirtual");
                return str;
            }
        }

        public string ClientDomainName
        {
            get
            {
                var str = ConfigurationExtensions.GetConnectionString(Configuration, "ClientDomainName");
                return str;
            }
        }

        public string QulityImageConfigStr
        {
            get
            {
                var str = ConfigurationExtensions.GetConnectionString(Configuration, "QulityImageConfigStr");
                return str;
            }
        }
        //
        public bool IsDeveloperMode
        {
            get
            {
                var str = ConfigurationExtensions.GetConnectionString(Configuration, "IsDeveloperMode");
                return bool.Parse( str);
            }
        }

        public string PhoneNumber
        {
            get
            {
                var str = ConfigurationExtensions.GetConnectionString(Configuration, "PhoneNumber");
                return str;
            }
        }
        public string PhoneNumberForShow
        {
            get
            {
                var str = ConfigurationExtensions.GetConnectionString(Configuration, "PhoneNumberForShow");
                return str;
            }

        }
    }
}
