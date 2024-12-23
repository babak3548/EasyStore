//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace ServiceLayer
//{
//    interface InterfaceFile
//    {
//    }


//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;
using DataLayer.EF;
namespace ServiceLayer
{
    interface IDocumentAccounting
    {
        
        /// <summary>
        /// محاسبه مبلغ بازاریابی
        /// </summary>
        /// <param name="MoneySum"></param>
        /// <param name="PersentTotalMarketing"></param>
        /// <param name="PersentSpecial"></param>
        /// <returns></returns>
         decimal CalculaterMakrketing(decimal MoneySum, float PersentTotalMarketing, float PersentSpecial);
         
        /// <summary>
        /// سند پرداخت 
        /// </summary>
        /// <param name="FK_User"></param>
        /// <param name="FK_Invoice"></param>
        /// <param name="Payment_User_Money"></param>
         void PaymentUser(int FK_User, long FK_Payment, decimal Payment_User_Money);

        /// <summary>
         /// سند بازاریاب ها بعد از تحویل سفارش
        /// </summary>
        /// <param name="invoice"></param>
         decimal AInvoiceMarketingDocument(Invoice invoice,int FK_User);

        /// <summary>
        /// سند بازاریاب ها یک مخصول در فاکتور
        /// </summary>
        /// <param name="bridge_Invoice_Product"></param>
        /// <param name="FK_Invoice"></param>
         decimal AProductMarketingDocument(BridgeInvoiceProduct bridge_Invoice_Product, int FK_Invoice);

        /// <summary>
        /// سند بازاریاب های پدر  
        /// </summary>
        /// <param name="bridge_Invoice_Product"></param>
        /// <param name="marketer"></param>
        /// <param name="MoneySum_bridge_Invoice_Product"></param>
        /// <param name="FK_Invoice"></param>
        /// <param name="m3"></param>
         void AProductMarketingParentsDocument(BridgeInvoiceProduct bridge_Invoice_Product, Marketer marketer, decimal MoneySum_bridge_Invoice_Product, int FK_Invoice, ref decimal m3);


        Accounting CreateDocument(string Name, decimal Creditor, decimal Debtor, DateTime date, long FkPaymentLog, int FkUser);


        /// <summary>
        /// مبلغ درخواستی کاربر را با موجودی کاربر چک می کند
        /// </summary>
        /// <param name="RequestMoney"></param>
        /// <param name="FK_User"></param>
        /// <returns></returns>
         bool validateReuestMoney(decimal RequestMoney, int FK_User);

        /// <summary>
        /// مانده حساب کاربر را محاسبه می کند
        /// </summary>
        /// <param name="FK_User"></param>
        /// <returns></returns>
         decimal AccountBalance(int FK_User);

         void ShippingMoneyDocumentWithOutCostShipping(int FK_User, int FK_Invoice, decimal RequestMoney);

         void ShippingMoneyDocument(int FK_User, int FK_Invoice, decimal RequestMoney);

         void Delivery(int FK_User, int FK_UserBusinessOwner, int FK_Invoice, decimal Payment_ToContinue_Money);

         /// <summary>
         /// سند پرداخت هزینه ای آموزش توسط بازاریاب 
         /// </summary>
         /// <param name="FK_User"></param>
         /// <param name="FK_Invoice"></param>
         /// <param name="Payment_User_Money"></param>
         void PaymentEductionBusinessOwner(int FK_User_BusinessOwner, int FK_User_Marketer, int FK_Invoice, decimal Payment_User_Money);


    }


}
