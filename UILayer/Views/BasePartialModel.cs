using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Views
{
    public class BasePartialModel
    {
        /// <summary>
        /// پر کردن این فیلد اجباری است
        /// </summary>
        public string PropertyName ;
        /// <summary>
        /// به طور پیش فرض نیازی به پر کردن این فیلد نیست در صورت پر شدن باید با متد گیت استرینق مقدار ش از فایل ریس اکس گرفته و پر شود
        /// </summary>
        public string langugeValue="";
        public string TitleValue="";
        public string ValidateMethod="";
        /// <summary>
        /// پر کردن این فیلد اجباری است
        /// </summary>
        public string value;

       // public bool IsReadonly;
      //  public string ActionNameAjax;
       // public string ControllerNameAjax;

       

    }
}