using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Enums;
using DataLayer;
using ServiceLayer;
using Utility;
using DataLayer.Contract;
using Microsoft.AspNetCore.Http;

namespace UILayer.Miscellaneous
{
    public static class AccessFiled
    {
        private static List<AccessExtraContract> _accessList;
        // private static AccessService _accessService;

        private static List<SettingContract> _settingListRoute;

       /// <summary>
        /// در صورتی مسیر درخوتی
        /// این متد در پروژهای که تعداد رولها بالاست استفاده می گردد
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roleValue"></param>
        /// <returns></returns>
        private static bool accessThisRole(string roleName, int roleValue)
        {
            int rouleNumber;
            rouleNumber = RoleNameToRoleValue(roleName);

            return MultiValueAnalizor.ValueIsMultiValue(rouleNumber, roleValue);
        }
        /// <summary>
        /// دسترسی اکشن ها در کنترولرها
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        private static int RoleNameToRoleValue(string roleName)
        {
            int rouleNumber;
            switch (roleName)
            {
                case ("admin"): { rouleNumber = 2; break; }
                case ("BusinessOwner"): { rouleNumber = 4; break; }
                case ("Marketer"): { rouleNumber = 8; break; }
                case ("User"): { rouleNumber = 16; break; }
                case ("Guest"): { rouleNumber = 32; break; }
                default: { rouleNumber = 32; break; }
            }
            return rouleNumber;
        }
        /// <summary>
        /// اگر کاربر به فیلد مورد نظر در حالت نمایش مورد نظر، دسترسی داشت مقدار ترو را برمی گرداند
        /// </summary>
        /// <param name="AccessEnumString">مد نمایش فیلد </param>
        /// <param name="AccessEnums">عدد نشان دهنده نقشهای مجاز  به دسترسی این فیلد </param>
        /// <returns></returns>
        public static bool AccessFiledHas(string entityName, string filedName, string roleName, long displayMode)
        {
            var access = _accessList.FirstOrDefault(a => a.EntityName == entityName && a.FiledName == filedName && a.RoleName == roleName);
            if (access != null) return MultiValueAnalizor.ValueIsMultiValue(displayMode, access.DisplayMode);
            else return false;
        }

        /// <summary>
        /// اگر کاربر به فیلد مورد نظر در حالت نمایش مورد نظر، دسترسی داشت مقدار ترو را برمی گرداند
        /// </summary>
        /// <param name="AccessEnumString">مد نمایش فیلد </param>
        /// <param name="AccessEnums">عدد نشان دهنده نقشهای مجاز  به دسترسی این فیلد </param>
        /// <returns></returns>
        public static bool AccessFiledHas(string entityName, string filedName, string roleName, long displayMode,ref bool ReadOnly)
        {
            var access = _accessList.FirstOrDefault(a => a.EntityName == entityName && a.FiledName == filedName && a.RoleName == roleName);
            if (access != null)
            {
                ReadOnly = MultiValueAnalizor.ValueIsMultiValue((long)DisplayMode.ReadOnly, access.DisplayMode);
                return MultiValueAnalizor.ValueIsMultiValue(displayMode, access.DisplayMode);
            }
            else return false;
        }
        /// <summary>
        /// اگر کاربر به فیلد مورد نظر در حالت نمایش ویرایش ، دسترسی داشت مقدار ترو را برمی گرداند
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="filedName"></param>
        /// <param name="roleName"></param>
        /// <param name="displayMode"></param>
        /// <returns></returns>
        public static bool AccessFiledToChange(string entityName, string filedName, string roleName)
        {

            var access = _accessList.FirstOrDefault(a => a.EntityName == entityName && a.FiledName == filedName && a.RoleName == roleName);
            if (access != null) return !MultiValueAnalizor.ValueIsMultiValue((long)DisplayMode.ReadOnly, access.DisplayMode);//اگر ردانلی باشد قابل تغییر نیست
            else return false;
        }

        /// <summary>
        /// اگر کاربر به فیلد مورد نظر دسترسی داشت مقدار ترو را به همراه مقدار حالت نمایش برمی گرداند
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="filedName"></param>
        /// <param name="roleName"></param>
        /// <param name="displayMode"></param>
        /// <returns></returns>
        public static bool AccessFiledHas(string entityName, string filedName, string roleName, ref long displayMode)
        {
            displayMode = 0;
            var access = _accessList.FirstOrDefault(a => a.EntityName == entityName && a.FiledName == filedName && a.RoleName == roleName);
            if (access != null)
            {
                displayMode = access.DisplayMode;
                return true;
            }
            else return false;
        }





     static   List<string> noChangeableFild = new List<string>();
        public static void ValidateAccessFormCollection(ref FormCollection formCollection, string entityName, string roleName, long displayMode)
        {
            var count = formCollection.Count;
            foreach (var item in formCollection)
            {
                string prorName = item.Key;
                if (!AccessFiledToChange(entityName, prorName, roleName) & prorName != "HiddenId")
                {
                  
                }
            }
            //for (int i = 0; i < count; i++)
            //{
            //    string prorName = formCollection.Keys[i].ToString();
            //    if (!AccessFiledToChange(entityName, prorName, roleName) & prorName != "HiddenId")
            //    {
            //        formCollection[prorName] = ConstSetting.NoChenge;
            //    }
            //}

        }

    }
}
