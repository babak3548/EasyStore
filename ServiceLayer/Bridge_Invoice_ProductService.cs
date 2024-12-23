using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;
using Utility;
using DataLayer.EF;
namespace ServiceLayer
{
    public partial class Bridge_Invoice_ProductService : BaseService<BridgeInvoiceProduct>
    {
        //AccountingService _accountingService;
  
        public Bridge_Invoice_ProductService(OnlineShopping OnlineShopping)
            : base(OnlineShopping)
        {
            
        }

        public decimal  CalcMoneySumARow(decimal price, int count)
        {
            return price * count;
        }
        /// <summary>
        /// مبلغ یک سفارش را بدون در نظرگرفتن هزینه حمل و تخفیف محاسبه می نمایید
        /// </summary>
        /// <param name="InvoiceId">کد فاکتور</param>
        /// <returns></returns>
        public decimal TotalMoneySumByInvoiceId(int InvoiceId)
        {
            //decimal TotalMoneySum = 0;
            
            //foreach (var B_I_P in Find(b=>b.FkInvoice==InvoiceId))
            //{
            //    //اگر تعداد کالای درخواستی مشتری بیشتر یا مساوی  تعداد فروش عمده باشد  قیمت کالا به قیمت عمده حساب می گردد 
            //    var price = (decimal)(B_I_P.Count >= B_I_P.Product.MinCountForPrice.ToInteger(int.MaxValue) ? B_I_P.Product.CountPrice : B_I_P.Product.Price);
            //    TotalMoneySum += CalcMoneySumARow( price, B_I_P.Count);
            //}
            //return TotalMoneySum;
           return TotalMoneySumByInvoiceId(Find(b => b.FkInvoice == InvoiceId));
        }


        public decimal TotalMoneySumByInvoiceId(IEnumerable<BridgeInvoiceProduct> listB_I_P)
        {
            decimal TotalMoneySum = 0;

            foreach (var B_I_P in listB_I_P)
            {
                //اگر تعداد کالای درخواستی مشتری بیشتر یا مساوی  تعداد فروش عمده باشد  قیمت کالا به قیمت عمده حساب می گردد 
                var price = (decimal)(B_I_P.Count >= (B_I_P.FkProductNavigation.MinCountForPrice ?? int.MaxValue) && B_I_P.FkProductNavigation.CountPrice != null ? B_I_P.FkProductNavigation.CountPrice : B_I_P.FkProductNavigation.Price);
                TotalMoneySum += CalcMoneySumARow(price, B_I_P.Count);
            }
            return TotalMoneySum;
        }
        public decimal CalcMarketingByInvoiceId(int InvoiceId)
        {
            var _accountingService = new AccountingService(_OnlineShopping);
            decimal TotalMarketingSum = 0;
            foreach (var bridge_Invoice_Product in Find(b => b.FkInvoice == InvoiceId))
            {
                TotalMarketingSum += _accountingService.CalculaterMakrketing( CalcMoneySumARow(bridge_Invoice_Product.FkProductNavigation.Price, bridge_Invoice_Product.Count)
                    ,bridge_Invoice_Product.FkProductNavigation.PersentMarkater,bridge_Invoice_Product.FkProductNavigation.PersentMarkater) ;
            }
            return TotalMarketingSum;
        }

        ///برای ولیدیت کردن صاحب انتیتی مورد استفاده قرار می گیردو متد های کلاس پدر را هر کلاس باید اور راید می کند
        #region ValidationOwnerEntity
        public override bool CheckOwnerEntity(int Id, int FkUser, out BridgeInvoiceProduct bridge_Invoice_Product)
        {
            bridge_Invoice_Product = FirstOrDefault(p => p.Id == Id);
            if (bridge_Invoice_Product != null && bridge_Invoice_Product.FkInvoiceNavigation.FkUser == FkUser)
                return true;
            else
                return false;
        }
        /// <summary>
        /// شرط بررسی صاحب انتیتی  بودن
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="FkUser"></param>
        /// <returns></returns>
        public override bool CheckOwnerEntity(BridgeInvoiceProduct entity, int FkUser)
        {
            if (entity != null && entity.FkInvoiceNavigation.FkUser == FkUser)
                return true;
            else
                return false;
        }
        #endregion ValidationOwnerEntity
    }
}
