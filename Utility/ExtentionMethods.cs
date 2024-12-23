using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Security.Cryptography;
using System.Globalization;
using Utility;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace System
{
    public static class ExtentionMethods
    {
        public static DateTime PersianToGorgian(this string date)
        {
           
            var arrDash= date.Split("-");
            var arrslash= date.Split("/");

            if (arrDash.Length == 3) return PersianToGorgian(int.Parse(arrDash[0]), int.Parse(arrDash[1]), int.Parse(arrDash[2]));
            else if (arrslash.Length == 3) return PersianToGorgian(int.Parse(arrslash[0]), int.Parse(arrslash[1]), int.Parse(arrslash[2]));
            else throw new  Exception("امکان تبدیل تاریخ از شمسی به میلای وجود ندارد");
        }
        public static DateTime PersianToGorgian(int year, int month, int day)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(year, month, day, pc);
            return dt;
        }
        public static string ToPersianDate(this DateTime date)
        {
            if (date.Year == 1)
            {
                return "";
            }
            PersianCalendar jc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));
            return pdate;
        }
        public static string ToPersianDateTime(this DateTime datetime)
        {
            if (datetime == DateTime.MinValue)
                return string.Empty;
            PersianCalendar jc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(datetime), jc.GetMonth(datetime), jc.GetDayOfMonth(datetime));
            return string.Concat( pdate ," ", datetime.Hour,":",datetime.Minute,":",datetime.Second)  ;
        }
        public static string ToPersianDateWithMonthName(this DateTime date)
        {
            PersianCalendar jc = new PersianCalendar();
           // string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));

           
            int month = jc.GetMonth(date);
            string monthName = "";
            switch (month)
            {
                case 1: monthName = "فررودين"; break;
                case 2: monthName = "ارديبهشت"; break;
                case 3: monthName = "خرداد"; break;
                case 4: monthName = "تير‏"; break;
                case 5: monthName = "مرداد"; break;
                case 6: monthName = "شهريور"; break;
                case 7: monthName = "مهر"; break;
                case 8: monthName = "آبان"; break;
                case 9: monthName = "آذر"; break;
                case 10: monthName = "دي"; break;
                case 11: monthName = "بهمن"; break;
                case 12: monthName = "اسفند"; break;
                default: monthName = ""; break;
            }
            string pdate = string.Concat(jc.GetDayOfMonth(date), " ", monthName, " ", jc.GetYear(date));
            return pdate;
        }

        public static string ToLatinNumber(this string input)
        {
            if (string.IsNullOrWhiteSpace( input)) return input;
            //٠١٢٣٤٥٦٧٨٩  ARABIC
            //۰۱۲۳۴۵۶۷۸۹ PERSIAN

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹ PERSIAN
            //٠٩٣٠٧٠٩٤٧٠
            //٠٩١٠٧٤٦٠٩٨٣
            //٠٩٣٣٦٩٩٤١٤٨
            //٠٩١٢٤١٦٢٧٤٠
            input = input.Replace("۰", "0").Replace("٠", "0");
            input = input.Replace("۱", "1").Replace("١", "1");
            input = input.Replace("۲", "2").Replace("٢", "2");
            input = input.Replace("۳", "3").Replace("٣", "3");
            input = input.Replace("۴", "4").Replace("٤", "4");
            input = input.Replace("۵", "5").Replace("٥", "5");
            input = input.Replace("۶", "6").Replace("٦", "6");
            input = input.Replace("۷", "7").Replace("٧", "7");
            input = input.Replace("۸", "8").Replace("٨", "8");
            input = input.Replace("۹", "9").Replace("٩", "9");
            return input;
        }

        /// <summary>
        /// Ported from mehrang project
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToPersianString(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return string.Empty;
            return obj.ToString().Replace((char)1603/*ك*/, (char)1705/*ک*/)
                                 .Replace((char)1610/*ي*/, (char)1740/*ی*/)
                             .Replace('0', '٠')
            .Replace('1', '١')
            .Replace('2', '٢')
            .Replace('3', '٣')
            .Replace('4', '٤')
            .Replace('5', '٥')
            .Replace('6', '٦')
            .Replace('7', '٧')
            .Replace('8', '٨')
            .Replace('9', '٩');
        }



        public static bool ToBoolean(this object o, bool defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.BooleanValue;
        }
        public static byte ToByte(this object o, byte defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.ByteValue;
        }
        public static int ToInteger(this object o, int defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.IntegerValue;
        }
        public static int? ToIntegerDefaultNull(this object o)
        {
            ObjectManager objectManager = new ObjectManager(o, 0);
            if (objectManager.IntegerValue == 0)
                return null;
            else
            return objectManager.IntegerValue;
        }
        public static int? ToNullableInteger(this object o)
        {
            if (o == null) return null;
            ObjectManager objectManager = new ObjectManager(o, 0);
            return objectManager.IntegerValue;
        }
        public static long ToLong(this object o, long defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.LongValue;
        }
        public static Single ToSingle(this object o, Single defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.SingleValue;
        }
        public static float ToFloat(this object o, float defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.FloatValue;
        }
        public static double ToDouble(this object o, double defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.DoubleValue;
        }
        public static decimal ToDecimal(this object o, double defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.DecimalValue;
        }
        public static DateTime ToDateTime(this object o, DateTime defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.DateTimeValue;
        }
        public static DateTime PersianToDateTime(this string PersianDate)
        {
            PersianCalendar pc = new PersianCalendar();
           int year= int.Parse(PersianDate.Split('/')[0]);
           int month= int.Parse(PersianDate.Split('/')[1]);
           int day= int.Parse(PersianDate.Split('/')[2]);
            DateTime dt = new DateTime(year, month, day, pc);
            return dt ;
        }

        public static Guid ToGuid(this object o, Guid defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.GuidValue;
        }


        public static string ToString(this object o, string defaultValue)
        {
            ObjectManager objectManager = new ObjectManager(o, defaultValue);
            return objectManager.StringValue;
        }

        public static short likeness(this string Value, string campearValue)
        {
            var str = string.Concat(Value, campearValue);
            short ContainCounter = 0;
          //  short ValueWordCount = 0;
            short campearWordCount = 0;
            //var ValueList= Value.Split(' ');
            //var campearList =campearValue.Split(' ');
            foreach (var ValItem in Value.Split(' '))
            {
                campearWordCount = 0;
                foreach (var CmpItem in campearValue.Split(' '))
                {
                    campearWordCount++;
                    if (ValItem.Contains(CmpItem) | CmpItem.Contains(ValItem)) ContainCounter++;
                    if (campearWordCount >= 6) break;
                }
                
            }
            return (short)(((float)ContainCounter /  campearWordCount) * 100);
            // return true;   
        }
        public static string DeliveryCoded(int length)
        {
            string result;
            Random rand = new Random();

           result= rand.Next(1, 9).ToString();
           for (int i = 0; i < length-1; i++)
           {
               result += rand.Next(0, 9).ToString();
           }
           return result; 

        }
        public static string DeliveryCodedUnPerfect(this string s)
        {
            string result="";
            string AChar="";
            int Sum=0;
            int randValue=0;
          Random rand = new Random();
          randValue = rand.Next(-9, 9);
          Sum += randValue * s.Substring(0, 1).ToByte(0);
          result += "(" + randValue.ToString() + "*?)";
            for (int i = 1; i < s.Length; i++)
            {
                // AChar= s.Substring(i, i);
                 randValue= rand.Next(-9,9);
                 Sum+=randValue * s.Substring(i, 1).ToByte(0);
                 result += "+("+randValue.ToString()+"*?)";
            }
            return result+"="+Sum.ToString();
        }
        /*
        public static string DeliveryCodedUnPerfect(this string s)
        {

            var random = new Random();
            while(s.Count(ch => ch =='*') < 10)
            {
               int rand= random.Next(15);
               s=s.Remove(rand,1);
               s=s.Insert(rand , "*");

            }
        
            return s;
        }*/

        public static int StringDateToInteger(this string d)
        {
            d = d.Replace("/", "");
            return Convert.ToInt32(d);
        }

        public static string decimalToDigMony(this decimal d)
        {
            return (d/10).ToString("#,##") + " تومان";
        }
        //public static string decimalToDigMonyWithOutRial(this decimal? d)
        //{
        //    if (!d.HasValue) d = 0;
        //    string r = decimaToDigPrivate((decimal)d);

        //    return r ;
        //}
        //public static string decimalToDigMonyWithOutRial(this decimal d)
        //{

        //    return decimaToDigPrivate((decimal)d);

           
        //}
        //private static string decimaToDigPrivate(decimal d)
        //{
        //    string r = d.ToString().Split(".")[0];
        //    r = r.Length >3? (r.Substring(0, r.Length - 3) + "000" ): r;
        //    int lenR = r.Length;
        //    for (int i = lenR - 3; i > 0; i = i - 3)
        //    {
        //        r = r.Insert(i, ",");
        //    }
        //    return r;
        //}
        public static string decimalToDigMony(this decimal? d)
        {
            if (!d.HasValue) d = 0;
            return decimalToDigMony((decimal)d);
        }
        //

        public static string ReplaceDasht(this string s)
        {
            return s.Replace( ' ', '-');
        }
        public static string BusinessNameTrue(this string s)
        {

            return s.Trim().ToLower().Replace(" ", "-"); 
        }
        public static string MD5Hash(this string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return (strBuilder.ToString().Length > 20 ? strBuilder.ToString().Substring(0, 20) : strBuilder.ToString());
        }
        public static string GetLenghStr(this string text,int num)
        {
            if (text == null) return "";
            return (text.Length > num ? text.Substring(0, num) : text );
        }



    }



}
