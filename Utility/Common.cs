using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
   public  class Common
    {
        public static System.DateTime PersionToGergorian(List<int> perstionDate)
        {
            System.Globalization.PersianCalendar pg = new System.Globalization.PersianCalendar();
            System.DateTime dat = new System.DateTime(1900, 1, 1);
            List<int> Start = GergorianToPersion(dat);
            dat = pg.AddYears(dat, perstionDate[0] - Start[0]);
            dat = pg.AddMonths(dat, perstionDate[1] - Start[1]);
            dat = pg.AddDays(dat, perstionDate[2] - Start[2]);
            try
            {
                dat = pg.AddHours(dat, perstionDate[3] - Start[3]);
                dat = pg.AddMinutes(dat, perstionDate[4] - Start[4]);
                dat = pg.AddSeconds(dat, perstionDate[5] - Start[5]);
            }
            catch
            {
                dat = pg.AddHours(dat, 12 - Start[3]);
                dat = pg.AddMinutes(dat, 0 - Start[4]);
                dat = pg.AddSeconds(dat, 0 - Start[5]);
            }

            return dat;
        }
        public static List<int> GergorianToPersion(System.DateTime date)
        {

            System.Globalization.PersianCalendar pg = new System.Globalization.PersianCalendar();

            List<int> list = new List<int>();
            list.Add(pg.GetYear(date));
            list.Add(pg.GetMonth(date));
            list.Add(pg.GetDayOfMonth(date));
            list.Add(pg.GetHour(date));
            list.Add(pg.GetMinute(date));
            list.Add(pg.GetSecond(date));
            return list;

        }
        public static string GergorianToPersionString(System.DateTime date)
        {
            var r = GergorianToPersion(date);
            return r[0].ToString("0000") + "/" + r[1].ToString("00") + "/" + r[2].ToString("00");
        }

        public static string GergorianToPersionStringRtl(System.DateTime date)
        {
            var r = GergorianToPersion(date);
            return r[2].ToString("00") + "/" + r[1].ToString("00") + "/" + r[0].ToString("0000");
        }
    }
}
