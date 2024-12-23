using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataLayer.Contract;
using DataLayer.Enums;
using UILayer.Maper;

using UILayer.Miscellaneous;
using DataLayer;


//using System.Web.Mvc;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.EF;
using ServiceLayer;
using Utility;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using DataLayer.EFLog;
using UILayer.Models;

namespace UILayer.Controllers
{
    public class Base0Controller : Controller
    {
        protected ContentMaper _contentMaper;
        protected bool ReqestIsMobil = false;
        public OnlineShopping objectContext;
        public EasyStoreLog EasyStoreLog;
        public CategoryService _CategoryService;
        public PromotionProductService _promotionProductService;
        protected string entityName;
        protected AppSetting AppSetting;
        static Dictionary<string, string> parentCategories;
        static string blackKey = "Promotion";
        ServiceLayer.InvoiceService _invoiceService;
        ServiceLayer.WishService _wishService;

        public Base0Controller(string EntityName, OnlineShopping _onlineShopping )
        {

            entityName = EntityName;
            AppSetting = new AppSetting();
            ExtentionMethodsImage.AppSetting = AppSetting;
            _invoiceService = new InvoiceService(_onlineShopping);
            _wishService = new WishService(_onlineShopping);
            _promotionProductService = new PromotionProductService(_onlineShopping);
            if (CategoryService.categoryList == null || CategoryService.categoryList.Count == 0)
            {
                _CategoryService = new CategoryService(_onlineShopping);
                _CategoryService.FillCategory();
            }
    
            objectContext = _onlineShopping;
           
                 
            if (UIUtility.MultiLanguages == null || UIUtility.MultiLanguages.Count == 0)
            {
                //UIUtility.MultiLanguages = new List<MultiLanguage>();
                UIUtility.MultiLanguages=
                    _onlineShopping.MultiLanguage.Select(m=>new MultiLanguageModel
                    {Id= m.Id,
                        KeyLanguage = m.KeyLanguage, PersianValue=m.PersianValue}).ToList();
            }
        
        }
        protected string GetViewName(string viewName)
        {
            if (!ReqestIsMobil) return "~/Views/" + viewName + ".cshtml"; ;
            switch (viewName)
            {
                case "ss": return "dd";
                default: return "~/Views/ViewsM/" + viewName + ".cshtml";

            }
        }
        //protected ContentService _contentService = new ContentService();
        //
        // GET: /Adminstration/Base0/

        //public ActionResult Index()
        //{
        //    return View("Error");
        //}

        /*   protected override void OnException(ExceptionContext filterContext)
           {
               base.OnException(filterContext);
             filterContext.ExceptionHandled = true;
           }*/

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
           // ViewBag.OnlineShopping = objectContext;
            //if (HttpContext.Session.Get("User") == null)
            //{ (ConstSetting.Password, ConstSetting.EmailGuest, objectContext); }
            //   SetContentPage(CurrentUserContract.Role);

            UserService userService = new UserService(objectContext);
            // ViewData["User"] = userService.FirstOrDefault(u => u.Id == CurrentUserContract.Id);
            //Request.Cookies["Products"]
            InvoiceService invoiceService = new InvoiceService(objectContext);
            if (SessionUserContract != null)
            {
                var xx = Request.Cookies["Products"];
                // InvoiceService  invoiceService=new InvoiceService(objectContext);
                ViewData["invoicesCart"] = GetInvoisesCooki();
                  // objectContext.RejectAll();
            }
            ViewBag.HttpContext = HttpContext;

            if (parentCategories == null || parentCategories.Count == 0)
            {
                parentCategories = objectContext.Category.Where(c => c.Id == 2 || c.FkCategory == 2).Select(c => new KeyValuePair<string, string>(c.Discription, c.Name)).ToDictionary(x => x.Key, x => x.Value);
            }
            ViewBag.parentCategories = parentCategories;
            
          

            ViewBag.User = SessionUserContract;

            if (blackKey == "Promotion")
            {
                var objBlackKey = _promotionProductService.PromotionProductByTypes(PromotionTypes.HomeBlackKey).FirstOrDefault();
                if (objBlackKey == null) blackKey = "";
                else blackKey = objBlackKey.Title;
            }
            ViewBag.HomeBlackKeyTitle = blackKey;
        }

        public List<Invoice> GetInvoisesCooki()
        {
            var Products = CookieInvoiceToDictionary();
            return GetInvoisesCooki(Products);
        }

        public List<Invoice> GetInvoisesCooki(Dictionary<int, int> Products)
        {
            List<Invoice> ListInvoiceInCooki = new List<Invoice>();
            if (Products != null)
            {
                InvoiceService invoiceService = new InvoiceService(objectContext);
                ProductService productService = new ProductService(objectContext);
                foreach (var product in Products)
                {
                    Product productObj = productService.FirstOrDefault(p => p.Id == product.Key);
                    //این شرط چک می کند که کالا قبلا به در  فاکتوری ثبت نشده باشد
                    if (productObj != null && !ListInvoiceInCooki.Any(i => i.FkBusinessOwner == productObj.FkBusinessOwner))
                    {
                        Invoice invoice = new Invoice();
                        invoiceService.DictionaryToInvoice(productObj.FkBusinessOwner, Products, ref invoice);

                        ListInvoiceInCooki.Add(invoice);
                    }
                }
            }
            return ListInvoiceInCooki;
        }

        public Dictionary<int, int> CookieInvoiceToDictionary()
        {
            var ProductsCookie = Request.Cookies["Products"];
            Dictionary<int, int> Products;
            if (string.IsNullOrEmpty(ProductsCookie))
            {
                Products = new Dictionary<int, int>();
            }
            else
            {
                Products = XmlSerializerUtility.DeSerialize<Dictionary<int, int>>(StringCipher.Decrypt(ProductsCookie, StringCipher.passPhrase));
            }
            return Products;
        }

        /// <summary>
        /// اگر لاگین موفقیت آمیز بود یک یوزر کانترکت بر می گرداند و گرنه یک نال
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="email"></param>
        /// <param name="objectContext"></param>
        /// <param name="CurrentSestion"></param>
        /// <returns></returns>
        public UserContract LoginEmailReturnUserContract(string pass, string email, OnlineShopping objectContext)
        {
            bool resultLogin = false;

            UserContract _userContract = LoginCheckWithEmail(pass, email, objectContext, ref resultLogin);



            //  اگر لاگین موفقیت آمیز بود مقدار سیشن را تغییر بده
            if (resultLogin)
            {
                // UserContract CurrentUserContractOld = Session == null ? null : (Session["User"] as UserContract);

                HttpContext.Session.SetObjectAsJson("User", _userContract);

                //Session["User"] = _userContract;

            }

            return _userContract;
        }

        protected void LogOutUser()
        {
            HttpContext.Session.SessionRemove("User");
        }
        public UserContract LoginMobileReturnUserContract(string pass, string mobile, OnlineShopping objectContext)
        {
            bool resultLogin = false;

            UserContract _userContract = LoginCheckWithMobile(pass, mobile, objectContext, ref resultLogin);

            //  اگر لاگین موفقیت آمیز بود مقدار سیشن را تغییر بده
            //if (resultLogin)
            //{


            //    HttpContext.Session.SetObjectAsJson("User", _userContract);



            //}

            return _userContract;
        }
        /// <summary>
        /// حدالمقدر از این متد مستقیما استفاده نشود
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="email"></param>
        /// <param name="objectContext"></param>
        /// <param name="resultLogin"></param>
        /// <returns></returns>
        private UserContract LoginCheckWithEmail(string pass, string email, OnlineShopping objectContext, ref bool resultLogin)
        {
            resultLogin = false;
            var _entity = new User();
            var _service = new UserService(objectContext);

            UserContract _userContract = null;
            _entity = _service.FirstOrDefault(u => u.Email == email);

            if (_entity != null && _entity.Password == pass.MD5Hash() && _entity.Ative != false)
            {
                _userContract = LoginUserToSession(_entity);
                resultLogin = true;
            }
            return _userContract;
        }

        /// <summary>
        /// حدالمقدر از این متد مستقیما استفاده نشود
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="email"></param>
        /// <param name="objectContext"></param>
        /// <param name="resultLogin"></param>
        /// <returns></returns>
        private UserContract LoginCheckWithMobile(string pass, string mobile, OnlineShopping objectContext, ref bool resultLogin)
        {
            resultLogin = false;
            var _entity = new User();
            var _service = new UserService(objectContext);

            UserContract _userContract = null;
            _entity = _service.FirstOrDefault(u => u.Mobile == mobile);

            if (_entity != null && _entity.Password == pass.MD5Hash())// && _entity.Ative != false to do لاگین بدون تایید
            {
                _userContract = LoginUserToSession(_entity);
                resultLogin = true;
            }
            return _userContract;
        }

        internal UserContract LoginUserToSession(User _entity)
        {
            UserContract _userContract = null;
            var _maper = new UserMaper();
            _userContract = _maper.EntityToContract(_entity);
            updateInvoiceUserAndSesstion(_userContract);
            return _userContract;
        }

        public  void updateInvoiceUserAndSesstion(UserContract _userContract)
        {
            _userContract.InvoiveUser = _invoiceService.GetInvoiceUser(_userContract.Id);
            _userContract.CountWishes = _wishService.GetCountWishes(_userContract.Id);

            HttpContext.Session.SetObjectAsJson("User", _userContract);
        }

        // private UserContract _userContract;
        public UserContract SessionUserContract
        {
            get
            {
                if (HttpContext.Session != null && !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("User")))
                    return HttpContext.Session.GetObjectFromJson<UserContract>("User");
                else
                {
                    return null;
                }
            }
        }

        //public User CurrentUser
        //{
        //    get
        //    {//no call any req
        //        if (ViewData["User"] != null) return ((User)ViewData["User"]);
        //        else
        //        {
        //            var userService = new UserService(objectContext);
        //            ViewData["User"] = userService.FirstOrDefault(u => u.Id == CurrentUserContract.Id);
        //            return ((User)ViewData["User"]);
        //        }
        //    }

        //}
        //  BusinessOwnerContract _currentBusinessOwnerContract;
        /// <summary>
        /// اگر کاربر جاری رول بیزینس اونیر داشته باشد این پروپرتی دارای مفدار خواهد بود
        /// </summary>


        //protected void ValidateAccessToAction(int RoleValue)
        //{
        //    //رول مجاز اگر با رول جاری یکی باشد مجوز دسترسی داده میشود
        //    if (SessionUserContract == null ||
        //        !(SessionUserContract.FK_Role == RoleValue||
        //        //رول جاری اگر آدمین باشد دسترسی داده میشود
        //         SessionUserContract.FK_Role == RolesSystem.AdminValue
        //      ))
        //    {
        //        //return  RedirectToAction("LoginView", "User", new { message = "کاربر گرامی لطفا لاگین فرمایید" });
        //        // /User/LoginView
        //        //Response.ClearHeaders();
        //        // Response.Clear();

        //        //  Response.BufferOutput = false;
        //        //   Response.Flush();
        //        Response.Redirect(AppSetting.DomainName + "/User/LoginView?message=" + System.Net.WebUtility.UrlEncode("کاربر گرامی لطفا لاگین فرمایید"));
        //        //Response.Redirect(AppSetting.DomainName + "/User/LoginView?message=" + "کاربر گرامی لطفا لاگین فرمایید");

        //        // throw new MyException((byte)ExceptionType.AccessRoute, ExceptionType.AccessRoute.ToString(), "کاربر گرامی لطفا لاگین فرمایید");
        //    }

        //}

        protected bool ValidateAccessToActionBool(int RoleValue)
        {
            //رول مجاز اگر با رول جاری یکی باشد مجوز دسترسی داده میشود
            if (SessionUserContract != null)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        protected void ValidateAccessAdminOnly()
        {
            //رول جاری اگر آدمین باشد دسترسی داده میشود
            if (SessionUserContract.FK_Role == RolesSystem.AdminValue)
            {
                return;
            }
            else
            {
                Response.Redirect(AppSetting.DomainName + "/User/LoginView?message=کاربر گرامی لطفا لاگین فرمایید");
            }

        }


        protected void ExpireCookie(string CookieName)
        {
            Response.Cookies.Append(CookieName, "", new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(-1)
            });
        }

        protected void addOrChangeCookie(string CookieName, string value)
        {
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(5)
            };
            Response.Cookies.Append(CookieName, value, cookieOptions);
        }

        protected string getValeCookie(string CookiesName)
        {
            if (!string.IsNullOrEmpty(Request.Cookies[CookiesName]))
            {
                return Request.Cookies[CookiesName];
            }
            else return "0";
        }
        protected int? getValeCookieFK_Marketer()
        {
            string FK_MarketerStr = getValeCookie("FK_Marketer");
            int? FK_Marketer = string.IsNullOrWhiteSpace(FK_MarketerStr) || FK_MarketerStr == "0" ? null : (int?)int.Parse(FK_MarketerStr);
            return FK_Marketer;
        }

        protected void setValeCookieFK_Marketer(int fkMarketer)
        {
            addOrChangeCookie("FK_Marketer", fkMarketer.ToString());
        }

    }
}
