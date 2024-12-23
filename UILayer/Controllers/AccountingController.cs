using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataLayer;
using ServiceLayer;
using UILayer.Maper;
using DataLayer.Contract;
using DataLayer.Enums;
using Utility;

using UILayer.Miscellaneous;

using UILayer.Models;
using DataLayer.EF;
using Microsoft.AspNetCore.Mvc;

namespace UILayer.Controllers
{
    public  class AccountingController : Base0Controller
    {
        AccountingService _service;
        public AccountingController(OnlineShopping onlineShopping):base("",onlineShopping)
        {
            _service =  new AccountingService (onlineShopping);
        }
        /// <summary>
        /// بهترین بانک طرف قراداد سایت را برای یک حساب برمی گرداند
        /// </summary>
        /// <param name="WithOutShippingCost"></param>
        /// <returns></returns>
        private MyContractBank SutibleMyContractBank(ref bool WithOutShippingCost, string UserAccountBank)
        {
           ////توجه باید اصلاح گردد
            WithOutShippingCost = false;
            return new MyContractBank();
            //throw new NotImplementedException();
        }

        public ActionResult AccountReportView()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 

            return View(new ReportAccountModel {EndDate=UIUtility.CurrentDate });

        }

        public ActionResult AccountReport(string StartDate,string EndDate)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            
            return View(_service.Find(a =>a.FkUser==SessionUserContract.Id ));

        }

    }
}
