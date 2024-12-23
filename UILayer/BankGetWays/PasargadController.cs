using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Utility;
using DataLayer.Enums;
using UILayer.Miscellaneous;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;

using ServiceLayer;
using DataLayer.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using UILayer.Controllers;
using ServiceLayer.Maper;
using DataLayer.Contract;

namespace UILayer.BankGetWays
{
    public class PasargadController : Base0Controller
    {

        private PasargadData pasargadData { get; set; }
        private resultObjPasargad ResultObjPasargad { get; set; }
        private Invoice invoice { get; set; }

        private const string PrivateKey = "";
        private const string PublicKey = "";
                                          
       
        private int terminalCode { get { return (int.Parse( ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "terminalCode"))) ; } }//شماره ترمینال
        private int merchantCode { get { return (int.Parse(ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "merchantCode"))); } }//شماره فروشگاه
        private string PaymentAction { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "PaymentAction"); } }//شماره فروشگاه
        private string returnAction { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "returnAction");   } }//شماره فروشگاه151.241.191.88
    

        string CheckTransactionResultUrl { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "CheckTransactionResult");    } }
        string VerifyPaymentFinalUrl { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "VerifyPaymentPasargad");    } }
     //   static string VerifyPaymentFinalUrl { get { return WebConfigurationManager.AppSettings["VerifyPaymentPasargad"]; } }
        string DoRefundUrl { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "DoRefund") ; } }
       public static string gatewayUrl { get { throw new Exception("ادرس گیت وی را ست نمایید"); /*return ConfigurationExtensions.GetConnectionString(this.Configuration, "gateway");*/ } }
       public  string redirectAddressPasargadToShobe { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "redirectAddressPasargadToShobe"); } }
        //public static string gatewayUrl { get { return WebConfigurationManager.AppSettings["gateway"]; } }
        //آدرس برگشت به سرور در  حالت دیباگ
        // private const string redirectAddress = "http://localhost:8484/Pasargad/RedirectAddress";
        public Banks BankCode { get; set; }
        public string BankName { get; set; }

        public OnlineShopping ObjectContext { get; set; }

        InvoiceService _invoiceService ;
        public PasargadController(OnlineShopping _onlineShopping):base( "" ,  _onlineShopping)
       {
            ObjectContext = _onlineShopping;
            _invoiceService = new InvoiceService(ObjectContext);
            BankCode=Banks.Pasargad;
            BankName = Banks.Pasargad.ToString();
           
            pasargadData = new PasargadData();
        }


        public  ActionResult GetTokenPay(int invoiceId)
        {
            ValidateAccessToAction(RolesSystem.UserValue);
            //  FactoryBankObject FactoryBankObject = new FactoryBankObject(bankCode);
            ///   BaseBank baseBank = FactoryBankObject.BankObject;
            //  _entity.PaymentBankCode = baseBank.BankCode;
            //only test
          
            invoice = _invoiceService.FirstOrDefault(i => i.Id == invoiceId);
            //دادهای بانک مقصد
            ViewData["DataBank"] = BankData();
            return View(invoice);
            //@"~/Views/User/ToPayView"
            //  return RedirectToAction("ToPayView", "User", new { Id = invoice.Id, bankCode = invoice.PaymentBankCode });
            // throw new NotImplementedException();
        }

        public ActionResult RedirectAddress(string iN, string iD, string tref)
        {
            invoice = _invoiceService.FirstOrDefault(i => i.Id == iN.ToInteger(0));
            // چک وضعیت قبلی سفارش 

            //اگر فاکتور ارسالی بانک وجود داشت صحت وایز از بانک استعلام می گرددو گرنه خطا می دهد و از کاربر درخواست پرداخت از یک بانک دیگر را می دهد
            if (invoice != null && PaymentResultDetermin(tref))
            {
                invoice.RegisterDate = ResultObjPasargad.invoiceDate.Trim().Substring(0, 10);
                invoice.TimeBankPayInfo = ResultObjPasargad.invoiceDate.Trim().Substring(11, 8); //*&
                invoice.TransctionRefrenceId = tref;
                invoice.PaymentBankCode = Banks.Pasargad;

                _invoiceService.InvoiceGoToPaymentState(invoice, CurrentUserContract.Id, tref);
                sendMsgPayToBusOwn(invoice);
            //    try
            //    {
            //        var emailSender = new Email(Server.MapPath("~") + Paths.LogPath + Paths.ErrorLogFileName);
            //        emailSender.SendAEmail(invoice.User.Email, "DeliveryCode", "<html><body><span>" + "مشتری گرامی تحویل کد زیر به فروشنده به منزله تائید کالا تحویلی فروشنده می باشد کد:" + invoice.DeliveryCode +
            //"</span></b></body></html>");
            //        emailSender.SendAEmail(invoice.BusinessOwner.User.Email, "DeliveryCodedUnPerfect", "<html><body><span>" + "فروشنده گرامی جهت اطمینان از درست بودن کد تائید مشتری آن را با کد ناقص زیر مقایسه نمائید. کد: "
            //      + invoice.DeliveryCodedUnPerfect + "</span></b></body></html>");
            //    }
            //    catch (Exception)
            //    {
            //    }

                return RedirectToAction("ShowMessage", "ShowContent", new
                {
                    message = " مشتری گرامی عملیات پرداخت با موفقیت انجام شد ,هنگام دریافت سفارش کد زیر به فروشنده تحویل دهید. تحویل این کد، به" +
                        " منزله تائید کالاهای تحویلی فروشنده می باشد و فروشنده بااین کد مبلغ پرداختی شما را دریافت می کند"+
                        " اگر سفارش شمااز طریق شرکت های حمل نقل ارسال شده باشد بعد از دریافت، از قسمت سفارشات تحویل سفارش مورد نظر را تایید فرمایید  کد:" + invoice.DeliveryCode
                });
            }
            else
                return RedirectToAction("ShowMessage", "ShowContent", new
                {
                    message = " مشتری گرامی عملیات پرداخت با شکست مواجه شد می توانید درگاه پرداخت  بانکهای دیگر را امتحان نمایید  "
                });
        }

        private  string GeanreateSign()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateKey);
            //کلید خصوصی فروشنده

            string data = "#" + pasargadData.merchantCode + "#" + pasargadData.terminalCode + "#" + pasargadData.invoiceNumber + "#"
                + pasargadData.invoiceDate + "#" + pasargadData.amount + "#" + pasargadData.redirectAddress
            + "#" + pasargadData.action + "#" + pasargadData.timeStamp + "#";

            byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new SHA1CryptoServiceProvider());
            string sign = Convert.ToBase64String(signMain);
            return sign;

        }

        private PasargadData BankData()
        {
            pasargadData.merchantCode = merchantCode;
            pasargadData.terminalCode = terminalCode;
            pasargadData.amount = invoice.PaymentToCountinue.ToDecimal(0);
            pasargadData.redirectAddress = redirectAddressPasargadToShobe;
            //میلادی
            pasargadData.timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");//ToString(UIUtility.CurrentDate + " " + DateTime.Now.TimeOfDay.ToString().Substring(0, 8));
            pasargadData.invoiceNumber = invoice.Id;//شماره فاکتور 
            //شمسی
            pasargadData.invoiceDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); //تاريخ فاکتور  
            pasargadData.action = PaymentAction;

            pasargadData.sign = GeanreateSign();


            return pasargadData;

        }

        #region Payment
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TransactionReferenceID"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
          public  bool PaymentResultDetermin(object TransactionReferenceID)
        {
            string resultTransaction = "";
            resultTransaction = ReadPaymentResult(TransactionReferenceID.ToString());

            ResultObjPasargad = ParsXML(resultTransaction);
            //چک کردن تاریحخ به دلیل  اینکه زمان ارسال تاریخ ثبت نمی شود  آن را چک نمی کنیم
            if (ResultObjPasargad.result == true && ResultObjPasargad.invoiceNumber == invoice.Id && ResultObjPasargad.merchantCode == merchantCode && ResultObjPasargad.terminalCode == terminalCode)
            {
		        var finalVerfy= finalVerifyPayment(paymentLog);
                return finalVerfy;
			 }
            else
            { return false; }

            //  throw new NotImplementedException();
        }

      //    public override bool PaymentResultNoDetermin(Invoice _invoice)
       //   {
       //       string resultTransaction = ReadPaymentResult(_invoice.RegisterDate, _invoice.Id);

        //      resultObjPasargad _resultObj = ParsXML(resultTransaction);
         //     //چک کردن تاریحخ به دلیل  اینکه زمان ارسال تاریخ ثبت نمی شود  آن را چک نمی کنیم
        //      if (_resultObj.result == true && _resultObj.invoiceNumber == _invoice.Id && _resultObj.merchantCode == merchantCode && _resultObj.terminalCode == terminalCode)
      //        { return true; }
       //       else
      //        { return false; }
     //     }

        /// <summary>
        /// TransactionReferenceID متد  نهایی خواندن یا چک کردن وضیعت یک تراکنش با داشتن  
        /// </summary>
        /// <param name="TransactionReferenceID"></param>
        /// <returns></returns>
        private string ReadPaymentResult(string TransactionReferenceID)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CheckTransactionResultUrl);
            string text = "invoiceUID=" + TransactionReferenceID;//Request.QueryString["tref"];
            byte[] textArray = Encoding.UTF8.GetBytes(text);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = textArray.Length;
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
                //delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                //new RemoteCertificateValidationCallback(RemoteCertificateValidation);
            //برای اطمينان حاصل آردن از اينكه مشتری داده های خود را به سايت بانك ارسال//
            //دريافت شده فقط و فقط از جانب سايت بانك می باشد. response آرده و
            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();
            return result;
            //ميباشد XML شامل نتيجه به صورت Result در اين مرحله
        }
        private bool finalVerifyPayment(Invoice invoice  )
        {
            string strPostVerify = finalVerifyPaymentPost(invoice);

            string message = "";
             bool resultVerify= false;

            using (XmlReader reader = XmlReader.Create(new StringReader(strPostVerify)))
            {
                var xdoc = XDocument.Load(reader);

                var nodeResultObj = xdoc.Descendants("actionResult").FirstOrDefault();
                resultVerify= nodeResultObj.Element("result").Value.ToBoolean(false);
                message = nodeResultObj.Element("resultMessage").Value;

            }
            if (!resultVerify) throw new Exception("خطایی بحرانی تایید پرداخت اتفاق افتاده حتما برای بررسی شود" + message);

            return resultVerify;
            //ميباشد XML شامل نتيجه به صورت Result در اين مرحله
        }

        private string finalVerifyPaymentPost(Invoice invoice)
        {
            //merchantCode =merchantCode ;// ; 115 کد پذيرنده
            //terminalCode =terminalCode; // ; 12کد ترمينال 
            //amount =invoice.PaymentToCountinue; // ; 2000000 مبلغ فاکتور
            //invoiceNumber = invoice.Id;// ; 1949945شماره فاکتور 
            //باید اصلاح شود
            string invoiceDate = invoice.RegisterDate.ToString("yyyy/MM/dd HH:mm:ss");  // ; 12:45:32تاريخ فاکتور  باید 
            string action = returnAction; // برای درخواست برگشت خريد : 1004
            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateKey);
            string data = "#" + merchantCode + "#" + terminalCode + "#" +
             invoice.Id + "#" + invoiceDate + "#" + invoice.PaymentToCountinue + "#" +  timeStamp + "#";
            byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new
            SHA1CryptoServiceProvider());
            string sign = Convert.ToBase64String(signMain);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(VerifyPaymentFinalUrl);
            string text = "InvoiceNumber=" + invoice.Id + "&InvoiceDate=" +
            invoiceDate + "&MerchantCode=" + merchantCode + "&TerminalCode=" +
            terminalCode + "&Amount=" + invoice.PaymentToCountinue +  "&TimeStamp=" + timeStamp + "&Sign=" + sign;
            byte[] textArray = Encoding.UTF8.GetBytes(text);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = textArray.Length;
            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();
            //يباشدم XML شامل نتيجه برگشت خريد به صورت Result
            return result;
        }

        /// <summary>
        ///TransactionReferenceID متد  نهایی خواندن یا چک کردن وضیعت یک تراکنش بدون داشتن  
        /// </summary>
        /// <param name="InvoiceDate"></param>
        /// <param name="InvoiceNumber"></param>
        /// <param name="terminalCode"></param>
        /// <param name="merchantCode"></param>
        /// <returns></returns>
        private string ReadPaymentResult(string InvoiceDate, int InvoiceNumber)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CheckTransactionResultUrl);
            string text = "InvoiceNumber=" + InvoiceNumber + "&InvoiceDate=" +
            InvoiceDate + "&MerchantCode=" + merchantCode + "&TerminalCode=" +
            terminalCode; //Request.QueryString["tref"];
            byte[] textArray = Encoding.UTF8.GetBytes(text);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = textArray.Length;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidation);
            //برای اطمينان حاصل آردن از اينكه مشتری داده های خود را به سايت بانك ارسال//
            //دريافت شده فقط و فقط از جانب سايت بانك می باشد. response آرده و
            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();
            return result;
            //ميباشد XML شامل نتيجه به صورت Result در اين مرحله

        }

        //را در False يا True آه در زير آمده است مقدار RemoteCertificateValidation تابع
        //certificate برمی گرداند. اگر خطايی در ارتباط با SSl Certificate نتيجه ی چك آردن
        //برگردانده می شود. False در غير اينصورت ،True وجود نداشته باشد
        private static bool RemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            return false;
        }

        #endregion Payment


        #region ReturnPayment

        /// <summary>
        /// عمل برگشت از خرید را انجام، ونتیجه بر می گرداند
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns>نتیجه موفق ترو ناموفق فالس</returns>
          public  bool Refund()  //PaymentReturnResult
        {
            if (invoice == null)
                throw  new MyException((byte) ExceptionType.Payment, "Refund payment", "چنین پرداختی در سیستم ثبت نشده است");
            string resultTransaction = "";

            resultTransaction = DoRefund(invoice);

            resultObjPasargad _resultObj = ParsXML(resultTransaction);
            //چک کردن تاریحخ به دلیل  اینکه زمان ارسال تاریخ ثبت نمی شود  آن را چک نمی کنیم
            if (_resultObj.result == true && _resultObj.action == returnAction && _resultObj.invoiceNumber == invoice.Id && _resultObj.merchantCode == merchantCode && _resultObj.terminalCode == terminalCode)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        /// برگشت از خرید متد نهایی ارسال کنند درخواست به بانک
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns>نتیجه فایل ایکس ام ال به صورت استرینق</returns>
        private string DoRefund(Invoice invoice)
        {
            //merchantCode =merchantCode ;// ; 115 کد پذيرنده
            //terminalCode =terminalCode; // ; 12کد ترمينال 
            //amount =invoice.PaymentToCountinue; // ; 2000000 مبلغ فاکتور
            //invoiceNumber = invoice.Id;// ; 1949945شماره فاکتور 
            //باید اصلاح شود
            string invoiceDate = invoice.RegisterDate.ToString("yyyy/MM/dd HH:mm:ss");  // ; 12:45:32تاريخ فاکتور  باید 
            string action = returnAction; // برای درخواست برگشت خريد : 1004
            string timeStamp =DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateKey);
            string data = "#" + merchantCode + "#" + terminalCode + "#" +
             invoice.Id + "#" + invoiceDate + "#" + invoice.PaymentToCountinue + "#" + action + "#" +
            timeStamp + "#";
            byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new
            SHA1CryptoServiceProvider());
            string sign = Convert.ToBase64String(signMain);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DoRefundUrl);
            string text = "InvoiceNumber=" + invoice.Id + "&InvoiceDate=" +
            invoiceDate + "&MerchantCode=" + merchantCode + "&TerminalCode=" +
            terminalCode + "&Amount=" + invoice.PaymentToCountinue + "&action=" + action + "&TimeStamp=" + timeStamp + "&Sign=" + sign;
            byte[] textArray = Encoding.UTF8.GetBytes(text);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = textArray.Length;
            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();
            //يباشدم XML شامل نتيجه برگشت خريد به صورت Result
            return result;
        }

        #endregion ReturnPayment

        private resultObjPasargad ParsXML(string xmlString)
        {
            resultObjPasargad _resultObj = new resultObjPasargad();

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                var xdoc = XDocument.Load(reader);

                var nodeResultObj = xdoc.Descendants("resultObj").FirstOrDefault();

                _resultObj.action = nodeResultObj.Element("action").Value;

                _resultObj.invoiceDate = nodeResultObj.Element("invoiceDate").Value;

                _resultObj.invoiceNumber = nodeResultObj.Element("invoiceNumber").Value.ToInteger(0);

                _resultObj.merchantCode = nodeResultObj.Element("merchantCode").Value.ToInteger(0);

             //   _resultObj.referenceNumber = nodeResultObj.Element("referenceNumber").Value.ToInteger(0);

                _resultObj.result = nodeResultObj.Element("result").Value.ToBoolean(false);

                _resultObj.terminalCode = nodeResultObj.Element("terminalCode").Value.ToInteger(0);

              //  _resultObj.traceNumber = nodeResultObj.Element("traceNumber").Value.ToInteger(0);

             //   _resultObj.transactionDate = nodeResultObj.Element("transactionDate").Value;

                _resultObj.transactionReferenceID =int.Parse( nodeResultObj.Element("transactionReferenceID").Value);
            }

            return _resultObj;
        }




    }
}