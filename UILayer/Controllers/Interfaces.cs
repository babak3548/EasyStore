using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;

using UILayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace UILayer.Controllers
{
    /// <summary>
    /// سند
    /// </summary>
    interface IDocumentActions
    {

         ActionResult MyAccount();

        /// <summary>
        /// در خواست انتقال وحه
        /// </summary>
        /// <param name="FK_Invoice"></param>
        /// <returns></returns>
         ActionResult RequestShippingMoneyInvoice(decimal MyRequestMony, string BankAccountNumber);

        /// <summary>
        /// تایید انتقال وجه
        /// </summary>
        /// <param name="Id">ای دی فاکتور</param>
        /// <param name="ActivateCode"></param>
        /// <returns></returns>
         ActionResult AcceptShippingMonyInvoice(int Id, string ActivateCode);

         ActionResult ShippingMonyToBankAccountUsers(int NumberShippingValue);

    }

        // عملیات مخصوص اینویس که باعٍث تغییر مراحل فاکتور می گرددند
        public interface IInvoiceStateScenario
        {
            ActionResult initializeScenario(int ProductId, int FK_BusinessOwner, int? FK_Marketer = 0);
            ActionResult DeterminScenario(int Id, string DeliveryAddress, short bankCode, string NoteForBusinessOwner, short? ShippingCompany, short? TypeSel, int? FK_Province, int? FK_Marketer);
            //, decimal PaymentToCountinue
            ActionResult RequestScenario(int Id, string DeliveryAddress, decimal Discount, string NoteForBusinessOwner, int FK_Province, int? FK_Marketer);
            ActionResult BusinessOwnerAcceptScenario(int Id, decimal Discount, decimal PaymentToCountinue, string NoteForUser);
            ActionResult ToPayScenario(int Id, string DeliveryAddress, short bankCode, string NoteForBusinessOwner, int? FK_Province, int? FK_Marketer);
            ActionResult paymentScenario(int Id);
            ActionResult SendScenario(int Id,  string ShippingNumber);
            // عمل تایید توسط فروشنده با کد تحویل صورت می گیرد
            ActionResult DeliveredScenario(int Id, string DeliveryCode);
            // عمل تایید تحویل توسط خریدار صورت می گیرد
            ActionResult FinalFromSendScenario(int Id, short VoteBusinessOwner, string CommentForBusinessman, short VoteMarketer = -1, string CommentForMarketer = "");

            ActionResult FinalScenario(int Id, short VoteBusinessOwner, string CommentForBusinessman, short VoteMarketer = -1, string CommentForMarketer = "");
            ActionResult canceledScenario(int Id);

        }

        //لیست سفارشات را در هر مرحله نمایش می دهد
        public interface IInvoiceState
        {
            ActionResult Invoices();
            ActionResult AInvoice();
            ActionResult initialize();
            ActionResult request();
            ActionResult BusinessOwnerAccept();
            ActionResult ToPay();
            ActionResult payment();
            ActionResult Send();
            ActionResult Delivered();
            ActionResult Final();
            ActionResult canceled();
        }
    //ویو یک فاکتور را 
        public interface IInvoiceStateView
        {
            ActionResult AInvoiceView(int Id);
            ActionResult DeterminView(int Id);
            ActionResult RequestView(int Id);
            ActionResult BusinessOwnerAcceptView(int Id);
            ActionResult ToPayView(int Id);
            ActionResult paymentView(int Id);
            ActionResult SendView(int Id);
            ActionResult DeliveredView(int Id);
            ActionResult FinalView(int Id);
            ActionResult CanceleView(int Id);
        }

        public interface ICURDOperation<TEntity>
        {
            ActionResult RegisterView(object obj=null);
            ActionResult Register(TEntity entity,object obj=null);
            ActionResult GridView();
            ActionResult Delete(int Id);
            ActionResult EditView(int Id);
            ActionResult Edit(TEntity entity);

        }

    
 

}
