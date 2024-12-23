using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;
using DataLayer.Enums;
using Utility;

using System.Web;
using DataLayer.EF;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using DataLayer.Contract;


namespace ServiceLayer
{
    public partial class InvoiceService : BaseService<Invoice>
    {
        BridgeInvoiceProduct _BridgeInvoiceProduct = new BridgeInvoiceProduct();
        Bridge_Invoice_ProductService _bridge_Invoice_ProductService;
        BusinessOwnerService _businessOwnerService;
        ProductService _productService;

        Invoice _invoice = new Invoice();

        public InvoiceService(OnlineShopping OnlineShopping)
            : base(OnlineShopping)
        {
            _bridge_Invoice_ProductService = new Bridge_Invoice_ProductService(OnlineShopping);
            _businessOwnerService = new BusinessOwnerService(OnlineShopping);
            _productService = new ProductService(OnlineShopping);
        }

        public Invoice CreateOrUpdateAddProductToInvoice(int FkBusinessOwner, int FkUser, int FkProduct, int? FkMarketer, int productCount, string color, ref string Message)
        {
            var busName = "";
            Message = "";
            var product = _productService.FirstOrDefault(p => p.Id == FkProduct);
            if (product == null)
            {
                throw new Exception("کالای مورد نظر یافت نشد");
            }
            ///اگر فاکتوری در مرحله ایجاد یا اقدام به پرداخت باشد را بر می گرداند
            var invoice = FirstOrDefault(i => i.FkUser == FkUser && i.Status <= InvoiceStatus.ActionToPay); // && i.FkBusinessOwner == FkBusinessOwner);
            if (invoice != null)
            {
                Message = "سفارش از قبل وجود داشت";
                _invoice = invoice;
                // busName = _invoice.BusinessOwner.Name;
            }
            else/// فاکتوری جدید ایجاد می گردد
            {
                Message = "سفارش جدید ایجاد شد";
                busName = _businessOwnerService.FirstOrDefault(b => b.Id == FkBusinessOwner).Name;
                var userService = new UserService(_OnlineShopping);
                var userName = userService.FirstOrDefault(u => u.Id == FkUser).Name;

                _invoice.FkUser = FkUser;
                _invoice.FkBusinessOwner = FkBusinessOwner;
                _invoice.FkMarketer = FkMarketer;
                _invoice.RegisterDate = DateTime.Now;
                _invoice.Status = InvoiceStatus.initialize;
                _invoice.PaymentType = PaymentType.PayOnline;

                Add(_invoice);
                SaveAllChengeOrAllReject(true);
            }
            ///اگر محصول اضافه شده در فاکتور موجود باشد آن را بر می گرداند
            var bip = _bridge_Invoice_ProductService.FirstOrDefault(b => b.FkInvoice == _invoice.Id && b.FkProduct == FkProduct && b.Colore == color);

            if (bip != null)
            {
                _BridgeInvoiceProduct = bip;
                _BridgeInvoiceProduct.FkInvoice = _invoice.Id;
                _BridgeInvoiceProduct.FkProduct = FkProduct;
                _BridgeInvoiceProduct.BuyPrice = product.BuyPrice;
                _BridgeInvoiceProduct.BeforDiscountPrice = product.BeforDiscountPrice;
                _BridgeInvoiceProduct.Price = product.Price;
                // _BridgeInvoiceProduct.Colore = color;
                _BridgeInvoiceProduct.Image = product.Image;
                _BridgeInvoiceProduct.Count += productCount;
                _BridgeInvoiceProduct.HistoryStateAndDescription = $"InvoiceDetilasState:{InvoiceDetilasStatus.Normal},";
            }
            else
            {
                _BridgeInvoiceProduct = new BridgeInvoiceProduct();
                _BridgeInvoiceProduct.FkInvoice = _invoice.Id;
                _BridgeInvoiceProduct.FkProduct = FkProduct;
                _BridgeInvoiceProduct.InvoiceDetilasState = InvoiceDetilasStatus.Normal;
                _BridgeInvoiceProduct.BuyPrice = product.BuyPrice;
                _BridgeInvoiceProduct.BeforDiscountPrice = product.BeforDiscountPrice;
                _BridgeInvoiceProduct.Price = product.Price;
                _BridgeInvoiceProduct.Colore = color;
                _BridgeInvoiceProduct.Image = product.Image;
                _BridgeInvoiceProduct.Count += productCount;
                _BridgeInvoiceProduct.HistoryStateAndDescription = $"InvoiceDetilasState:{InvoiceDetilasStatus.Normal},";

                _bridge_Invoice_ProductService.Add(_BridgeInvoiceProduct);
                // SaveAllChengeOrAllReject(true);
            }
            ///تغیرات در فاکتور جدید اعمال می گردد

            _invoice.UpdateDate = DateTime.Now;
            _invoice.ShippingCompany = ShippingCompanies.PostIran;


            FillCalcFieldInvoice(_invoice);

            SaveAllChengeOrAllReject(true);
            return _invoice;
        }
  
        public void ChangeStatusInvoice(Invoice invoice, InvoiceStatus invoiceStatus)
        {
            invoice.Status = invoiceStatus;
            AddHistory(invoice, invoiceStatus);
        }

        public static void AddHistory(Invoice invoice, InvoiceStatus invoiceStatus)
        {
            invoice.HistoryStateAndDescription = string.Concat(invoice.HistoryStateAndDescription, "{", "PaymentToCountinue:", invoice.PaymentToCountinue, ",InvoiceStatus:", (byte)invoiceStatus, ",ChangeTime:", DateTime.Now.ToString(), ",ChangeTimePersian:", DateTime.Now.ToPersianDateTime(), "},", Environment.NewLine);
        }

        //فاکتور موجود در کوکی را در ایتابیس ذخیره می نمایید 
        public void AddCookieInvoiceToDbInvoice(int CurrentUserId, int FkMarketer, Dictionary<int, int> Products, ref string MessageIsNewInvoice)
        {
            //  Dictionary<int, int> Products = CookieToDictionary();
            ProductService _productService = new ProductService(_OnlineShopping);

            //if (_product == null) throw new Exception("محصول مورد نظر وجود ندارد");
            Product product;

            foreach (var productInfo in Products)
            {
                product = _productService.FirstOrDefault(p => p.Id == productInfo.Key);
                _invoice = new Invoice();
                if (product.FkBusinessOwnerNavigation.FkUserNavigation.Id == CurrentUserId)
                    throw new MyException((byte)ExceptionType.UnnormalUser, "UnnormalUser", "اضافه کردن سفارشات قبل از لاگین، خرید از فروشگاه خود تعریف نشده است ");

                _invoice = CreateOrUpdateAddProductToInvoice(product.FkBusinessOwner, CurrentUserId, productInfo.Key, FkMarketer, 1, "", ref MessageIsNewInvoice);

            }
            SaveAllChengeOrAllReject(CheckOwnerEntity(_invoice, CurrentUserId));

        }

        public int DeleteItemInvoice(int bipId)
        {
            var bip = _bridge_Invoice_ProductService.FirstOrDefault(b => b.Id == bipId);
            _bridge_Invoice_ProductService.Delete(bip);
            _bridge_Invoice_ProductService.SaveAllChengeOrAllReject(true);

            var invoice = FirstOrDefault(i => i.Id == bip.FkInvoice);
            FillCalcFieldInvoice(invoice);
            SaveAllChengeOrAllReject(true);

            return bip.FkInvoice;
        }

        public void UpdateCountProduct(string[] bipc , int fkUser , int invoiceId)
        {
            BridgeInvoiceProduct bip = null;

            foreach (var item in bipc)
            {
                var id = int.Parse(item.Split(",")[0]);
                var countP = int.Parse(item.Split(",")[1]);
                bip = _bridge_Invoice_ProductService.FirstOrDefault(b => b.Id == id);
                bip.Count = countP;
            }
            // _bridge_Invoice_ProductService.SaveAllChengeOrAllReject(true);
            var invoice = FirstOrDefault(i => i.FkUser == fkUser && i.Id == invoiceId);
            FillCalcFieldInvoice(invoice);
            SaveAllChengeOrAllReject(true);
        }

        //updatinvoiceTopayedandDecreaseUserAcount
        public Invoice GetInvoice(long invoiceId, long currentUserId)
        {
            return FirstOrDefault(i => i.Id == invoiceId && i.FkUser == currentUserId);

        }
        public Invoice GetLastInvoice( long currentUserId)
        {
            return FirstOrDefault(i => i.FkUser == currentUserId && i.Status <= InvoiceStatus.ActionToPay );

        }

        public Invoice GetLastwithAddress(long currentUserId)
        {
            return GetAll().OrderByDescending(i=>i.Id).FirstOrDefault(i => i.FkUser == currentUserId &&
          ! string.IsNullOrWhiteSpace(i.DeliveryAddress ) && i.FkProvince != null);

        }
        public void InvoiceGoToPaymentState(Invoice invoice, int CurrentUserId, string _bankReferenceId)
        {
            try
            {
                //اگر کاربری یو ار ال ریدایرکت را دو باره و تکراری به ما ارسال کرد واز انجام تکراری وتولید تحویل تکراری جلو گیری می کند  
                if (invoice.Status >= InvoiceStatus.payment)
                    throw new MyException((byte)ExceptionType.PaymentSave, " ", "این فاکتور از مرحله پرداخت رد شده است");

                AccountingService _accountingService = new AccountingService(_OnlineShopping);
                // Random rand = new Random();
                _accountingService.PaymentUser(CurrentUserId, invoice.Id, invoice.PaymentToCountinue);
                // _entity.Date = UIUtility.CurrentDate;
                invoice.Status = InvoiceStatus.payment;
                SaveAllChengeOrAllReject(CheckOwnerEntity(invoice, CurrentUserId));
            }
            catch (Exception ex)
            {
                throw new MyException((byte)ExceptionType.PaymentSave, "خطای نادر ", "  خطا: لطفا با داشتن کد زیر با آدمین تماس بگیرید کد پرداخت:  " + "  " + _bankReferenceId);
            }
        }

        private void FillCalcFieldInvoice(Invoice invoice)
        {
            //تغییر به خاطر فاکتور مهمان اگر درست جواب داد تغییر می ماند
            // invoice.MoneySum = _bridge_Invoice_ProductService.TotalMoneySumByInvoiceId(invoice.Id);
            invoice.TotalSumProductPrice = _bridge_Invoice_ProductService.TotalMoneySumByInvoiceId(invoice.BridgeInvoiceProduct);
            var payToContinue = invoice.TotalSumProductPrice.ToDecimal(0) - invoice.Discount.ToDecimal(0);
            invoice.ShippingCost =( payToContinue >= DefualtValue.LimitationForFreeShipping || invoice.FkUser== 1398) ? 0 : DefualtValue.DefualtShippingCast;
            invoice.ProcessingDays =(short) (invoice.BridgeInvoiceProduct.Count > 0 ? invoice.BridgeInvoiceProduct.Max(b => b.FkProductNavigation.MaxShippingDay) : 0);
            invoice.PaymentToCountinue = payToContinue + invoice.ShippingCost; // add vat here 
        }

        public void InvoiceGoToActionPayStatus(int userId, int id, string DeliveryAddress, Banks bankCode,
            string NoteForBusinessOwner, int fk_Province, int? FK_Marketer ,
            string name, string familyName, string company, string mobile, string tel, string postCode ,
             ShippingCompanies ShippingCompany ,string cityName)
        {
            var _entity = FirstOrDefault(i => i.Id == id);
            _entity.FkMarketer = FK_Marketer;
            _entity.FkProvince = fk_Province;
          //  _entity.ShippingCost = calcShipping(fk_Province, id);
            ///مبلغ باید محاسبه گردد
           // _entity.PaymentToCountinue = _entity.TotalSumProductPrice + _entity.ShippingCost;

          //  _entity.Discount = 0;
           // _entity.PaymentToCountinue = _entity.PaymentToCountinue;

            
            _entity.NoteForBusinessOwner = NoteForBusinessOwner;
            _entity.RegisterDate = DateTime.Now;
            _entity.TimeBankPayInfo = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
            _entity.PaymentBankCode = bankCode;

            // string , string , string , string , string , string ,
           // _entity.DeliveryAddress =
                //string.Concat(" نام شهر:" , cityName ,Environment.NewLine,
                //" آدرس تحویل:",   DeliveryAddress, Environment.NewLine,
                //" نام:" ,name, Environment.NewLine,
                //" نام خانوادگی:", familyName, Environment.NewLine,
                //" نام شرکت:" , company, Environment.NewLine,
                //" موبایل:" , mobile, Environment.NewLine,
                //" تلفن:", tel, Environment.NewLine,
                //" پست:" , postCode );

            _entity.DeliveryAddress = DeliveryAddress;
            _entity.DeliveryName = name;
            _entity.DeliveryLastName = familyName;
            _entity.DeliveryMobile = mobile;
            _entity.DeliveryCompanyName = company;
            _entity.DeliveryPostCode = postCode;
            _entity.DeliveryTel = tel;
            _entity.DeliveryCityName = cityName;


            _entity.ShippingCompany = ShippingCompany;
            //_entity.na = ;
            //_entity. = familyName;
            //_entity.company = ;
            //_entity. = mobile;
            //_entity. = tel;
            //_entity. = postCode;



            ChangeStatusInvoice(_entity, InvoiceStatus.ActionToPay);

            SaveAllChengeOrAllReject(CheckOwnerEntity(_entity, userId));
        }
        public PaymentLog CreatePaymentLog(int? invoiceId ,long userId , decimal amount , string description)
        {
            PaymentLog paylog = new PaymentLog();
            paylog.UserId = userId;
            paylog.Status = PayLogType.request;
            paylog.UpdateDate = DateTime.Now;
            paylog.CreateDate = DateTime.Now;
            paylog.InvoiceId = invoiceId;
            paylog.Amount = amount;
            paylog.Description = description;


            _OnlineShopping.PaymentLogs.Add(paylog);
            if (_OnlineShopping.SaveChanges() < 0) throw new Exception("عملیات ذخیره پرداخت با شکست مواجه شد");
            return paylog;
        }
        public InvoiveUser GetInvoiceUser(int userId)
        {
            var invoice = FirstOrDefault(i => i.FkUser == userId && i.Status <= InvoiceStatus.ActionToPay);
            if (invoice == null)
            {
                return null;
            }
            InvoiveUser InvoiveUser = new InvoiveUser
            {
                PaymentToCountinue = invoice.PaymentToCountinue,
                TotalSumProductPrice = invoice.TotalSumProductPrice,
                InvoiceId = invoice.Id,
                InvoiveItemUsers = new List<InvoiveItemUser>()
            };
            foreach (var item in invoice.BridgeInvoiceProduct)
            {
                InvoiveUser.InvoiveItemUsers.Add(new InvoiveItemUser
                {
                    Count = item.Count,
                    Price = item.Price,
                    ProductName = item.FkProductNavigation.Name,
                    ProductId = item.FkProduct,
                    ProductImage = item.Image,
                    ProductNameForUrl =item.FkProductNavigation.NameForUrll,
                    BIPId = item.Id,
                });
            }
            return InvoiveUser;

        }

        /// <summary>
        /// فاکتور را از لحاظ مبلغ پرداختی و هزینه حمل و هزینه بازاریابی وبقیه موارد چک میکند
        /// </summary>
        /// <param name="invoice"></param>
        public void CheckInvoice(Invoice invoice)
        {
            if (invoice.PaymentToCountinue < 0 | invoice.PaymentToCountinue < 0 | invoice.Discount < 0 | invoice.TotalSumProductPrice < 0 | invoice.ShippingCost < 0)
            {
                SaveAllChengeOrAllReject(false);
                throw new MyException((byte)ExceptionType.validation, "CheckInvoice", "مبلغ پرداختی نباید بیشتر از مبلغ نهایی  باشد ");
            }

            if (invoice.Discount > (invoice.TotalSumProductPrice / 2))
            {
                SaveAllChengeOrAllReject(false);
                throw new MyException((byte)ExceptionType.validation, "CheckInvoice", "کاربر گرامی حداکثر تخفیف درخواستی شما باید کمتراز نصف مبلغ کل فروش باشد.");
            }
            if (invoice.PaymentToCountinue != null && invoice.PaymentToCountinue < _bridge_Invoice_ProductService.CalcMarketingByInvoiceId(invoice.Id) && invoice.PaymentToCountinue < 1000)
            {
                SaveAllChengeOrAllReject(false);
                // RoleBackContext();
                throw new MyException((byte)ExceptionType.validation, "CheckInvoice", "مبلغ پرداختی نمی تواند کمتراز هزار ریال یا پورسانت بازاریابی باشد پورسانت بازاریابی:  " + _bridge_Invoice_ProductService.CalcMarketingByInvoiceId(invoice.Id) + "  ریال ");
            }
            if (invoice.PaymentToCountinue > invoice.PaymentToCountinue)
            {
                SaveAllChengeOrAllReject(false);
                throw new MyException((byte)ExceptionType.validation, "CheckInvoice", "مبلغ پرداختی نباید بیشتر از مبلغ نهایی  باشد ");
            }
        }


        ///برای ولیدیت کردن صاحب انتیتی مورد استفاده قرار می گیردو متد های کلاس پدر را هر کلاس باید اور راید می کند
        #region ValidationOwnerEntity
        public override bool CheckOwnerEntity(int Id, int FkUser, out Invoice invoice)
        {
            invoice = FirstOrDefault(p => p.Id == Id);
            if (invoice != null && invoice.FkUser == FkUser)
                return true;
            else
                return false;
        }

        public override bool CheckOwnerEntity(int Id, int FkUser)
        {
            var invoice = FirstOrDefault(p => p.Id == Id);
            if (invoice != null && invoice.FkUser == FkUser)
                return true;
            else
                return false;
        }

        public override bool CheckOwnerEntity(Invoice entity, int FkUser)
        {
            if (entity != null && entity.FkUser == FkUser)
                return true;
            else
                return false;
        }
        public bool CheckOwnerEntityBusinessOwner(Invoice entity, int FkBusinessOwner)
        {
            if (entity != null && entity.FkBusinessOwner == FkBusinessOwner)
                return true;
            else
                return false;
        }

        #endregion ValidationOwnerEntity


        /// <summary>
        /// محاسبه هزینه حمل و نقل
        /// </summary>
        /// <param name="ProvinceId">کد استان</param>
        /// <param name="InvoiceId">کد فاکتور</param>
        /// <returns></returns>
        public decimal calcShipping(int ProvinceId, int InvoiceId)
        {
            var invoiceService = new InvoiceService(_OnlineShopping);
            var invoice = invoiceService.FirstOrDefault(i => i.Id == InvoiceId);
            var bridge_Province_BusinessOwner = invoice.FkBusinessOwnerNavigation.BridgeProvinceBusinessOwner.FirstOrDefault(b => b.FkProvince == ProvinceId);

            return calcShipping(ProvinceId, invoice);
        }

        /// <summary>
        /// محاسبه هزینه حمل و نقل
        /// </summary>
        /// <param name="ProvinceId">کد استان</param>
        /// <param name="InvoiceId">کد فاکتور</param>
        /// <returns></returns>
        public decimal calcShipping(int ProvinceId, Invoice invoice)
        {
            var bridge_Province_BusinessOwnerService = new Bridge_Province_BusinessOwnerService(_OnlineShopping);
            return bridge_Province_BusinessOwnerService.CalcShipping(ProvinceId, invoice);
        }


        /// <summary>
        /// این متد بر اساس کد فروشنده فاکتور  مربوط به فروشنده مورد نظر را تولید می کند
        /// این متد از آبجکت کانتکست جدا می باشد
        /// </summary>
        /// <param name="FkBusinessOwner"></param>
        /// <param name="Products"></param>
        /// <param name="_invoice"></param>
        public void DictionaryToInvoice(int FkBusinessOwner, Dictionary<int, int> Products, ref Invoice _invoice)
        {
            BusinessOwnerService bussinessOwnerService = new BusinessOwnerService(_OnlineShopping);
            BusinessOwner bussinesOwner = bussinessOwnerService.FirstOrDefault(b => b.Id == FkBusinessOwner);

            foreach (var product in Products)
            {
                Product ObjProduct = bussinesOwner.Product.FirstOrDefault(p => p.Id == product.Key);
                //_OnlineShopping.Detach(ObjProduct);
                if (ObjProduct != null)
                {
                    _invoice.BridgeInvoiceProduct.Add(new BridgeInvoiceProduct
                    {
                        Id = -1,
                        FkInvoice = 0,
                        FkProduct = product.Key,
                        Count = product.Value,
                        FkProductNavigation = new Product { Name = ObjProduct.Name, Price = ObjProduct.Price, CountPrice = ObjProduct.CountPrice, Image = ObjProduct.Image }
                    });
                    _invoice.FkBusinessOwnerNavigation = new BusinessOwner { Name = bussinesOwner.Name };
                }
            }
            //  _invoice.FkBusinessOwner = -1;
            _invoice.RegisterDate = DateTime.Now;
            _invoice.FkBusinessOwner = FkBusinessOwner;
            FillCalcFieldInvoice(_invoice);

        }

        public List<short> FullStateList()
        {
            List<short> stateListShower = new List<short>();
            stateListShower.Add((byte)InvoiceStatus.initialize);
            stateListShower.Add((byte)InvoiceStatus.ActionToPay);
            stateListShower.Add((byte)InvoiceStatus.payment);
            stateListShower.Add((byte)InvoiceStatus.CanceleByCustomer);
            stateListShower.Add((byte)InvoiceStatus.CanceleByShop);
            stateListShower.Add((byte)InvoiceStatus.Send);
            stateListShower.Add((byte)InvoiceStatus.IncompleteSend);
            stateListShower.Add((byte)InvoiceStatus.Final);
            return stateListShower;
        }

        public void ApplyDiscount(long invoiceId, string coupon)
        {
            //throw new NotImplementedException();

        }

        public void PayByCredit(int invoiceId)
        {
            // throw new NotImplementedException();
        }

        public new IPagedList<Invoice> GetAll(Pagination pagination, int? invoiceId, string userEmail, int? invoiceStatus, OrderByInvoice orderBy)
        {
            var qry = _OnlineShopping.Invoice
                .Include(a => a.FkUserNavigation).AsQueryable();

            if (invoiceId.HasValue)
            {
                qry = qry.Where(a => a.Id == invoiceId.Value);
            }
            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                qry = qry.Where(a => a.FkUserNavigation.Email.Contains(userEmail));
            }
            if (invoiceStatus.HasValue)
            {
                qry = qry.Where(a => a.Status == (InvoiceStatus)invoiceStatus.Value);
            }

            if(orderBy== OrderByInvoice.Amount)
            {
                qry = qry.OrderByDescending(q => q.PaymentToCountinue);
            }
            else if (orderBy == OrderByInvoice.CreateDate)
            {
                qry = qry.OrderByDescending(q => q.RegisterDate);
            }
            else if (orderBy == OrderByInvoice.UpdateDate)
            {
                qry = qry.OrderByDescending(q => q.UpdateDate);
            }
            else if (orderBy == OrderByInvoice.Status)
            {
                qry = qry.OrderByDescending(q => q.Status);
            }
            else if (orderBy == OrderByInvoice.MoboieNumber)
            {
                qry = qry.OrderByDescending(q => q.FkUserNavigation.Mobile);
            }
            else 
            {
                qry = qry.OrderByDescending(q => q.Id);
            }
            return PagedList<Invoice>.Create(qry, pagination);
        }
        public Invoice GetbyId(int id)
        {
            var result = _OnlineShopping.Invoice
                .Include(a => a.FkUserNavigation)
                .Include(a => a.BridgeInvoiceProduct)
                .ThenInclude(b => b.FkProductNavigation)
                .FirstOrDefault(i=>i.Id==id);

            return result;
        }
    }


}
