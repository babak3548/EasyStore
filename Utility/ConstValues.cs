
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Utility
{
    public class ConstValues
    {
        public const string CaptchaImgPath = @"E:\workArea\ShoppingCenters\UILayer\UILayer\Images\captchaImg.gif";//"Images\\captchaImg";

   public  static   IConfiguration Configuration;
        public  string HostName
        {
            get
            {
       
                if (string.IsNullOrWhiteSpace( ConfigurationExtensions.GetConnectionString(Configuration, "DefaultConnection") ))
                    throw new Exception("نام هاست خالی می باشد");
                return ConfigurationExtensions.GetConnectionString(Configuration, "DefaultConnection");

            }
        }

        public  string DefaultMail
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationExtensions.GetConnectionString(Configuration,  "DefaultMail")))
                    throw new Exception("نام ایمیل پیش فرض خالی می باشد");
                return ConfigurationExtensions.GetConnectionString(Configuration, "DefaultMail");

            }
        }

        public  string DefaultMailYahoo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationExtensions.GetConnectionString(Configuration, "DefaultMailYahoo")))
                    throw new Exception("نام ایمیل پیش فرض خالی می باشد");
                return ConfigurationExtensions.GetConnectionString(Configuration, "DefaultMailYahoo");

            }
        }
        public  int Port
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationExtensions.GetConnectionString(Configuration, "Port")))
                    throw new Exception("کد پورت خالی می باشد");
                return int.Parse(ConfigurationExtensions.GetConnectionString(Configuration, "Port"));

            }
        }
        public  bool enableSsl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationExtensions.GetConnectionString(Configuration, "enableSsl")))
                    return false;
                //throw new Exception("کد پورت خالی می باشد");
                return bool.Parse(ConfigurationExtensions.GetConnectionString(Configuration, "enableSsl"));

            }
        }
        public  string UserName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationExtensions.GetConnectionString(Configuration, "UserName")))
                    return "";
                else
                    return ConfigurationExtensions.GetConnectionString(Configuration, "UserName");

            }
        }

        public  string Password
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationExtensions.GetConnectionString(Configuration, "Password")))
                    return "";
                else
                    return ConfigurationExtensions.GetConnectionString(Configuration, "Password");

            }
        }
    }
}
