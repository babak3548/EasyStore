using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using UILayer.Miscellaneous;
using DataLayer.Contract;
using Microsoft.AspNetCore.Mvc;
using DataLayer.EF;

namespace UILayer.Controllers
{
    public class ErrorController : Base0Controller
    {
        public ErrorController( OnlineShopping _onlineShopping)
            : base("Error" ,  _onlineShopping)
        {

        }

        public ActionResult Index(string type, string message)
        {
           // DataLayer.Contract.ErrorContract.Message = "pleses true excute";
           if (string.IsNullOrEmpty(message)) message ="در سیستم خطایی پیش آمده است";
           if (string.IsNullOrEmpty(type)) type =" ";
          
            return View(new ErrorContract { Type = type, Message = message });
        }

        public ActionResult NotFound()
        {
            return View();
        }

        //public ActionResult handledError()
        //{
        //    // DataLayer.Contract.ErrorContract.Message = "pleses true excute";

        //    return View();
        //}


    }
}
