using System;
using System.Collections.Generic;
using System.Net.Http;

namespace NotifictionService
{


    public static class SMSkavenegar
    {
        private static string urlkavenegar = "https://api.kavenegar.com/v1/492F6B77754B38704D5A7035785261336769435A2B426D744F31586434474835435643544F4E6E6B7648773D/verify/lookup.json";
        static HttpClient client = new HttpClient();

        static string templeate { get; set; }

        static string ToNum { get; set; }

        //VerifyPhone
        public static int SendOneSMS(string templete, string toNum, string token)
        {
            try
            {
                string strUrl = string.Concat(urlkavenegar, "?receptor=", toNum, "&token=", token, "&template=", templete);

                var xx = client.GetAsync(strUrl);
                HttpResponseMessage response = xx.Result;
                response.EnsureSuccessStatusCode();

                return 1;

            }
            catch (Exception e)
            {
                return -1;
            }

        }

        public static void SendOneSMS(string templete, string toNum, string token, string token2)
        {
            try
            {
                string strUrl = string.Concat(urlkavenegar, "?receptor=", toNum, "&token=", token, "&token2=", token2, "&template=", templete);


                var xx = client.GetAsync(strUrl);
                HttpResponseMessage response = xx.Result;
                response.EnsureSuccessStatusCode();

            }
            catch (Exception e)
            {
                // throw;
            }

        }

        public static void SendOneSMS(string templete, string toNum, string token, string token2, string token3)
        {
            try
            {
                string strUrl = string.Concat(urlkavenegar, "?receptor=", toNum, "&token=", token, "&token2=", token2, "&token3=", token3, "&template=", templete).Replace(" ", "");


                var xx = client.GetAsync(strUrl);
                HttpResponseMessage response = xx.Result;
                response.EnsureSuccessStatusCode();

            }
            catch (Exception e)
            {
                // throw;
            }

        }
        static Dictionary<int, string> statusRes = new Dictionary<int, string>() {
{ 0, "بدون خطا" },
{1  ,"نام کاربری و رمز عبور نامعتبر است"}  ,
{2  ,"شماره فرستنده نا معتبر است"}  ,
{3  ,"شماره گیرنده نامعتبر است" }  ,
{11 ,"خطا در ارتباط با سرور"  }  ,
{21  ,"خطا در ارتباط با سرور"} ,
{22 ,"اعتبار حساب کافی نمی باشد"}  ,
{51 ,"اکانت نمایندگی برای شما وجود ندارد"}  ,
{52 ,"موجودی کافی جهت شارژ مشتری وجود ندارد"}  ,
{53 ,"چنین مشتری در پنل نمایندگی شما وجود ندارد"}  ,
{54 ,"خطا در شارژ مشتری" }};

    }




}