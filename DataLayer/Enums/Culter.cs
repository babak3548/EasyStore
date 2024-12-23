using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;


namespace DataLayer.Enums
{
    public class Culture
    {

        //public static string  ErrorMessage { get; set; }
        private static global::System.Resources.ResourceManager resourceMan;
        public static string CurrentCulterName { get; set; }
        //public static string ViewName = ScenarioOrViewName.AdminView;
        public static bool CurrentCulter(string CulterName)
        {
            if (CulterName == CurrentCulterName) return true;
            else return false;
        }

        public static string CurrentDate
        {
            get
            {
                if (CurrentCulterName == "en") return DateTime.Now.ToShortDateString();
                else return Common.GergorianToPersionString(DateTime.Now);
            }
        }


        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>

        /// <summary>
        /// با لود شدن این متد زبان و فرهنگ سیستم تغییر می کند
        /// </summary>
        /// <param name="CulterName">زبان مورد نظر</param>
        public static void LoadCurrentCulter(String CulterName)
        {
            CurrentCulterName = "fa";

        }
    }
}
