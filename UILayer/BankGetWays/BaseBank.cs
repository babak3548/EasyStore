using DataLayer.EF;
using DataLayer.Enums;
using DataLayer.Models;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
//using System.Web.Mvc;

namespace UILayer.BankGetWays
{
    public abstract class BaseBank
    {
        public Banks BankCode { get; set; }
        public string BankName { get; set; }
        protected decimal Amount { get; set; }
       
        public long UserId { get; set; }
      
        protected string Description { get; set; }

        public OnlineShopping ObjectContext { get; set; }
        public InvoiceService invoiceService { get; set; }
       

        // protected PaymentLog  PaymentLog{ get; set; }
        //  protected long? OrdeId { get; set; }
        //   protected long? FinancialDocumentId { get; set; }//فقط برای شارژ حساب استفاده میش ود

  
       
        protected BaseBank( decimal amount, OnlineShopping objectContext,long userId,string description)
        {
            Amount = amount;
            ObjectContext = objectContext;
            UserId = userId;
            Description = description;
         
        }



       // public abstract Result<PasargadData> GetTokenPay();


        //وضیعت مشخص است برای چک اردن نهایی از این متد استفاده میگردد
        //  استعلام نتیجه
        public abstract Result<PaymentLog> Veryfiy(PaymentLog paymentLog, object paymentDate,  object TransactionReferenceID);

//        • InvoiceNumber) در فيلد iN(
//• InvoiceDate) در فيلد iD(
//• TransactionReferenceID) در فيلد tref(
        //وضیعت  پرداخت نامشخص باشد برای چک کردن پرداخت مشتری بررسی میگردد
        //    public abstract bool PaymentResultNoDetermin(Invoice inovice);
        public abstract Result<bool> Refund(long paymentLogId);//ActionResult
        protected Result<PaymentLog> UpdatePaymentAfterVeryfiyBankPasargad(PaymentLog paymentLog, string transactionReferenceID, 
             string message, bool veryfiyBool , string traceNumber , string referenceNumber)
        {
            if (veryfiyBool)
            {
                paymentLog.Status = PayLogType.veryFiy;
                paymentLog.Description = string.Concat("tranf:", transactionReferenceID,
                    " traceNumber:", traceNumber, "refRef", referenceNumber);
                if (ObjectContext.SaveChanges() <= 0)
                {
                    throw new Exception(string.Concat("خطای خطر ناک پرداخت به روز رسانی نشد--Veryfiy ", "paymentId:", paymentLog.Id.ToString(), "TransactionReferenceID : ", transactionReferenceID));
                }
                return Result<PaymentLog>.Sucsess(paymentLog);
            }
            else
            {
                message = " کاربر گرامی عملیات پرداخت از طرف بانک تایید نشد و اگر مبلغی از حساب شما کم شده باشد به زودی به حسابتان برمی گردد ";
                paymentLog.Status = PayLogType.noVeryFiy;
                string description = string.Concat("fail==>tranf:", transactionReferenceID,
                    " trceNum:", traceNumber, " refRef:", referenceNumber);
                paymentLog.Description = description;
                if (ObjectContext.SaveChanges() <= 0)
                {
                    throw new Exception(string.Concat("خطای خطر ناک پرداخت به روز رسانی نشد--Veryfiy ", "paymentId:", paymentLog.Id.ToString(), description));
                }
                return Result<PaymentLog>.Fail(paymentLog, message);
            }
        }

        public void sendMsgPayToBusOwn()
        {
            //MessageService messageService = new MessageService(objectContext);
            //string strBody = "<span>با سلام</span><br /><b>از فروشگاه شما خریدی انجام شد</b><br />"
            //    + " <span>لطفا برای بررسی و ارسال کالا به سایت مراجعه فرمایید</span><br />"
            //    + "<span>تاریخ خرید:" + UIUtility.CurrentTime + " " + UIUtility.CurrentDate + "</span><br />";
            //messageService.SendMessageToEmail(invoice.BusinessOwner.User.Email, "از فروشگاه شما خریدی انجام شد", strBody);
            //messageService.SendMessageToMobile(invoice.BusinessOwner.User.Mobile, "در شعبه فروش از فروشگاه شما خریدی انجام شد");
        }

     

        //public abstract object PaymentReturn();
    }
}