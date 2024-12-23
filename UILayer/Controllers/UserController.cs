using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataLayer;
using ServiceLayer;
using UILayer.Maper;
using DataLayer.Contract;
using DataLayer.Enums;
using UILayer.Miscellaneous;

using Utility;

using DataLayer.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.OleDb;
using DataLayer.EFLog;

namespace UILayer.Controllers
{
    /// <summary>
    /// استUser کلاس کنترولر موجودیت
    /// </summary>
    public partial class UserController : Base0Controller
        //<User, UserService, UserMaper, UserContract>,  IInvoiceState, IInvoiceStateView, ICURDOperation<User>
    {
        Random _random;
        UserService _service;
        InvoiceService _invoiceService;
        User _entity;
        public UserController(OnlineShopping onlineShopping):base("", onlineShopping)
        {
            _service = new UserService(onlineShopping);
            _random = new Random();


        }
        //public ActionResult smsTest()
        //{
        //   //Parsgreen.sendSMS
        // int successCount = 0;
        //    string[] ReturnStr = null;
        //    parsgreen.SendSMS.SendSMS x = new parsgreen.SendSMS.SendSMS();
        //int res=    x.SendGroupSMS("29E18A45-F6AF-44F1-BDD8-494C825DE7F1", "10001393", new string[] { "09362528282" }, "تست  3 test test اس ام اس پارس گرین"
        //        , false, string.Empty, ref successCount, ref ReturnStr);
        //return RedirectToAction("ShowMessage", "ShowContent", new { message = "send test rse: " + res + " successCount:" + successCount
        //+ " ReturnStr: " + ReturnStr[0].ToString() 
        //});
        //}

        public ActionResult LogOut()
        {
            // if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            //return Login(ConstSetting.EmailGuest, ConstSetting.Password);
            //Login(string Email, string Password)

            LogOutUser();

            return RedirectToAction("Index", "Home");
           //LoginReturnUserContract(ConstSetting.Password, ConstSetting.EmailGuest, objectContext); 
        }

        protected  UserContract CheangeTContract(UserContract usercontract)
        {
          //  usercontract.Agreement = UIUtility.ResourceManager.GetString("Agrement");
            return usercontract;
        }
        //protected  ActionResult DeterminViewSaveChange(string ViewName, User Entity, int PropValue, string PropName)
        //{
        //     if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
        //    return base.DeterminViewSaveChange(ViewName, Entity, PropValue, PropName);

        //}

        public string ValidateRepeatEmail(string id)
        {
            id = id.ToLatinNumber();
            _entity = _service.FirstOrDefault(u => u.Email.Trim().ToLower() == id.Trim().ToLower());
            if (_entity == null) return "درست";
            else return "Error";
        }

        public string ValidateRepeatMobile(string id)
        {
            id = id.ToLatinNumber();
            _entity = _service.FirstOrDefault(u => u.Mobile.Trim().ToLower() == id.Trim().ToLower());
            if (_entity == null) return "درست";
            else return "Error";
        }
        public ActionResult Captha(int? id)
        {
            
            // var captcha = new Utility.Captcha();
            // var bmp = captcha.CreateImage();
            var captcha = new CaptchaActionResault();
          HttpContext.Session.SetString("capcthText",captcha.capcthText);
            //bmp.Save(Response.OutputStream, ImageFormat.Gif);
            return captcha;
            //   Session["captchaText"] = 
        }
        //لود ایجکسی عکس کپچا
        public ActionResult CapthaImgTagAjax()
        {

            return View();
            //   Session["captchaText"] = 
        }

        #region register user
        public ActionResult LoginView(string message = "" , string redirectUrl="")
        {
            ViewData["message"] = message;
            ViewData["redirectUrl"] = redirectUrl;
            return View(new User());
        }
        public ActionResult Login(string mobile, string password , string redirectUrl = "")
        {
            mobile = mobile.ToLatinNumber();
            password = password.ToLatinNumber();
               // UserContract userContractOld = CurrentUserContract;
               UserContract userContractNew = LoginMobileReturnUserContract(password, mobile, objectContext); 
          
            if (userContractNew != null )
            {
                //  بعد از لاگین کالاهای موجود در کوکی را به فاکتور دیتابیس اضافه می نماییم
                //اگر در کوکی کاربر فاکتوری باشد ان را به فاکتورهای کاربر اضافه می نمایید 
                ViewBag.message = "ورود شما به سایت موفق بود";
                CookiToInvoiceAfterLogin();
                if (!string.IsNullOrWhiteSpace(redirectUrl)) return Redirect(redirectUrl);
                else return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LoginView", new { message = UILayer.Miscellaneous.UIUtility.ResourceManager.GetString("InorrectLagin") });
            }
        }

        public ActionResult RegisterView(string message = "")
        {
            ViewData["message"] = message;
            ViewBag.agreement = objectContext.Content.FirstOrDefault(c => c.Id == 30);
            return View("LoginView", new User());
        }
        public ActionResult RegisterUser(User user, string redirectUrl = "") //, string captcha)
        {
            //string SessionCaptcha =HttpContext.Session.GetString("capcthText");
            //HttpContext.Session.SetString("capcthText", ""); 
            // if (SessionCaptcha != captcha) return RedirectToAction("RegisterView", "User", new { message = "کاربر  گرامی لطفا کارکتر های عکس را به طور صحیح وارد نمایید" });
            //  user.Email = user.Email.Trim();
            user.Mobile = user.Mobile.ToLatinNumber();
            user.Password = user.Password.ToLatinNumber();
            string passTemp = user.Password;

            var existUser = objectContext.User.FirstOrDefault(u => u.Mobile == user.Mobile);
            //existUser?.Ative == true to do بدون تایید
            if (existUser != null) return RedirectToAction("LoginView", new { message = "حساب کاربری، قبلا با این شماره ثبت شده است اگر کلمه عبور را فراموش کرده اید از طریق بازیابی کلمه عبور اقدام نمایید " });
            else if (existUser?.Ative == false) return RedirectToAction("SendActivationCodeView", new { mobile = user.Mobile, message = "حساب کاربری، قبلا با این شماره ثبت شده است برای فعال سازی حساب کاربری از این قسمت اقدام نمایید" });

            user.Ative = false;
            user.RegisterDate = UIUtility.CurrentDate;
            var activationCode = ExtentionMethods.DeliveryCoded(5);//Guid.NewGuid().ToString().Substring(0, 5);
            user.AtivationCode = activationCode;
            user.FkRole = RolesSystem.UserValue;
            user.Password = user.Password.MD5Hash();
          
            _service.Add(user);
            _service.SaveAllChengeOrAllReject(true);

            MessageService messageService = new MessageService(objectContext);
           
            return RedirectToAction("Login", new { mobile = user.Mobile, password = passTemp, redirectUrl = redirectUrl });

            // to do  return RedirectToAction("SendActivationCode",  new { mobile=user.Mobile,  message = "لطفا کد فعال سازی را وارد نمایید" });
        }
        
        public ActionResult SendActivationCodeView(string mobile ="", string message="" )
        {
            mobile = mobile.ToLatinNumber();
            ViewBag.message = message;
        
            ViewBag.mobile = mobile;
            ViewBag.ViewMode = "activeFirst"; 
            return View("ForgetPasswordView");
        }
        public ActionResult SendActivationCode(string mobile, string captcha, string message = "")
        {
            //string SessionCaptcha =HttpContext.Session.GetString("capcthText");
            //HttpContext.Session.SetString("capcthText","");
            //if (SessionCaptcha != captcha) return RedirectToAction("ShowMessage", "ShowContent", new { message = "کاربر  گرامی لطفا کارکتر های عکس را به طور صحیح وارد نمایید" });
            mobile = mobile.ToLatinNumber();
            captcha = captcha.ToLatinNumber();
            ViewBag.mobile = mobile;
            ViewBag.message = message;

            _entity = _service.FirstOrDefault(u => u.Mobile == mobile);
            if (_entity != null)
            {
                if (_entity.Ative != null &&  _entity.Ative == true) return RedirectToAction("LoginView", new { message = " این حساب کاربری قبلا فعال شده است لطفا لاگین نمایید" });
                MessageService messageService = new ServiceLayer.MessageService(objectContext);
                _entity.AtivationCode = _random.Next(1000, 9999).ToString();
                // messageService.SendActivationCodeToEmail(_entity);
                messageService.SendActivationCodeToMobile(_entity.Mobile,_entity.AtivationCode);
                _service.SaveAllChengeOrAllReject(true);
                ViewBag.ViewMode = "sendedCode";
                ViewBag.mobile = mobile;
                return View("ForgetPasswordView", new { mobile= mobile});
                //RedirectToAction("ShowMessage", "ShowContent", new { message = "کد فعال سازی به ایمیلتان ارسال شد" });
            }
            else
            {
                return RedirectToAction("SendActivationCodeView", new { message = "موبایل وارد شده اشتباه می باشد" });
            }
        }
        [HttpPost]
        public ActionResult ActivateUser(string mobile, string activateCode )
        {
            mobile = mobile.ToLatinNumber();
            activateCode = activateCode.ToLatinNumber();
            _entity = _service.FirstOrDefault(u => u.Mobile == mobile);
            if (_entity.AtivationCode == activateCode && (_entity.Ative == null || _entity.Ative == false))
            {
                _entity.Ative = true;
                _service.SaveChanges();
                LoginUserToSession(_entity);
                CookiToInvoiceAfterLogin();
                ViewBag.Toasts = "لاگین شماموفق بود";
                return RedirectToAction("Index","Home") ;
            }
            else if (_entity.Ative == true) return RedirectToAction("LoginView", new { message = " این حساب کاربری قبلا فعال شده است لطفا لاگین فرمایید" });
            else return RedirectToAction("SendActivationCodeView", new { message = "عملیات فعال سازی ناموفق بود دوباره امتحان کنید " });
        }

        public ActionResult ForgetPasswordView(string message = "")
        {
            ViewBag.forgetMessage = message;
            ViewBag.ViewMode = "activeFirst";
            return View( );
        }
        public ActionResult RecoveryPassword(string mobile, string captcha)
        {
            //string SessionCaptcha =HttpContext.Session.GetString("capcthText");
            //HttpContext.Session.SetString("capcthText", "");
            //if (SessionCaptcha != captcha) return RedirectToAction("ShowMessage", "ShowContent", new { message = "کاربر  گرامی لطفا کارکتر های عکس را به طور صحیح وارد نمایید" });
            mobile = mobile.ToLatinNumber();
            captcha = captcha.ToLatinNumber();
            _entity = _service.FirstOrDefault(u => u.Mobile ==mobile);
            if (_entity != null)
            {
                string newPassword = Guid.NewGuid().ToString().Substring(0, 4);
                _entity.Password = newPassword.MD5Hash();

                MessageService messageService = new ServiceLayer.MessageService(objectContext);
              //  messageService.SendPasswordToEmail(_entity.Email, newPassword);
                messageService.SendNewPasswordToMobile(_entity.Mobile, newPassword);

                _service.SaveAllChengeOrAllReject(true);
                return RedirectToAction("LoginView",  new { message = "کلمه عبور به موبایل شما ارسال شد" });

            }
            else
            {
                return RedirectToAction("ForgetPasswordView", new { message = "موبایل وارد شده اشتباه می باشد" });
            }
        }
        public ActionResult ChangePasswordAndInfo (string oldPassword,string fullname="",  string NewPassword="")
        {
            oldPassword = oldPassword.ToLatinNumber();
            fullname = fullname.ToLatinNumber();
            NewPassword = NewPassword.ToLatinNumber();
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            _entity = _service.FirstOrDefault(u => u.Id == SessionUserContract.Id);

            //چک کردن پسورد
            if (_entity.Password != oldPassword.MD5Hash())
            { return RedirectToAction("ShowMessage", "ShowContent", new { message = "کلمه عبور وارد شده اشتباه می باشد" }); }

            if (NewPassword != "")
            {
                //تغییر پسورد
                _entity.Password = NewPassword.MD5Hash();
            }
            _entity.Name = fullname;
         
           _service.SaveAllChengeOrAllReject(true);

            ViewBag.toasts = "اطلاعات شما با موفقیت تغییر یافت";

           return RedirectToAction("Index", "Home");
        }



        private void CookiToInvoiceAfterLogin()
        {
            if (Request.Cookies["Products"] != null)
            {
               
                InvoiceService invoiceService = new InvoiceService(objectContext);
                string MessageIsNewInvoice = "";
                Dictionary<int, int> CookieDictionary = CookieInvoiceToDictionary();
                string Fk_MarketerStr = getValeCookie("FK_Marketer");

                ExpireCookie("Products");//کوکی قبل از اعمال پاک می گردد به این دلیل که ،اگر دادهای آن خطا ایجاد کرد در روند خرید بعداز لاگین اشکال ایجاد نکند 
                invoiceService.AddCookieInvoiceToDbInvoice(SessionUserContract.Id, 0, CookieDictionary, ref MessageIsNewInvoice);
                updateInvoiceUserAndSesstion(SessionUserContract);
                //بعد از اضافه شدن کالاهای موجود در کوکی به فاکتور دیتابیس آنها را در سبد کاربر اضافه می نمایین
                ViewData["invoicesCart"] = _service.FindMyLastInvoice(SessionUserContract.Id);
            } /**/
        }

        #endregion ICURDOperation
        public virtual ActionResult MyAccount()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            return View(_service.FirstOrDefault(m => m.Id == SessionUserContract.Id));
        }
        public virtual ActionResult SaveInfoUser(Gender idGender, string firstName ,string lastName, string emailName,
            string birthday , bool spicialOffer , bool newsletter )
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            _service.SaveInfoUser(SessionUserContract.Id,idGender, firstName, lastName , emailName, birthday , spicialOffer, newsletter);
            return RedirectToAction("MyAccount");
        }
        public virtual ActionResult ChangePassword( string password, string newPassword)
        {
            password = password.ToLatinNumber();
            newPassword = newPassword.ToLatinNumber();

             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
          var result=  _service.ChangePassword(SessionUserContract.Id, password , newPassword);
            if (result) ViewBag.toast = "تغییر کلمه عبور با موفقیت انجام شد";
            else ViewBag.toast = "تغییر کلمه عبور ناموفق بود";
            return RedirectToAction("MyAccount");
        }
        //
        //لیست سفارشات را در هر مرحله نمایش می دهد
        #region InterfaceIInvoiceState

        public ActionResult Invoices()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            var _invoiceService = new InvoiceService(objectContext);
            CreateTotalListInvoices();
            return View(_invoiceService.Find(i => i.FkUser == SessionUserContract.Id));
        }
        public ActionResult AInvoice()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
          
            CreateTotalListInvoices();

            return View((object)"User");
        }
        public ActionResult initialize()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            InvoiceService _invoiceService = new InvoiceService(objectContext);
            CreateTotalListInvoices();
            return View("Invoices", _service.FindMyLastInvoice(SessionUserContract.Id));
        }
        public ActionResult request()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            CreateTotalListInvoices();
            var _invoiceService = new InvoiceService(objectContext);
            return View("Invoices", _invoiceService.Find(i => i.FkUser == SessionUserContract.Id ));
        }
        public ActionResult BusinessOwnerAccept()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            CreateTotalListInvoices();
            var _invoiceService = new InvoiceService(objectContext);
            return View("Invoices", _invoiceService.Find(i => i.FkUser == SessionUserContract.Id ));
        }
        public ActionResult ToPay()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            CreateTotalListInvoices();
            var _invoiceService = new InvoiceService(objectContext);
            return View("Invoices", _invoiceService.Find(i => i.FkUser == SessionUserContract.Id));
        }
        public ActionResult payment()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 

            CreateTotalListInvoices();
            var _invoiceService = new InvoiceService(objectContext);
            return View("Invoices", _invoiceService.Find(i => i.FkUser == SessionUserContract.Id ));
        }
        public ActionResult Send()
        {

             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            CreateTotalListInvoices();
            var _invoiceService = new InvoiceService(objectContext);
            return View("Invoices", _invoiceService.Find(i => i.FkUser == SessionUserContract.Id ));
        }
        public ActionResult Delivered()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 

            CreateTotalListInvoices();
            var _invoiceService = new InvoiceService(objectContext);
            return View("Invoices", _invoiceService.Find(i => i.FkUser == SessionUserContract.Id));
        }
        public ActionResult Final()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 

            CreateTotalListInvoices();
            var _invoiceService = new InvoiceService(objectContext);
            return View("Invoices", _invoiceService.Find(i => i.FkUser == SessionUserContract.Id ));
        }
        public ActionResult canceled()
        {

             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 

            CreateTotalListInvoices();
            var _invoiceService = new InvoiceService(objectContext);
            return View("Invoices", _invoiceService.Find(i => i.FkUser == SessionUserContract.Id ));
        }

       

        #endregion InterfaceIInvoiceState

        #region InterfaceIInvoiceStateView
        public ActionResult initializeGuestView(int id)
        {
            InvoiceService _invoiceService = new InvoiceService(objectContext);
            List<Invoice> invoices = GetInvoisesCooki();
            Invoice invoice = invoices.FirstOrDefault(i => i.FkBusinessOwner == id);
            var provinceService = new ProvinceService(objectContext);
            ViewData["ProvinceShipping"] = provinceService.Find(p => p.BridgeProvinceBusinessOwner.Any(b => b.FkBusinessOwner == id));
            List<short> stateListShower = new List<short>();
            stateListShower.Add((byte)InvoiceStatus.initialize);
            stateListShower.Add((byte)InvoiceStatus.ActionToPay);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.Send);
            stateListShower.Add((byte)InvoiceStatus.Final);
            ViewData["currentStateList"] = stateListShower;
            ViewData["currentState"] = invoice.Status;
            invoice.FkMarketer = getValeCookieFK_Marketer();
            return View("DeterminView", invoice);
        }
        public ActionResult AInvoiceView(int Id)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            InvoiceService _invoiceService = new InvoiceService(objectContext);
            Invoice invoice = _invoiceService.FirstOrDefault(i => i.Id == Id);

            if (invoice == null || invoice.FkUser != SessionUserContract.Id)
                return RedirectToAction("ShowMessage", "ShowContent", new { message = "کد فاکتور وارد شده اشتباه می باشد یا خریدار این فاکتور شما نیستید " });
            else
                return RedirectToAction(EnumUtility.EnumShortValueToStringValue<InvoiceStatus>((byte)invoice.Status) + "View", new { Id = Id });
        }

        public ActionResult initializeView(int Id)
        {
            return RedirectToAction("DeterminView", new { Id = Id });
        }

     
        public ActionResult RequestView(int Id)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 

            var _invoiceService = new InvoiceService(objectContext);
            var invoice = _invoiceService.FirstOrDefault(i => i.Id == Id);

            var provinceService = new ProvinceService(objectContext);
            ViewData["Province"] = provinceService.ProvinceShippingBusinessOwner(invoice.FkBusinessOwner, invoice.FkBusinessOwnerNavigation.FkProvince);
            List<short> stateListShower = new List<short>();
            stateListShower.Add((byte)InvoiceStatus.initialize);
            stateListShower.Add((byte)InvoiceStatus.Final);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.Send);
            stateListShower.Add((byte)InvoiceStatus.Final);


            ViewData["currentState"] = invoice.Status;
            ViewData["currentStateList"] = stateListShower;
            ViewData["ShippingCompany"] = invoice.ShippingCompany;

            return View(invoice);
        }
        public ActionResult BusinessOwnerAcceptView(int Id)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            InvoiceService _invoiceService = new InvoiceService(objectContext);
            Invoice invoice = _invoiceService.FirstOrDefault(i => i.Id == Id);

            ProvinceService provinceService = new ProvinceService(objectContext);
            ViewData["Province"] = provinceService.ProvinceShippingBusinessOwner(invoice.FkBusinessOwner, invoice.FkBusinessOwnerNavigation.FkProvince);
            List<short> stateListShower = new List<short>();
            stateListShower.Add((byte)InvoiceStatus.initialize);
            stateListShower.Add((byte)InvoiceStatus.ActionToPay);
            stateListShower.Add((byte)InvoiceStatus.Final);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.Send);
            stateListShower.Add((byte)InvoiceStatus.Final);


            ViewData["currentState"] = invoice.Status;
            ViewData["currentStateList"] = stateListShower;
            return View(invoice);
            // throw new NotImplementedException();
        }
        public ActionResult ToPayView(int Id)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            InvoiceService _invoiceService = new InvoiceService(objectContext);
            Invoice invoice = _invoiceService.FirstOrDefault(i => i.Id == Id);
            List<short> stateListShower = new List<short>();
            stateListShower.Add((byte)InvoiceStatus.initialize);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.ActionToPay);
            stateListShower.Add((byte)InvoiceStatus.Send);
            stateListShower.Add((byte)InvoiceStatus.Final);


            ViewData["currentState"] = invoice.Status;
            ViewData["currentStateList"] = stateListShower;
            ProvinceService provinceService = new ProvinceService(objectContext);
            ViewData["Province"] = provinceService.ProvinceShippingBusinessOwner(invoice.FkBusinessOwner, invoice.FkBusinessOwnerNavigation .FkProvince);
            return View(invoice);
        }
        public ActionResult paymentView(int Id)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            var _invoiceService = new InvoiceService(objectContext);
            Invoice invoice = _invoiceService.FirstOrDefault(i => i.Id == Id);

            var provinceService = new ProvinceService(objectContext);
            ViewData["Province"] = provinceService.ProvinceShippingBusinessOwner(invoice.FkBusinessOwner, invoice.FkBusinessOwnerNavigation.FkProvince);
        
            List<short> stateListShower = new List<short>();
            stateListShower.Add((byte)InvoiceStatus.initialize);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.Send);
      
          

            ViewData["currentState"] = invoice.Status;
            ViewData["currentStateList"] = stateListShower;
            return View(invoice);
        }
        public ActionResult SendView(int Id)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            InvoiceService _invoiceService = new InvoiceService(objectContext);
            Invoice invoice = _invoiceService.FirstOrDefault(i => i.Id == Id);

            List<short> stateListShower = new List<short>();
            stateListShower.Add((byte)InvoiceStatus.initialize);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.Send);



            ViewData["currentState"] = invoice.Status;
            ViewData["currentStateList"] = stateListShower;
            return View(invoice);
        }
        public ActionResult DeliveredView(int Id)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            InvoiceService _invoiceService = new InvoiceService(objectContext);
            Invoice invoice = _invoiceService.FirstOrDefault(i => i.Id == Id);
            List<short> stateListShower = new List<short>();
            stateListShower.Add((byte)InvoiceStatus.initialize);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.Send);



            ViewData["currentState"] = invoice.Status;
            ViewData["currentStateList"] = stateListShower;
            return View(invoice);
        }
        public ActionResult FinalView(int Id)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            var _invoiceService = new InvoiceService(objectContext);
            var invoice = _invoiceService.FirstOrDefault(i => i.Id == Id);
            List<short> stateListShower = new List<short>();
            stateListShower.Add((byte)InvoiceStatus.initialize);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.Send);

            stateListShower.Add((byte)InvoiceStatus.Final);


            ViewData["currentState"] = invoice.Status;
            ViewData["currentStateList"] = stateListShower;
            return View("CanceleView", invoice);
        }
        public ActionResult CanceleView(int Id)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            var _invoiceService = new InvoiceService(objectContext);
            var invoice = _invoiceService.FirstOrDefault(i => i.Id == Id);
            List<short> stateListShower = new List<short>();
            stateListShower.Add((byte)InvoiceStatus.initialize);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.Send);
    


            ViewData["currentState"] = invoice.Status;
            ViewData["currentStateList"] = stateListShower;
            return View(invoice);
        }

        #endregion InterfaceIInvoiceStateView

  

        public virtual ActionResult Chat(string toSestionUserId, string subject)
        {
            toSestionUserId = toSestionUserId.ToLatinNumber();
            ViewData["toSestionUserId"] = toSestionUserId;
            ViewData["subject"] = subject;
            return View();
        }

        private void CreateTotalListInvoices()
        {
            InvoiceService invoiceService = new InvoiceService(objectContext);
            ViewData["controlerName"] = "User";
            ViewData["currentStateList"] = invoiceService.FullStateList();
        }

    }
}
