using DataLayer;
using DataLayer.EF;
using DataLayer.Enums;
using DataLayer.Models;
using Microsoft.Extensions.Configuration;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace UILayer.BankGetWays
{
    public class Pasargad : BaseBank
    {

        private PasargadData pasargadData { get; set; }
        private resultObjPasargad ResultObjPasargad { get; set; }
        private Invoice invoice { get; set; }

        private const string PrivateKey = "";
        private const string PublicKey = "";


        private int terminalCode { get { return (int.Parse(ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "terminalCode"))); } }//شماره ترمینال
        private int merchantCode { get { return (int.Parse(ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "merchantCode"))); } }//شماره فروشگاه
        private string PaymentAction { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "PaymentAction"); } }//شماره فروشگاه
        private string returnAction { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "returnAction"); } }//شماره فروشگاه151.241.191.88


        string CheckTransactionResultUrl { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "CheckTransactionResult"); } }
        string VerifyPaymentFinalUrl { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "VerifyPaymentPasargad"); } }
        //   static string VerifyPaymentFinalUrl { get { return WebConfigurationManager.AppSettings["VerifyPaymentPasargad"]; } }  gateway
        string DoRefundUrl { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "DoRefund"); } }
       public static string gatewayUrl { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "gateway"); } }
        public string redirectAddressPasargadToShobe { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "redirectAddressPasargadToShobe"); } }

        //public static string gatewayUrl { get { return WebConfigurationManager.AppSettings["gateway"]; } }
        //آدرس برگشت به سرور در  حالت دیباگ
        // private const string redirectAddress = "http://localhost:8484/Pasargad/RedirectAddress";

        // InvoiceService _invoiceService ;
      //  public Banks BankCode { get; set; }
      //  public string BankName { get; set; }

      // public OnlineShopping ObjectContext { get; set; }
        public InvoiceService invoiceService { get; set; }
       // public long  CurrentUserId { get; set; }

        // InvoiceService _invoiceService ;
        public Pasargad(decimal amount, OnlineShopping ObjectContext, long userId, string description)
            : base(amount, ObjectContext, userId, description)
        {
            //   _invoiceService = new InvoiceService(objectContext);
            BankCode = Banks.Pasargad;
            BankName = Banks.Pasargad.ToString();
            pasargadData = new PasargadData();
        }
    

        public Result<PasargadData> GetTokenPay(PaymentLog payment  )
        {
            
            //دادهای بانک مقصد
            var data = BankData(payment);
            return Result<PasargadData>.Sucsess(data);
        }
        //public abstract Result<Boolean> Veryfiy //(string iN, string iD, string tref)
        public override Result<PaymentLog> Veryfiy(PaymentLog paymentLog, object paymentDate, object TransactionReferenceID)
        {
            if (paymentLog == null) throw new Exception(string.Concat("خطای خطر ناک پرداخت ثبت نشده بوده--Veryfiy ", "paymentId:", paymentLog.Id.ToString(), "TransactionReferenceID : ", TransactionReferenceID.ToString()));

            paymentLog.UpdateDate = DateTime.Now;

            string message = "";
            
            //اگر فاکتور ارسالی بانک وجود داشت صحت وایز از بانک استعلام می گرددو گرنه خطا می دهد و از کاربر درخواست پرداخت از یک بانک دیگر را می دهد
            bool veryfiyBool = callAndVeryfiyWithServerPassargad(TransactionReferenceID, paymentLog);
          
            var updateResult= UpdatePaymentAfterVeryfiyBankPasargad(paymentLog, TransactionReferenceID.ToString(),  
                message, veryfiyBool, ResultObjPasargad.traceNumber.ToString(), ResultObjPasargad.referenceNumber.ToString());

            ObjectContext.SaveChanges();

            return updateResult;
        }

        private string GeanreateSign()
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

        private PasargadData BankData(PaymentLog paymentLog)
        {
            pasargadData.merchantCode = merchantCode;
            pasargadData.terminalCode = terminalCode;
            pasargadData.amount = Amount;
            pasargadData.redirectAddress = redirectAddressPasargadToShobe;
            //میلادی
            pasargadData.timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");//ToString(UIUtility.CurrentDate + " " + DateTime.Now.TimeOfDay.ToString().Substring(0, 8));
            pasargadData.invoiceNumber = (int)paymentLog.Id;//شماره فاکتور 
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
        private bool callAndVeryfiyWithServerPassargad(object TransactionReferenceID, PaymentLog paymentLog)
        {
            string resultTransaction = "";
            resultTransaction = CallBankForVerfiPayment(TransactionReferenceID.ToString());
         //   Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(new Exception ("resultTransaction : " + resultTransaction)));

            ResultObjPasargad = ParsXML(resultTransaction);
            //چک کردن تاریحخ به دلیل  اینکه زمان ارسال تاریخ ثبت نمی شود  آن را چک نمی کنیم
            if (ResultObjPasargad.result == true && ResultObjPasargad.invoiceNumber == paymentLog.Id
                && ResultObjPasargad.merchantCode == merchantCode && ResultObjPasargad.terminalCode == terminalCode)
            {
               var finalVerfy= finalVerifyPayment(paymentLog);
                return finalVerfy;

            }
            else
            { return false; }

        }

        //public override bool PaymentResultNoDetermin()
        //{
        //    string resultTransaction = ReadPaymentResult();

        //    resultObjPasargad _resultObj = ParsXML(resultTransaction);
        //    //چک کردن تاریحخ به دلیل  اینکه زمان ارسال تاریخ ثبت نمی شود  آن را چک نمی کنیم
        //    if (_resultObj.result == true && _resultObj.invoiceNumber == _invoice.Id
        //      && _resultObj.merchantCode == merchantCode && _resultObj.terminalCode == terminalCode)
        //    { return true; }
        //    else
        //    { return false; }
        //}

        /// <summary>
        /// TransactionReferenceID متد  نهایی خواندن یا چک کردن وضیعت یک تراکنش با داشتن  
        /// </summary>
        /// <param name="TransactionReferenceID"></param>
        /// <returns></returns>
        private string CallBankForVerfiPayment(string TransactionReferenceID)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CheckTransactionResultUrl);
            string text = "invoiceUID=" + TransactionReferenceID;//Request.QueryString["tref"];
            byte[] textArray = Encoding.UTF8.GetBytes(text);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = textArray.Length;
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
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

        private bool finalVerifyPayment(PaymentLog paymentLog  )
        {
            string strPostVerify = finalVerifyPaymentPost(paymentLog);

            string message = "";
             bool resultVerify= false;

            using (XmlReader reader = XmlReader.Create(new StringReader(strPostVerify)))
            {
                var xdoc = XDocument.Load(reader);

                var nodeResultObj = xdoc.Descendants("actionResult").FirstOrDefault();
                resultVerify=bool.Parse( nodeResultObj.Element("result").Value);
                message = nodeResultObj.Element("resultMessage").Value;

            }
            if (!resultVerify) throw new Exception("خطایی بحرانی تایید پرداخت اتفاق افتاده حتما باید بررسی شود" + message);

            return resultVerify;
            //ميباشد XML شامل نتيجه به صورت Result در اين مرحله
        }

        private string finalVerifyPaymentPost(PaymentLog paymentLog)
        {
            //merchantCode =merchantCode ;// ; 115 کد پذيرنده
            //terminalCode =terminalCode; // ; 12کد ترمينال 
            //amount =invoice.PaymentToCountinue; // ; 2000000 مبلغ فاکتور
            //invoiceNumber = invoice.Id;// ; 1949945شماره فاکتور 
            //باید اصلاح شود
            string invoiceDate = paymentLog.CreateDate.ToString("yyyy/MM/dd HH:mm:ss");  // ; 12:45:32تاريخ فاکتور  باید 
            string action = returnAction; // برای درخواست برگشت خريد : 1004
            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateKey);
            string data = "#" + merchantCode + "#" + terminalCode + "#" +
             paymentLog.Id + "#" + invoiceDate + "#" + Amount + "#" +  timeStamp + "#";
            byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new
            SHA1CryptoServiceProvider());
            string sign = Convert.ToBase64String(signMain);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(VerifyPaymentFinalUrl);
            string text = "InvoiceNumber=" + paymentLog.Id + "&InvoiceDate=" +
            invoiceDate + "&MerchantCode=" + merchantCode + "&TerminalCode=" +
            terminalCode + "&Amount=" + Amount +  "&TimeStamp=" + timeStamp + "&Sign=" + sign;
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
        public override Result<bool> Refund(long paymentLogId)
        {
            PaymentLog paymentLog = null;

            if (paymentLogId != null)
                paymentLog = ObjectContext.PaymentLogs.FirstOrDefault(p => p.Id == paymentLogId);

            if (paymentLog == null)
                return Result<bool>.Fail(false, "رسید یا فاکتوری با شماره ارسالی شما یافت نشد");

            string resultTransaction = "";

            resultTransaction = DoRefund(paymentLog);

            resultObjPasargad _resultObj = ParsXML(resultTransaction);
            //چک کردن تاریحخ به دلیل  اینکه زمان ارسال تاریخ ثبت نمی شود  آن را چک نمی کنیم
            if (_resultObj.result == true && _resultObj.action == returnAction && _resultObj.invoiceNumber == paymentLog.Id
                && _resultObj.merchantCode == merchantCode && _resultObj.terminalCode == terminalCode)
            { return Result<bool>.Sucsess(true); }
            else
            { return Result<bool>.Fail(false, "عملیات پرداخت شما ناموفق بود اگر بانک مبلغی از حساب شما کم کرده باشد تا 24 ساعت آینده به حساب شما باز می گرداند برای پرداخت مجدد از قسمت سفارشات اقدام نمایید"); }
        }

        /// <summary>
        /// برگشت از خرید متد نهایی ارسال کنند درخواست به بانک
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns>نتیجه فایل ایکس ام ال به صورت استرینق</returns>
        private string DoRefund(PaymentLog paymentLog)
        {
            //merchantCode =merchantCode ;// ; 115 کد پذيرنده
            //terminalCode =terminalCode; // ; 12کد ترمينال 
            //amount =invoice.PaymentToCountinue; // ; 2000000 مبلغ فاکتور
            //invoiceNumber = invoice.Id;// ; 1949945شماره فاکتور 
            //باید اصلاح شود
            string invoiceDate = paymentLog.CreateDate.ToString("yyyy/MM/dd HH:mm:ss");  // ; 12:45:32تاريخ فاکتور  باید 
            string action = returnAction; // برای درخواست برگشت خريد : 1004
            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateKey);
            string data = "#" + merchantCode + "#" + terminalCode + "#" +
             paymentLog.Id + "#" + invoiceDate + "#" + Amount + "#" + action + "#" +
            timeStamp + "#";
            byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new
            SHA1CryptoServiceProvider());
            string sign = Convert.ToBase64String(signMain);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DoRefundUrl);
            string text = "InvoiceNumber=" + paymentLog.Id + "&InvoiceDate=" +
            invoiceDate + "&MerchantCode=" + merchantCode + "&TerminalCode=" +
            terminalCode + "&Amount=" + Amount + "&action=" + action + "&TimeStamp=" + timeStamp + "&Sign=" + sign;
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

                _resultObj.invoiceNumber =int.Parse( nodeResultObj.Element("invoiceNumber").Value);

                _resultObj.merchantCode =int.Parse( nodeResultObj.Element("merchantCode").Value);

                // _resultObj.referenceNumber = nodeResultObj.Element("referenceNumber").Value.ToInteger(0);

                _resultObj.result =bool.Parse( nodeResultObj.Element("result").Value);

                _resultObj.terminalCode =int.Parse( nodeResultObj.Element("terminalCode").Value);

                //_resultObj.traceNumber = nodeResultObj.Element("traceNumber").Value.ToInteger(0);

                // _resultObj.transactionDate = nodeResultObj.Element("transactionDate").Value;

                _resultObj.transactionReferenceID = nodeResultObj.Element("transactionReferenceID").Value;
            }

            return _resultObj;
        }




    }


}
