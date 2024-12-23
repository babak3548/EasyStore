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
using UILayer.BankGetWays;
using UILayer.Models;
using System.Xml.Serialization;
using System.IO;

using Microsoft.AspNetCore.Mvc;
using DataLayer.EF;
using Microsoft.AspNetCore.Http;
using DataLayer.Models;
using System.Net.Http;
using NotifictionService;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataLayer.EFLog;
using System.Text.Json;
using UILayer.BankGetWays.Meli;

namespace UILayer.Controllers
{

    /// استInvoice کلاس کنترولر موجودیت
    /// </summary>
    public class InvoiceController : BaseController<Invoice, InvoiceService, InvoiceMaper, InvoiceContract>

    {
        Product _product = new Product();
        LogService _LogService;

        BridgeInvoiceProduct _bridge_Invoice_Product = new BridgeInvoiceProduct();

        AccountingService _accountingService;
        ProvinceService _provinceService;
        MessageService _messageService;
        string MessageIsNewInvoice = "";

        Meli _meli;
        //CreateDocument(string Name, decimal Creditor, decimal Debtor, DateTime date, long FkPaymentLog, int FkUser)
        public InvoiceController(OnlineShopping _onlineShopping, EasyStoreLog _EasyStoreLog)
            : base("Invoice", _onlineShopping)
        {
            EasyStoreLog = _EasyStoreLog;

            _service = new InvoiceService((objectContext) as OnlineShopping);
            _maper = new InvoiceMaper((objectContext) as OnlineShopping);
            _entity = new Invoice();
            _accountingService = new AccountingService(objectContext);
            _provinceService = new ProvinceService(objectContext);
            _messageService = new MessageService(objectContext);
            _LogService = new LogService(EasyStoreLog);
            _meli = new Meli();

        }

        public ActionResult AInvoice(int Id)
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User");

            var provinceService = new ProvinceService(objectContext);
            _entity = _service.FirstOrDefault(i => i.Id == Id);
            if (_entity.FkUser == SessionUserContract.Id || SessionUserContract.FK_Role == RolesSystem.AdminValue)
            {
                return View("initializeScenario", _entity);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
            //  ViewData["Province"] = provinceService.Find(p => p.BridgeProvinceBusinessOwner.Any(b => b.FkBusinessOwner == _entity.FkBusinessOwner));

        }

        public ActionResult InitializeInvoiceFromCooki(int ProductId)
        {
            Dictionary<int, int> Products;
            //  var x = Request.Cookies["Products"];
            //  Dictionary<int, int> ProductsTemp;
            Products = CookieInvoiceToDictionary();
            bool flagAdd = false;
            //    ProductsTemp = Products;
            foreach (var product in Products.ToList())
            {
                if (product.Key == ProductId)
                {
                    //تغییر در تیمپ  انجام می گیرد
                    //  Products[product.Key] = product.Value + 1;//&*
                    flagAdd = true;
                }

            }
            //   Products = ProductsTemp;
            if (!flagAdd)
            {
                Products.Add(ProductId, 1);
            }

            //_service.DictionaryToInvoice(FK_BusinessOwner, Products, ref _entity);
            //_entity.FK_Marketer = FK_Marketer;

            string ProductsSerializer = XmlSerializerUtility.Serialize(Products);
            var provinceService = new ProvinceService(objectContext);
            //var x = provinceService.Find(p => p.Bridge_Province_BusinessOwner.Any(b => b.FK_BusinessOwner == FK_BusinessOwner));
            ViewData["Province"] = provinceService.GetAll();

            // Response.Cookies.Add(new HttpCookie(, ));
            addOrChangeCookie("Products", StringCipher.Encrypt(ProductsSerializer, StringCipher.passPhrase));
            //return View(@"~/Views/User/InitializeView.cshtml", invoiceObj);

            //  return View("~/Views/User/InitializeView.cshtml", _entity);
            InvoiceService invoiceService = new InvoiceService(objectContext);
            ViewData["invoicesCart"] = GetInvoisesCooki();
            return View("CartPartial");
        }
        #region IInvoice"Index","Product"Scenario
        //همان اینشلایز هست

        #endregion  IInvoiceStatusScenario

        #region cart

        public ActionResult AddItemsToCart(int productId, int productCount, string productColor)
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User", new { redirectUrl = Url.Action("AddItemsToCart", "Invoice", new { productId = productId, productCount = productCount, productColor = productColor }) });
            //RedirectToAction("AddItemsToCartGuest", new { ProductId = productId });

            ProductService _productService = new ProductService(objectContext);
            UserService userService = new UserService(objectContext);
            //چک کردن محصول
            _product = _productService.FirstOrDefault(p => p.Id == productId);
            if (_product == null) throw new Exception("محصول مورد نظر وجود ندارد");
            // return RedirectToAction("ShowMessage", "ShowContent", new { message = "خرید از فروشگاه خود تعریف نشده است " });
            string productCountMsg = "";
            if (_product.MinCountForPrice != null && _product.MinCountForPrice > productCount)
            {
                productCount =(int) _product.MinCountForPrice;
                productCountMsg = string.Format("حداقل تعداد این محصول در یک سفارش {0} عدد می باشد", productCount);
            }
            _entity = _service.CreateOrUpdateAddProductToInvoice(_product.FkBusinessOwner, SessionUserContract.Id, _product.Id, getValeCookieFK_Marketer(), productCount, productColor, ref MessageIsNewInvoice);
            //_entity.Status = 0;//یعنی هنوز درخواست به فروشنده ارسال نشده است
            updateInvoiceUserAndSesstion(SessionUserContract);
            return RedirectToAction("GetCart", new { invoiceId = _entity.Id ,cartMsg = productCountMsg});
        }
        public ActionResult UpdateCountProduct(string[] bipc, int invoiceId)
        {
            _service.UpdateCountProduct(bipc, SessionUserContract.Id, invoiceId);
            updateInvoiceUserAndSesstion(SessionUserContract);
            return RedirectToAction("GetCart", new { invoiceId = invoiceId });
        }
        public ActionResult DeleteItemInvoice(int bipId)
        {
            var invoiceId = _service.DeleteItemInvoice(bipId);
            updateInvoiceUserAndSesstion(SessionUserContract);
            return RedirectToAction("GetCart", new { invoiceId = invoiceId });

        }

        public ActionResult GetCart(int invoiceId,string cartMsg = "")
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("GetCardGuest", new { invoiceId = invoiceId });
            Invoice invoice = _service.GetInvoice(invoiceId, SessionUserContract.Id);

            if (!string.IsNullOrWhiteSpace( cartMsg ))
                ViewBag.cartMsg = cartMsg;

            return View(invoice);
        }

        public ActionResult GetLastCart()
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User");
            Invoice invoice = _service.GetLastInvoice(SessionUserContract.Id);

            return View(invoice);
        }
        #endregion

        #region Payment pasargad
        //applyDiscount(long orderid, string coupon)
        public ActionResult GoToPayment(int invoiceId, string coupon)
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User");
            var invoice = _service.GetInvoice(invoiceId, SessionUserContract.Id);

            #region saveOrder
            _service.ApplyDiscount(invoiceId, coupon);
            #endregion
            updateInvoiceUserAndSesstion(SessionUserContract);
            return RedirectToAction("GetPaymentView", new { invoiceId = invoice.Id });
        }

        public ActionResult GetPaymentView(int invoiceId)
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User");
            ViewBag.LastInvoice = _service.GetLastwithAddress(SessionUserContract.Id);
            var invoice = _service.GetInvoice(invoiceId, SessionUserContract.Id);
            ViewBag.provinces = _provinceService.GetAll().ToList();
            ViewBag.shippingCompanies = EnumUtility.EnumToList<ShippingCompanies>().Where(t => t.Id > 0);

            return View(invoice);
        }

        #region paymentWithPasargad
        public JsonResult PaymentWithPasargad(long invoiceId, string address, string cityName, string noteForBussiness, int fkProvince,
          string name, string familyName, string company, string mobile, string tel, string postCode, ShippingCompanies shippingCompany)
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return Json("لطفا وارد حساب کاربری خود شوید");
            var invoice = _service.GetInvoice(invoiceId, SessionUserContract.Id);
            if (invoice == null) throw new Exception("کد در خواست درست نمی باشد");

            int? fkMarketre = getValeCookieFK_Marketer();
            fkProvince = fkProvince == 0 ? 8 : fkProvince;//to do
            _service.InvoiceGoToActionPayStatus(SessionUserContract.Id, invoice.Id, address, Banks.Pasargad,
                noteForBussiness, fkProvince, fkMarketre, name, familyName, company, mobile, tel, postCode,
                 shippingCompany, cityName
                );
            updateInvoiceUserAndSesstion(SessionUserContract);
            Result<PasargadData> PasargadData = getTokenPasargad(invoice.Id);
            return Json(PasargadData.ResultObject);
        }
        private Result<PasargadData> getTokenPasargad(long orderid)
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return new Result<PasargadData> { ActionExecuteSucsess = false, Message = "لطفا وارد حساب کاربری خود شوید" };
            var invoice = _service.GetInvoice(orderid, SessionUserContract.Id);
            if (invoice == null) throw new Exception("چنین سفارشی در سیستم برای شما ثبت نشده است");
            decimal payValue = invoice.TotalSumProductPrice - invoice.Discount;

            if (payValue < 100) throw new Exception("مبلغ زیر 100 تومان قابل پرداخت نیست");

            Pasargad pasargad = new Pasargad(invoice.PaymentToCountinue,
          objectContext, SessionUserContract.Id, invoice.Id + "پرداخت سفارش");

            PaymentLog payment = _service.CreatePaymentLog(invoice.Id,
                SessionUserContract.Id, invoice.PaymentToCountinue, invoice.Id + "پرداخت سفارش");

            Result<PasargadData> PasargadData = pasargad.GetTokenPay(payment);
            if (!PasargadData.ActionExecuteSucsess)
                throw new Exception("گرفتن تکن از بانک با خطا مواجه شد");
            return PasargadData;
        }
        //[HttpGet]
        //[Route("redirectpasargad")]
        public ActionResult RedirectPasargad(string iN, string iD, string tref)
        {
          //  if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User");
            _LogService.SaveInfo("Enter RedirectPasargad " + "iN:" + iN + "iD:" + iD + "tref:" + tref, "", "");
            long paymentLogId = long.Parse(iN);


            PaymentLog payLog = objectContext.PaymentLogs.FirstOrDefault(p => p.Id == paymentLogId);
            if (payLog == null) throw new Exception("کد پرداخت برگشتی اشتباه می باشد");
            //to do fori
            if (!(payLog.Status == PayLogType.request)) throw new InvalidOperationException("کاربر در زمان برگشت از بانک رفرش  انجام داده");
            payLog.Status = PayLogType.request; //PayLogType.request

            objectContext.SaveChanges();


            _LogService.SaveInfo("after method CheckPaymentStatus payLog:", "", payLog);
        
            #region payment 
            Pasargad pasargad = new Pasargad(payLog.Amount, objectContext, payLog.UserId, ""); //
                                                                                                        //  objectContext.SaveChanges();//tttt
            Result<PaymentLog> resbank = pasargad.Veryfiy(payLog, iD, tref);

            _LogService.SaveInfo("after method new Pasargad resbank:", "", resbank);
            #endregion

            if (!resbank.ActionExecuteSucsess)
            {

                ViewBag.messagePay = resbank.Message;

                return RedirectToAction("ShowMessage", "ShowContent", new
                {
                    message = resbank.Message
                });


            }


            if (payLog.FkInvoiceNavigation == null)
            {
                #region finacialDocument
                // _accountingService.PaymentUser(payLog.UserId, payLog.Id, payLog.Amount);
                #endregion
                throw new ExceptionForDisplay("پیش فاکتوری برای پرداخت یافت نشد");
            }
            else
            {
                checkBalanceWithDiscount((int)payLog.UserId, payLog.FkInvoiceNavigation.PaymentToCountinue, resbank.ResultObject.Amount);
                _LogService.SaveInfo("after method checkBalanceWithDiscount:", "", "");
                #region finacialDocument
                _accountingService.PaymentUserForInvoice((int)payLog.UserId, payLog.Id, payLog.Amount);
                _LogService.SaveInfo("after method PaymentUserForInvoice:", "", "");
                #endregion

                var invoivce = _service.FirstOrDefault(i => i.Id == resbank.ResultObject.InvoiceId);
                _service.ChangeStatusInvoice(invoivce, InvoiceStatus.payment);
                _LogService.SaveInfo("after method ChangeStatusInvoice:", "", "");
                _service.SaveAllChengeOrAllReject(true);
                _LogService.SaveInfo("after method SaveAllChengeOrAllReject:", "", "");

                //try
                //{
                //    updateInvoiceUserAndSesstion(SessionUserContract); // to do exit this code from try
                //    _messageService.SendPaymentRef(SessionUserContract.Mobile, invoivce.Id.ToString());//to do delete from
                //}
                //catch (Exception)
                //{
                //}

                return RedirectToAction("SuccessPayment", "ShowContent", new
                {
                    message = string.Concat("با تشکر از خرید شما، سفارش ", payLog.InvoiceId, " با موفقیت پرداخت شد"
                    , Environment.NewLine, ":شماره پی گیری ", tref) ,
                    invoivceId = payLog.InvoiceId
                });
            }
        }
        #endregion

        #region paymentWithMeli
        public ActionResult PaymentWithMeli(long invoiceId, string address, string cityName, string noteForBussiness, int fkProvince,
            string name, string familyName, string company, string mobile, string tel, string postCode, ShippingCompanies shippingCompany)
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User");
            var invoice = _service.GetInvoice(invoiceId, SessionUserContract.Id);
            if (invoice == null) throw new Exception("کد در خواست درست نمی باشد");

            int? fkMarketre = getValeCookieFK_Marketer();
            fkProvince = fkProvince == 0 ? 8 : fkProvince;//to do

            _service.InvoiceGoToActionPayStatus(SessionUserContract.Id, invoice.Id, address, Banks.Meli,
                noteForBussiness, fkProvince, fkMarketre, name, familyName, company, mobile, tel, postCode,
                   shippingCompany, cityName);
            updateInvoiceUserAndSesstion(SessionUserContract);


            if (invoice.PaymentToCountinue < 100) throw new Exception("مبلغ زیر 100 تومان قابل پرداخت نیست");

            PaymentLog payment = _service.CreatePaymentLog(invoice.Id,
        SessionUserContract.Id, invoice.PaymentToCountinue, invoice.Id + "پرداخت با ملی، سفارش");


            var res = _meli.GetToken(payment.Id.ToString(), (long)payment.Amount, Response);
            _LogService.SaveInfo("after meli.GetToken", "", "");
            _LogService.SaveInfo("after meli.GetToken", "", res);
            if (res != null && res.Result != null)
            {
                if (res.Result.ResCode == "0")
                {
                    _LogService.SaveInfo("if (res.Result.ResCode == \"0\")", "", "");
                    return Redirect(string.Format("{0}/Purchase/Index?token={1}", Meli._PurchasePage, res.Result.Token));
                }

                return RedirectToAction("ShowMessage", "ShowContent", new
                {
                    message = res.Result.Description
                });
            }

            return RedirectToAction("GetPaymentView", new { invoiceId = invoice.Id });
        }
        public ActionResult RedirectMeli(PurchaseResult result)
        {
            _LogService.SaveInfo("Enter RedirectMeli befor login ", "", "");
            _LogService.SaveInfo("Enter RedirectMeli ", "", result);

             long paymentLogId = long.Parse(result.OrderId);


            PaymentLog payLog = objectContext.PaymentLogs.FirstOrDefault(p => p.Id == paymentLogId);
            if (payLog == null) throw new Exception("کد پرداخت برگشتی اشتباه می باشد");
            //to do fori
            if (!(payLog.Status == PayLogType.request)) throw new InvalidOperationException("کاربر در زمان برگشت از بانک رفرش  انجام داده");
            payLog.Status = PayLogType.request; //PayLogType.request

            objectContext.SaveChanges();
            _LogService.SaveInfo("after method CheckPaymentStatus payLog:", "", payLog);
            #region payment

            var resutVerfiy = _meli.Verify(result);
            _LogService.SaveInfo("after method new Pasargad resbank:", "", resutVerfiy);

            #endregion
            payLog.Description = payLog.Description + Environment.NewLine + resutVerfiy.Message;
            if (resutVerfiy.ActionExecuteSucsess)
            {
                payLog.Status = PayLogType.veryFiy;
            }
            objectContext.SaveChanges();
            if (!resutVerfiy.ActionExecuteSucsess)
            {
                ViewBag.messagePay = resutVerfiy.Message;
                return RedirectToAction("ShowMessage", "ShowContent", new
                {
                    message = resutVerfiy.Message
                });
            }

            if (payLog.FkInvoiceNavigation == null)
            {
                #region finacialDocument
                // _accountingService.PaymentUser(SessionUserContract.Id, payLog.Id, payLog.Amount);
                #endregion
                throw new ExceptionForDisplay("پیش فاکتوری برای پرداخت یافت نشد");
            }
            else
            {
                _LogService.SaveInfo("befor method checkBalanceWithDiscount", "", "");
                _LogService.SaveInfo("befor method checkBalanceWithDiscount", "", resutVerfiy.ResultObject.VerifyResultData);
                checkBalanceWithDiscount((int)payLog.UserId, payLog.FkInvoiceNavigation.PaymentToCountinue, long.Parse(resutVerfiy.ResultObject.VerifyResultData.Amount));
                _LogService.SaveInfo("after method checkBalanceWithDiscount:", "", "");
                #region finacialDocument
                _accountingService.PaymentUserForInvoice((int)payLog.UserId, payLog.Id, payLog.Amount);
                _LogService.SaveInfo("after method PaymentUserForInvoice:", "", "");
                #endregion

                var invoivce = _service.FirstOrDefault(i => i.Id == payLog.InvoiceId);
                _service.ChangeStatusInvoice(invoivce, InvoiceStatus.payment);
                _LogService.SaveInfo("after method ChangeStatusInvoice:", "", "");
                _service.SaveAllChengeOrAllReject(true);
                _LogService.SaveInfo("after method SaveAllChengeOrAllReject:", "", "");

                //try
                //{
                //    updateInvoiceUserAndSesstion(SessionUserContract); // to do 

                //    _messageService.SendPaymentRef(SessionUserContract.Mobile, invoivce.Id.ToString());//to do delete from
                //}
                //catch (Exception)
                //{
                //}  SuccessPayment(string message, string invoivceId)

                return RedirectToAction("SuccessPayment", "ShowContent", new
                {
                    message = string.Concat("با تشکر از خرید شما، سفارش ", payLog.InvoiceId, " با موفقیت پرداخت شد"
                    , Environment.NewLine, ":شماره پی گیری ", result.VerifyResultData.RetrivalRefNo) ,
                    invoivceId = payLog.InvoiceId
                });
            }
        }
        #endregion


        private void checkBalanceWithDiscount(decimal price)
        {
            decimal balance = _accountingService.AccountBalance(SessionUserContract.Id);

            if (balance < price)
                throw new ExceptionForDisplay("اعتبار شما برای خرید سفارش کافی نمی باشد");
        }
        private void checkBalanceWithDiscount(int userId, decimal price, decimal newPayed)
        {
            decimal balance = _accountingService.AccountBalance(userId) + newPayed;

            if (balance < price)
                throw new ExceptionForDisplay("اعتبار شما برای خرید سفارش کافی نمی باشد");
        }
        private PaymentLog checkPaymentStatus(long paymentLogId)
        {
            throw new NotImplementedException();
        }



        #endregion Payment Pasargad

        #region Shipping        
        /// <summary>
        /// محاسبه ای جک سی هزینه حمل  
        /// </summary>
        /// <param name="id">آی دی استان</param>
        /// <param name="value2">آی دی فاکتور</param>
        /// <returns></returns>
        public decimal CalcShipping(int id, int value2)
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return -1;
            decimal moneyShiping;


            moneyShiping = _service.calcShipping(id, value2);

            return moneyShiping;
        }

        /// <summary>
        /// محاسبه ای جک سی هزینه حمل  
        /// </summary>
        /// <param name="id">آی دی استان</param>
        /// <param name="value2">آی دی فروشنده</param>
        /// <returns></returns>
        public decimal CalcShippingByBusinessOwnerId(int id, int value2)
        {
            decimal moneyShiping;
            int FK_BusinessOwner = value2;

            Dictionary<int, int> Products = CookieInvoiceToDictionary();
            ProductService productService = new ProductService(objectContext);

            _service.DictionaryToInvoice(FK_BusinessOwner, Products, ref _entity);
            moneyShiping = _service.calcShipping(id, _entity);

            return moneyShiping;
        }
        #endregion Shipping




        #region CommentRgionn
        //[HttpGet]
        //   [Route("checkout1")]
        //public ActionResult PaymentByCredit(long invoiceId, string address, string cityName, string noteForBussiness, int fkProvince,
        //    string name, string familyName, string company, string mobile, string tel, string postCode)
        //{
        //    if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User");
        //    var invoice = _service.GetInvoice(invoiceId, SessionUserContract.Id);
        //    if (invoice == null) throw new Exception("کد در خواست درست نمی باشد");

        //    _service.PayByCredit(invoice.Id);
        //    SMSkavenegar.SendOneSMS("buy-PayByCredit", SessionUserContract.Mobile, invoice.Id.ToString());
        //    checkBalanceWithDiscount(invoice.PaymentToCountinue);
        //    return RedirectToAction("ShowMessage", "ShowContent", new
        //    {
        //        message = string.Concat("با تشکر از خرید شما، سفارش ", invoice.Id, "با استفاده از اعتبار شما پرداخت شد")
        //    });
        //}
        #endregion





    }
}
