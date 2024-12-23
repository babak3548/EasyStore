using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;
using DataLayer.Enums;

using Utility;
using DataLayer.EF;

namespace ServiceLayer
{
    public class AccountingService : BaseService<Accounting>, IDocumentAccounting
    {

        InvoiceService _invoiceService;
        Invoice _invoice = new Invoice();

        Bridge_Invoice_ProductService _bridge_Invoice_ProductService;

        MarketerService _marketerService;
        Accounting _entity = new Accounting();

        public AccountingService(OnlineShopping OnlineShopping)
            : base(OnlineShopping)
        {
            _invoiceService = new InvoiceService(OnlineShopping);
            _marketerService = new MarketerService(OnlineShopping);
            _bridge_Invoice_ProductService = new Bridge_Invoice_ProductService(OnlineShopping);
        }


        #region IDocumentAccounting
        /// <summary>
        /// محاسبه مبلغ بازاریابی
        /// </summary>
        /// <param name="MoneySum"></param>
        /// <param name="PersentTotalMarketing"></param>
        /// <param name="PersentSpecial"></param>
        /// <returns></returns>
        public decimal CalculaterMakrketing(decimal MoneySum, float PersentTotalMarketing, float PersentSpecial)
        {
            return (((MoneySum * (decimal)PersentTotalMarketing) / 100) * (decimal)PersentSpecial) / 100;
        }

        /// <summary>
        /// سند پرداخت 
        /// </summary>
        /// <param name="FkUser"></param>
        /// <param name="FkPaymentLog"></param>
        /// <param name="Payment_User_Money"></param>
        public void PaymentUser(int FkUser, long FkPaymentLog, decimal Payment_User_Money)
        {
            //سند پرداخت مشتری
            _entity = CreateDocument(AccountingTypes.Payment_User, Payment_User_Money, 0, DateTime.Now, FkPaymentLog, FkUser);

            //سند دریافت حساب بانکی سایت 
            CreateDocument(AccountingTypes.Payment_User, 0, Payment_User_Money, DateTime.Now, FkPaymentLog, AccountingTypes.Bank_Account_Site_Value);
            SaveAllChengeOrAllReject(true);

        }
        public void PaymentUserForInvoice(int FkUser, long FkPaymentLog, decimal Payment_User_Money)
        {
            //سند پرداخت مشتری
            _entity = CreateDocument(AccountingTypes.Payment_User, Payment_User_Money, 0, DateTime.Now, FkPaymentLog, FkUser);

            //سند دریافت حساب بانکی سایت 
            CreateDocument(AccountingTypes.Payment_User, 0, Payment_User_Money, DateTime.Now, FkPaymentLog, AccountingTypes.Bank_Account_Site_Value);

            //سند خرید مشتری 
            CreateDocument(AccountingTypes.Buy_Invoice, 0, Payment_User_Money, DateTime.Now, FkPaymentLog, FkUser);
            SaveAllChengeOrAllReject(true);

        }


        /// <summary>
        /// سند بازاریاب ها بعد از تحویل سفارش
        /// </summary>
        /// <param name="invoice"></param>
        public decimal AInvoiceMarketingDocument(Invoice invoice, int FkUser)
        {
            decimal SumM = 0;
            foreach (var bridge_Invoice_Product in invoice.BridgeInvoiceProduct)
            {
                SumM += AProductMarketingDocument(bridge_Invoice_Product, invoice.Id);
            }
            return SumM;
        }

        /// <summary>
        /// سند بازاریابی یک محصول فاکتور
        /// </summary>
        /// <param name="bridge_Invoice_Product"></param>
        /// <param name="FkInvoice"></param>
        public decimal AProductMarketingDocument(BridgeInvoiceProduct bridge_Invoice_Product, int FkInvoice)
        {
            // decimal SumMarketingProduct=0;
            decimal m0 = 0, m1 = 0, m2 = 0, m3 = 0, RemainingM = 0, SumM = 0;
            //محاسبه مبلغ یک نوع محصول
            decimal MoneySum_bridge_Invoice_Product = _bridge_Invoice_ProductService.CalcMoneySumARow(bridge_Invoice_Product.FkProductNavigation.Price, bridge_Invoice_Product.Count);

            m0 = CalculaterMakrketing(MoneySum_bridge_Invoice_Product, bridge_Invoice_Product.FkProductNavigation.PersentMarkater, (short)MarketingPersent.Site);
            //سند بازاریابی سایت
            CreateDocument(AccountingTypes.Persent_Marketing_Site, m0, 0, DateTime.Now, FkInvoice, AccountingTypes.Persent_Marketing_Site_value);

            if (bridge_Invoice_Product.FkProductNavigation.FkBusinessOwnerNavigation.FkMarketer != null)
            {
                //سند بازاریابی معرفی کننده
                m1 = CalculaterMakrketing(MoneySum_bridge_Invoice_Product, bridge_Invoice_Product.FkProductNavigation.PersentMarkater, (short)MarketingPersent.Presenter);
                CreateDocument(AccountingTypes.Persent_Marketing_PresenterBusinessOwner, m1, 0, DateTime.Now, FkInvoice, bridge_Invoice_Product.FkProductNavigation.FkBusinessOwnerNavigation.FkMarketerNavigation.FkUser);
            }

            if (bridge_Invoice_Product.FkInvoiceNavigation.FkMarketer != null)
            {
                m2 = CalculaterMakrketing(MoneySum_bridge_Invoice_Product, bridge_Invoice_Product.FkProductNavigation.PersentMarkater, (short)MarketingPersent.seller);
                //سند بازاریابی  ،بازاریاب فروشنده
                CreateDocument(AccountingTypes.Persent_Marketing_seller, m2, 0, DateTime.Now, FkInvoice, bridge_Invoice_Product.FkInvoiceNavigation.FkMarketerNavigation.FkUser);
            }

            if (bridge_Invoice_Product.FkInvoiceNavigation.FkMarketer != null)
            {
                AProductMarketingParentsDocument(bridge_Invoice_Product, bridge_Invoice_Product.FkInvoiceNavigation.FkMarketerNavigation, MoneySum_bridge_Invoice_Product, FkInvoice, ref m3);
            }

            // سند باقی مانده مبلغ بازاریابی
            SumM = CalculaterMakrketing(MoneySum_bridge_Invoice_Product, bridge_Invoice_Product.FkProductNavigation.PersentMarkater, 100);
            RemainingM = SumM - (m0 + m1 + m2 + m3);
            CreateDocument(AccountingTypes.Remaining_Marketing, RemainingM, 0, DateTime.Now, FkInvoice, AccountingTypes.Remaining_Marketing_value);

            //دادن حق بازاریابی توسط صاحب تجارت
            CreateDocument(AccountingTypes.GiveMarketingWithBusinessOwner, 0, SumM, DateTime.Now, FkInvoice, bridge_Invoice_Product.FkInvoiceNavigation.FkBusinessOwnerNavigation.FkUser.Value);

            return SumM;
        }

        /// <summary>
        /// سند بازاریاب های پدر  
        /// کل درصد بازاریابی پدر تقسیم بر دو شده به اولین پدر سند  زده می شود و این روال به ترتیب از پایین دبه بالا به پدران دیگر نیز سند می گردد 
        /// </summary>
        /// <param name="bridge_Invoice_Product"></param>
        /// <param name="marketer"></param>
        /// <param name="MoneySum_bridge_Invoice_Product"></param>
        /// <param name="FkInvoice"></param>
        /// <param name="m3"></param>
        public void AProductMarketingParentsDocument(BridgeInvoiceProduct bridge_Invoice_Product, Marketer marketer, decimal MoneySum_bridge_Invoice_Product, int FkInvoice, ref decimal m3)
        {
            decimal m4 = 0;
            float ParentMarketing = (float)MarketingPersent.ParentMarketing / 2;
            m4 = CalculaterMakrketing(MoneySum_bridge_Invoice_Product, bridge_Invoice_Product.FkProductNavigation.PersentMarkater, ParentMarketing);

            //تازمانی ادامه دارد که بازاریاب پدری موجود نباشد یا مبلغ سند به حد اقل رسیده باشد
            while (marketer.FkMarketer != null & m4 > (decimal)ConstSetting.MinMonyParentMarketing)
            {
                marketer = _marketerService.FirstOrDefault(m => m.Id == marketer.FkMarketer);
                //سند درصد بازاریابی پدر فروشنده بازاریاب
                CreateDocument(AccountingTypes.Persent_Marketing_Parent, m4, 0, DateTime.Now, FkInvoice, marketer.FkUser);

                m3 += m4;
                ParentMarketing /= 2;
                m4 = 0;
                m4 = CalculaterMakrketing(MoneySum_bridge_Invoice_Product, bridge_Invoice_Product.FkProductNavigation.PersentMarkater, ParentMarketing);
            }
        }

        /// <summary>
        /// سند تحویل کالا قسمت خریدار فروشنده
        /// </summary>
        /// <param name="FkUser"></param>
        /// <param name="BusinessOwnerFkUser"></param>
        /// <param name="FkInvoice"></param>
        /// <param name="Payment_ToContinue_Money"></param>
        public void Delivery(int FkUser, int BusinessOwnerFkUser, int FkInvoice, decimal Payment_ToContinue_Money)
        {
            //سند تحویل سفارش
            _entity = CreateDocument(AccountingTypes.Buy_Invoice, Payment_ToContinue_Money, 0, DateTime.Now, FkInvoice, BusinessOwnerFkUser);
            //سند دریافت سفارش 
            CreateDocument(AccountingTypes.Buy_Invoice, 0, Payment_ToContinue_Money, DateTime.Now, FkInvoice, FkUser);

        }

        /// <summary>
        /// ذخیره سند
        /// </summary>
        /// <param name="Name">عنوان یا نام حساب</param>
        /// <param name="Creditor">بستانکار</param>
        /// <param name="Debtor">بدهکار</param>
        /// <param name="Date">تاریخ</param>
        /// <param name="FkPaymentLog">کد فاکتور</param>
        /// <param name="FkUser">کد کاربری</param>
        /// <returns></returns>
        public Accounting CreateDocument(string Name, decimal Creditor, decimal Debtor, DateTime date, long FkPaymentLog, int FkUser)
        {
            var accounting = new Accounting();
            accounting.Name = Name;
            accounting.Debtor = Debtor;
            accounting.Creditor = Creditor;
            accounting.Date = date;
            accounting.RegisterDate = DateTime.Now;
            // accounting.FkPaymentLog = FkPaymentLog;
            accounting.FkUser = FkUser;

            Add(accounting);
            //SaveEntity(accounting);

            return accounting;
        }

        /// <summary>
        ///  سند درخواست مبلغ از حساب با هزینه انتقال بین بانکی 
        /// </summary>
        /// <param name="FkUser"></param>
        /// <param name="CurrentMarketer"></param>
        /// <param name="FkInvoice"></param>
        /// <param name="RequestMoney"></param>
        /// <returns></returns>
        public void ShippingMoneyDocument(int FkUser, int FkInvoice, decimal RequestMoney)
        {

            if (validateReuestMoney(RequestMoney, FkUser))
            {
                //مبلغ انتقال وجه
                CreateDocument(AccountingTypes.Request_Balance_Money, RequestMoney, 0, DateTime.Now, FkInvoice, AccountingTypes.Bank_Account_Site_Value);
                CreateDocument(AccountingTypes.Request_Balance_Money, 0, RequestMoney, DateTime.Now, FkInvoice, FkUser);

                //سند هزینه انتقال وجه
                CreateDocument(AccountingTypes.Money_Shipping_Cost, ConstSetting.MoneyShippingCost, 0, DateTime.Now, FkInvoice, AccountingTypes.Money_Shipping_Cost_value);
                CreateDocument(AccountingTypes.Money_Shipping_Cost, 0, ConstSetting.MoneyShippingCost, DateTime.Now, FkInvoice, FkUser);

            }
            else
            { throw new MyException((byte)ExceptionType.validation, ExceptionType.validation.ToString(), "کاربر گرامی مبلغ درخواستی شما به علاوه هزینه انتقال بین بانکی نباید بیشتر از مانده شما در سایت باشد"); }
        }

        /// <summary>
        ///  سند درخواست مبلغ از حساب بدون هزینه انتقال بین بانکی 
        /// </summary>
        /// <param name="FkUser"></param>
        /// <param name="CurrentMarketer"></param>
        /// <param name="FkInvoice"></param>
        /// <param name="RequestMoney"></param>
        /// <returns></returns>
        public void ShippingMoneyDocumentWithOutCostShipping(int FkUser, int FkInvoice, decimal RequestMoney)
        {

            if (RequestMoney < AccountBalance(FkUser))
            {
                //مبلغ درخواستی
                CreateDocument(AccountingTypes.Request_Balance_Money, RequestMoney, 0, DateTime.Now, FkInvoice, AccountingTypes.Bank_Account_Site_Value);
                CreateDocument(AccountingTypes.Request_Balance_Money, 0, RequestMoney, DateTime.Now, FkInvoice, FkUser);
            }
            else
            { throw new MyException((byte)ExceptionType.validation, ExceptionType.validation.ToString(), "کاربر گرامی مبلغ درخواستی شما  نباید بیشتر از مانده شما در سایت باشد"); }
        }

        /// <summary>
        /// مبلغ درخواستی کاربر را با موجودی کاربر چک می کند
        /// </summary>
        /// <param name="RequestMoney"></param>
        /// <param name="FkUser"></param>
        /// <returns></returns>
        public bool validateReuestMoney(decimal RequestMoney, int FkUser)
        {
            var invoiceService = new InvoiceService(_OnlineShopping);

            // سفارش در انتظار ارسال را نشان می دهد
            var InvoiceWaitDelivery = invoiceService.FirstOrDefault(i => i.FkUser == FkUser && i.Status == InvoiceStatus.payment);
            decimal amuontInvoiceWaitDelivery = (InvoiceWaitDelivery == null ? 0 : InvoiceWaitDelivery.PaymentToCountinue);

            if (RequestMoney > 0 && (AccountBalance(FkUser) - RequestMoney - ConstSetting.MoneyShippingCost - amuontInvoiceWaitDelivery) >= 0) return true;
            else return false;
        }

        /// <summary>
        /// مانده حساب کاربر را محاسبه می کند
        /// </summary>
        /// <param name="FkUser"></param>
        /// <returns></returns>
        public decimal AccountBalance(int FkUser)
        {
            decimal _accountBalance = 0;
            foreach (var accounting in Find(a => a.FkUser == FkUser))
            {
                _accountBalance = _accountBalance + (accounting.Creditor - accounting.Debtor);
            }
            return _accountBalance;
        }

        public List<Accounting> GetTansctionsUser(int FkUser)
        {
            return Find(a => a.FkUser == FkUser).ToList();
        }
        /// <summary>
        /// متد دریافت هزینه ای آموزش از فروشنده
        /// </summary>
        /// <param name="FkUser_BusinessOwner"></param>
        /// <param name="FkUser_Marketer"></param>
        /// <param name="FkInvoice"></param>
        /// <param name="Payment_User_Money"></param>
        public void PaymentEductionBusinessOwner(int FkUser_BusinessOwner, int FkUser_Marketer, int FkInvoice, decimal Payment_User_Money)
        {
            //سند پرداخت کاربر
            _entity = CreateDocument(AccountingTypes.Payment_User, Payment_User_Money, 0, DateTime.Now, FkInvoice, FkUser_BusinessOwner);
            //سند دریافت حساب بانکی سایت 
            CreateDocument(AccountingTypes.Payment_User, 0, Payment_User_Money, DateTime.Now, FkInvoice, AccountingTypes.Bank_Account_Site_Value);

            //-------------------------------------------------فسمت دوم سند--------------------------------------------------
            decimal marketerPorsantMony = (((decimal)EductionPersent.Marketer) * Payment_User_Money / 100);
            //سند دریافت سهم آموزش سایت
            _entity = CreateDocument("سند دریافت سهم آموزش سایت", Payment_User_Money - marketerPorsantMony, 0, DateTime.Now, FkInvoice, AccountingTypes.daryaft_hazineh_Amozesh_site_value);

            //سند دریافت سهم آموزش بازاریاب
            _entity = CreateDocument("سند دریافت سهم آموزش بازاریاب", marketerPorsantMony, 0, DateTime.Now, FkInvoice, FkUser_Marketer);

            //سند دادن مبلغ آموزش توسط  فروشنده 
            CreateDocument("سند دادن مبلغ آموزش توسط  فروشنده", 0, Payment_User_Money, DateTime.Now, FkInvoice, FkUser_BusinessOwner);



        }
        #endregion IDocumentAccounting

        ///برای ولیدیت کردن صاحب انتیتی مورد استفاده قرار می گیردو متد های کلاس پدر را هر کلاس باید اور راید می کند
        #region ValidationOwnerEntity
        public override bool CheckOwnerEntity(int Id, int FkUser, out Accounting accounting)
        {
            accounting = FirstOrDefault(p => p.Id == Id);
            if (accounting != null && accounting.FkUser == FkUser)
                return true;
            else
                return false;
        }

        public override bool CheckOwnerEntity(Accounting entity, int FkUser)
        {
            if (entity != null && entity.FkUser == FkUser)
                return true;
            else
                return false;
        }




        #endregion ValidationOwnerEntity







    }
}
