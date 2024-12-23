using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Common;
using DataLayer;
using DataLayer.Contract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace AdminPanel.Controllers
{
    public class BaseController : Controller
    {
     
        public BaseController()
        {
            ExtentionMethodsImage.AppSetting = new AppSetting();
         
        } 
        public UserContract CurrentUserContract
        {
            get
            {
                if (HttpContext.Session != null && HttpContext.Session.Get("User") != null)
                    return HttpContext.Session.GetObjectFromJson<UserContract>("User");
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Session.SetObjectAsJson("User", value);
                ViewBag.User = value;
            }
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
           
           // this.Environme
            // کاربری که لاگین نیست
            if (!ExtentionMethodsImage.AppSetting.IsDeveloperMode && CurrentUserContract == null && !(Request.Path.Value.Equals("/Users/LoginView") || Request.Path.Value.Equals("/Users/Login")))
            {
                filterContext.Result = new RedirectResult(Url.Action("LoginView", "Users"));

            }
        }

    }
}