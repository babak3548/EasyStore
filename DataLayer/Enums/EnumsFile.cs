using DataLayer.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;



namespace DataLayer.Enums
{
    public class DefualtValue
    {
        //    public Dictionary<int, string> ImgExtention;
        //    public Dictionary<int, string> TextExtention;

        //    public void SetExtintionFiles ()
        //{



        public static Dictionary<int, string> ImgExtention = new Dictionary<int, string>()
        {
           { 1 , ".jpg"},{ 2 , ".JPEG"},{ 3 , ".PNG"}
           ,{ 4 , ".TIFF"},{ 5 , "jpg"}
           ,{ 6 , ".Exif"},{ 7 , ".GIF"}
           ,{ 8 , ".BMP"},{ 9 , ".PPM"},
           { 10 , ".PNM"},{ 11 , ".WEBP"}
        };

        public static Dictionary<int, string> TextExtention = new Dictionary<int, string>
        {
           {1, ".txt"},         {2, ".pdf"},
           {3, ".docx"},           {4, ".doc"},
           {5, ".rtf"},           {6, ".odt"},
           {7, ".xps"},       {8, ".rar"}
        };


        public const string MenuName = "Menu";
        public const decimal DefualtShippingCast = 1; // ریال 
        public const decimal LimitationForFreeShipping = 2; // ریال 

        public const int PostDeliveryTime = 2; // ریال 
        public const int ExpressInTehranDeliveryTime = 1; // ریال 


        /// <summary>
        /// نقشهای موجود در سیستم
        /// </summary>

        public static Dictionary<int, string> PartialType = new Dictionary<int, string>()
    {
       {0, "DefaultValue"},
       {1, "Lable"},
       {2, "Editor"},
       {3, "Date"},
       {4, "image"}
    };

        public const string AllCategory = "all";
        public const string sliderDefaultImg = "0sliderDefault";



    }

    public static class EnumUtility
    {
        public static DateTime CalcDeliveryDatetime(this ShippingCompanies shippingCompanies, int processDay)
        {
            DateTime deleveryDay = DateTime.Now;
            //if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday || DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            //{
            //    deleveryDay = deleveryDay.AddDays(1);
            //}
            if (shippingCompanies == ShippingCompanies.PostIran)
            {
                deleveryDay= deleveryDay.AddDays(processDay +DefualtValue.PostDeliveryTime);
            }
            else if (shippingCompanies == ShippingCompanies.ShippingInLocalCity)
            {
                deleveryDay= deleveryDay.AddDays(processDay + DefualtValue.ExpressInTehranDeliveryTime);
            }
            else
            {
                deleveryDay = deleveryDay.AddDays(processDay + DefualtValue.PostDeliveryTime);
            }
            return deleveryDay;
        }
        /// <summary>
        /// این متد همیشه باید با نام فیلدهای طرف دیتا بیس مچ باشد
        /// </summary>
        /// <param name="EnumName"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetLSelectListItem(string EnumName, string selectedValue)
        {
            switch (EnumName)
            {
                case "PartialType": return EnumToList<PartialType>(selectedValue);
                //             case "GridSetting": return NewMethod<GridSetting>(selectedValue);
                //case "AccessEnum": return NewMethod<AccessEnum>(selectedValue);
                case "DisplayMode": return EnumToList<DisplayMode>(selectedValue);
                case "TrendDocuments": return EnumToList<TrendDocuments>(selectedValue);
                case "Width": return EnumToList<Width>(selectedValue);
                case "Position": return EnumToList<Position>(selectedValue);
                case "SellOrBuy": return DictionaryToList(SellOrBuy, selectedValue);
                case "State": return EnumToCurrentLanguageList<InvoiceStatus>(selectedValue);

                default: return EnumToList<NotFound>(selectedValue);

            }
        }

        /// <summary>
        /// مقدار انم را به صورت لاتین در ایتم لیست بر می گرداند
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        private static List<SelectListItem> EnumToList<T>(string selectedValue = "")
        {
            List<SelectListItem> LSelectListItem = new List<SelectListItem>();

            foreach (var item in Enum.GetValues(typeof(T)).Cast<T>())
            {
                LSelectListItem.Add(new SelectListItem { Text = item.ToString(), Value = Convert.ToInt16(item).ToString(), Selected = (Convert.ToInt16(item).ToString() == selectedValue) });
            }
            return LSelectListItem;
        }

        public static List<EnumListItem> EnumToList<T>()
        {
            List<EnumListItem> LSelectListItem = new List<EnumListItem>();

            foreach (var item in Enum.GetValues(typeof(T)).Cast<T>())
            {
                LSelectListItem.Add(new EnumListItem { Name = GetDescription<T>(Convert.ToInt16(item)), Id = Convert.ToInt16(item) });
            }
            return LSelectListItem.OrderBy(a => a.Id).ToList();
        }
        /// <summary>
        /// مقدار انم را به صورت زبان جاری در ایتم لیست بر می گرداند
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static List<SelectListItem> EnumToCurrentLanguageList<T>(string selectedValue = "")
        {
            List<SelectListItem> LSelectListItem = new List<SelectListItem>();

            foreach (var item in Enum.GetValues(typeof(T)).Cast<T>())
            {
                LSelectListItem.Add(new SelectListItem { Text = GetDescription<T>(Convert.ToInt16(item)), Value = Convert.ToInt16(item).ToString(), Selected = (Convert.ToInt16(item).ToString() == selectedValue) });
            }
            return LSelectListItem;
        }


        /// <summary>
        /// بر حسب ولیو عددی یک اینوم مقدار استرینق آن در زبان جاری را بر می گردداند
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EnumToLanguageValue<T>(short value)
        {
            return GetDescription<T>(value);
        }

        public static string GetDescription<T>(this int? val)
        {
            if (val == null) return "";

            return GetDescription<T>(val.Value);
        }

        public static string GetDescription<T>(this int val)
        {
            var res = GetEnum<T>(val);
            return GetDescription(res);
        }
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }

        public static Enum GetEnum<T>(this int val)
        {
            var res = (Enum)Enum.Parse(typeof(T), val.ToString());
            return res;
        }

        /// <summary>
        /// بر حسب ولیو عددی یک اینوم مقدار استرینق لاتین آن را بر می گردداند
        /// </summary>
        /// <typeparam name="T">نوع اینوم</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EnumShortValueToStringValue<T>(short value)
        {
            var item = Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(i => Convert.ToInt16(i) == value);
            return item.ToString();
        }
        /// <summary>
        /// یک لیست را به صورت یک لیستی از سلیکت لیست ایتم بر می گرداند  
        /// </summary>
        /// <param name="drdValue"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        private static List<SelectListItem> DictionaryToList(Dictionary<string, string> drdValue, string selectedValue = "")
        {
            List<SelectListItem> LSelectListItem = new List<SelectListItem>();

            foreach (var item in drdValue)
            {
                LSelectListItem.Add(new SelectListItem { Text = item.Key, Value = item.Value.ToString(), Selected = (item.Value.ToString().ToLower() == selectedValue.ToLower()) });
            }
            return LSelectListItem;
        }

        public static Dictionary<string, string> SellOrBuy = new Dictionary<string, string>
        {
              { "TrueValue" ,"true"},
            {"FalseValue","false"}
        };

        public static PromotionTypes GetPromotionTypeProduct(this PromotionTypes promotionTypes)
        {
            switch (promotionTypes)
            {
                case PromotionTypes.TopHomeSlider1_3: return PromotionTypes.TopHomeSlider1_3_M;
                case PromotionTypes.TopHomeSlider2_3: return PromotionTypes.TopHomeSlider2_3_M;
                case PromotionTypes.TopHomeSlider3_3: return PromotionTypes.TopHomeSlider3_3_M;

                case PromotionTypes.SearchBaner: return PromotionTypes.SearchBaner_M;
                case PromotionTypes.HomeImgSecondRow1_2: return PromotionTypes.HomeImgSecondRow1_2_M;
                case PromotionTypes.HomeImgSecondRow2_2: return PromotionTypes.HomeImgSecondRow2_2_M;

                case PromotionTypes.HomeImgTopRow1_3: return PromotionTypes.HomeImgTopRow1_3_M;
                case PromotionTypes.HomeImgTopRow2_3: return PromotionTypes.HomeImgTopRow2_3_M;
                case PromotionTypes.HomeImgTopRow3_3: return PromotionTypes.HomeImgTopRow3_3_M;

                case PromotionTypes.BottenHomePage: return PromotionTypes.HomeImgTopRow3_3_M;



                default: return PromotionTypes.NoSetPromotionType; ;
            }
        }
    }



    public enum PartialType
    {

        DefaultValue = 0,
        Lable = 1,
        Editor = 2,//ویرایشگر  اچتی ام ال منظور ماست
        Date = 3,
        Image = 4,
        DropDown = 5,
        LinkChild = 6,
        EditOrDelete = 7,
        CheckBoxChecked = 8,
        EmptyTextBox = 9,
        DropDownWithList = 10,
        DropDownEnums = 11,
        UploadImage = 12,
        Link = 13,
        Numeric = 14,
        Email = 15,
        ReEmail = 16,
        Password = 17,
        RePassword = 18,
        CheckBox = 19,
        btnFormSubmit = 20,
        TextArea = 21,
        HiddenFiled = 22,
        UploadFile1 = 23,
        TextAreaField = 24,
        CostumLink = 25,
        AjaxTextBox = 26,
        /// <summary>
        /// فیلد پیش فرض استاتیک که پارشیال مدل باید برای آن پر شود
        /// </summary>
        DefaultFeildView = 27,
        /// <summary>
        /// فیلد پیش فرض داینامیک
        /// </summary>
        DefaultFeildViewDyn = 28,
        UploadFile = 29

    }
    public enum TypeMessage
    {
        Alarmed = 1,
        BusinessOwner = 2,
        Marketer = 2,
        DirectEmail = 3,
        Guest = 4,

    }


    public enum ExceptionType
    {
        validation = 1,
        NotImplement = 2,
        AccessRoute = 3,
        UnnormalSenario = 4,//عملیات آن نورمال از کاربر احتمال تلاش برای هک
        PaymentSave = 5,
        Payment = 6,
        UnnormalUser = 7,//رفتار نادرست کاربر 
    }
    public enum Display
    {
        Grid = 2,
        Edit = 4,
        DisPlay = 8,
        Filter = 16
    }
    /// <summary>
    /// اگر ردانلی باشد قابل تغییر نیست 
    /// </summary>
    public enum DisplayMode
    {
        Grid = 2,
        Edit = 4,
        ReadOnly = 8,
        Filter = 16,
        Grid_Edit = 6,
        Grid_Edit_Filter = 22,
        Grid_ReadOnly = 10,
        Edit_ReadOnly = 12,
        Grid_Edit_ReadOnly = 14,
        Grid_Filter_ReadOnly = 26,
        Grid_Edit_Filter_ReadOnly = 30,





    }
    public enum MarketingPersent
    {
        Site = 50,
        seller = 30,
        ParentMarketing = 10,
        Presenter = 10,


    }
    public enum EductionPersent
    {
        Site = 20,
        Marketer = 80,



    }


    /// <summary>
    /// روند یا فرایندهای عملیات سند زدن
    /// </summary>
    public enum TrendDocuments
    {
        PrimaryBuy = 1,
        AfterApproved = 2,
        TransferRequest = 3,
        Transfer = 4,
    }
    /// <summary>
    ///  عرض های استاندارد در یوآی پروژه 
    /// </summary>
    public enum Width
    {
        Small = 20,
        X_Small = 30,
        Large = 50,
        X_Large = 80,
        XX_Large = 90,
        XXX_Large = 100
    }

    /// <summary>
    ///  پوزیشن های استاندارد در یوآی پروژه 
    /// </summary>
    public enum Position
    {
        Header = 1,
        Category = 2,
        Propaganda = 3,
        Bottom = 4,
        center = 5
    }

    public enum NotFound
    {
        NotFoundEnum = 1
    }

    public enum PaymentType
    {
        [Description("پرداخت در محل")]
        PayInPlace = 1,
        [Description("پرداخت آنلاین")]
        PayOnline = 2,
        [Description("پرداخت از اعتبار")]
        PayByCredit = 3,
        [Description("پرداخت کارت به کارت سامان")]
        PayCardToCard = 4
    }
    
    public enum ShippingTypes
    {
        [Description("ارسال پستی ")]
        initialize = 2,
        [Description("ارسال پیکی در تهران")]
        payment = 4,//
    }
    public enum InvoiceStatus
    {
        [Description("ایجاد")]
        initialize = 0,
        [Description("اقدام به پرداخت")]
        ActionToPay = 3,//  
        [Description("پرداخت شده")]
        payment = 4,//
        [Description("ارسال شده")]
        Send = 5,//
        [Description("ارسال ناقص")]
        IncompleteSend = 6,//
        [Description("نهایی")]// 
        Final = 7,
        [Description("لغو توسط مشتری ")]
        CanceleByCustomer = 8,
        [Description("لغو توسط فروشگاه ")]
        CanceleByShop = 9,
        [Description("دریافت نشده")]
        HasNotReachedTheCustomer = 10,
        [Description("برگشتی")]
        Rejected = 11,
        [Description("برگشت مبلغی از پرداخت")]
        RejectedSomeOfPay = 12

    }
    public enum InvoiceDetilasStatus
    {
        [Description("نرمال")]
        Normal = 1,//  
        [Description("نهایی")]// 
        Final = 2,
        [Description("ارسال نشده")]
        IncompleteSend = 4,//
        [Description("عدم موجودی")]
        NotAvilabel = 6,//
        [Description("لغو شده")]
        CanceleByCustomer = 8,
        [Description("دریافت نشده")]
        HasNotReachedTheCustomer = 9,
        [Description("برگشتی")]
        Rejected = 10,
        [Description("برگشت مبلغی از پرداخت")]
        RejectedSomeOfPay = 11

    }
    
    public enum Gender
    {
        [Description("آقا")]
        Male = 1,//  
        [Description("خانم")]// 
        Female = 2,//  
    }

    public enum ScoreInvoice
    {
        Abstained = 0,//ممتنع
        Satisfied = 1,//راضی
        NoSatisfied = 2,//نا راضی  
    }

    public enum WriterDispute
    {
        WriterDisputeUser = 0,//نویسنده کاربر
        WriterDisputeBussinessOwner = 1,//
        WriterDisputeMarketer = 2,//  
        WriterDisputeAdmin = 3
    }

    public enum DisputerwhoPerson
    {
        disputerwhoPersonUser = 0,//خریدار
        disputerwhoPersonBussinessOwner = 1//,فروشنده

    }

    public enum ShippingCompanies
    {
        [Description(" نوع ارسال نامشخص")]
        NoDetermin = 0,
        [Description(" ارسال پستی")]
        PostIran = 2,//  
        [Description("ارسال اکسپرس  (فقط شهر تهران)")]
        ShippingInLocalCity = 4,//پیک خود فروشنده
       // Tepaxs = 4,//شرکت 
     
        //Barbari = 16,//  
       // TPG = 32,//
       // Aramex = 64,//
       // Terminal = 128,//
    }
    public enum CalcTypeShippings
    {
        CalcBySite = 2,//
        Free = 4,//رایگان 
        CalcByBussinessOwner = 8,//  توسط فروشنده

    }
    //
    //public enum InvoiceType
    //{
    //    normalInvoice = 0,//همان فاکتور فروش نرمال سیستم
    //    requestShippingMony = 8,//در خواست انتقال وجه
    //    EductionInvoice = 10,//در خواست انتقال وجه

    //}
    public enum TypeContent
    {
        HtmlText = 1,
        Normaltext = 2,
        Question = 3,
        Answer = 4
    }
    public enum SellerPerson
    {
        Seller = 0,
        Site = 1
    }
    public enum TypeSell
    {
        [Description("تامین کننده")]
        Supplyer = 2,
      //  Seller = 4,

    }
    public enum TypeShippingBussinessOwner
    {
        [Description("بر عهد خودمون")]
         WeSipping= 1,
        [Description("تامین کننده تحویل می ده")]
        HeSipping = 2,
        [Description("خودمان یا تامین کننده بسته به شریط")]
        WeOrHeSipping = 3,
        [Description("تامین کننده با پیک و هزینه ما ارسال می کنه")]
        WithPaykSipping = 4,
        [Description("مشخص نیست")]
        NoDetermine = 10,
        //  Seller = 4,

    }
    
    public enum CommentType
    {
        Yourproposals = 1,
        QuestionAdmin = 2,
        QuestionPublic = 3,
        WorkToUs = 4,
        ContractUs = 5,
        CommentForProduct = 6,

    }

    public enum Banks
    {
        [Description("ملی")]
        Meli = 1,
        [Description("پاسارگاد")]
        Pasargad = 2
    }
    public enum PayLogType
    {
        [Description("درخواست پرداخت")]
        request = 1,
        [Description("تایید پرداخت")]
        veryFiy = 2,
        [Description("عدم تایید پرداخت")]
        noVeryFiy = 3,
        [Description("در حال بررسی")]
        InVeryFing = 4,
    }
    public enum PromotionTypes
    {
        [Description(" نوع پرومشن نامشخض")]
        NoSetPromotionType = 0,
        [Description("محصولات پیشنهاد های برتر ماه")]
        BestOfferMonth = 1,
        [Description("محصولات ویژه")]
        SpecialProducts = 2,
        [Description("محصولات پرفروش")]
        BestSellerProducts = 3,
        [Description("محصولات جدید")]
        NewProducts = 4,
        [Description("محصولات پایین صفحه اصلی")]
        BottenHomePage = 5,
        [Description("محصولات بالاترین امتیاز ماه")]
        TopScoreMonth = 6,
        [Description("محصولات گروهای نمایشی در صفحه اصلی")]
        TopHomeCategory = 7,

        [Description("اسلایدر در صفحه اصلی 1 از 3")]
        TopHomeSlider1_3 = 100,
        [Description("محصولات اسلایدر در صفحه اصلی 1 از 3")]
        TopHomeSlider1_3_M = 9,
        [Description("اسلایدر در صفحه اصلی 2 از 3")]
        TopHomeSlider2_3 = 110,
        [Description("محصولات اسلایدر در صفحه اصلی 2 از 3")]
        TopHomeSlider2_3_M = 11,
        [Description("اسلایدر در صفحه اصلی 3 از 3")]
        TopHomeSlider3_3 = 120,
        [Description("محصولات اسلایدر در صفحه اصلی 3 از 3")]
        TopHomeSlider3_3_M = 13,


        [Description("عکس ردیف اول در صفحه اصلی 1 از 3")]
        HomeImgTopRow1_3 = 140,
        [Description("محصولات عکس ردیف اول در صفحه اصلی 1 از 3")]
        HomeImgTopRow1_3_M = 15,
        [Description("عکس ردیف اول در صفحه اصلی 2 از 3")]
        HomeImgTopRow2_3 = 160,
        [Description("محصولات عکس ردیف اول در صفحه اصلی 2 از 3")]
        HomeImgTopRow2_3_M = 17,
        [Description("عکس ردیف اول در صفحه اصلی 3 از 3")]
        HomeImgTopRow3_3 = 180,
        [Description("محصولات عکس ردیف اول در صفحه اصلی 3 از 3")]
        HomeImgTopRow3_3_M = 19,

        [Description("عکس ردیف دوم در صفحه اصلی 1 از 2")]
        HomeImgSecondRow1_2 = 200,
        [Description("محصول ردیف دوم در صفحه اصلی 1 از 2")]
        HomeImgSecondRow1_2_M = 21,
        [Description("عکس ردیف دوم در صفحه اصلی 2 از 2")]
        HomeImgSecondRow2_2 = 220,
        [Description("محصول ردیف دوم در صفحه اصلی 2 از 2")]
        HomeImgSecondRow2_2_M = 23,

        [Description("بنر صفحه جستجو")]
        SearchBaner = 240,
        [Description("محصولات بنر صفحه جستجو")]
        SearchBaner_M = 25,

        [Description("کلید سیاه صفحه اول")]
        HomeBlackKey = 250,
        [Description("محصولات کلید سیاه صفحه اول")]
        HomeBlackKey_M = 27,
        //[Description("بنر اول در صفحه اصلی 2 از 2")]
        //FirstHomeBaner2_2 = 260,
        //[Description("محصولات بنر اول در صفحه اصلی 2 از 2")]
        //FirstHomeBaner2_2_M = 27,


        //[Description("بنر دوم در صفحه اصلی 3 از 1")]
        //SecondHomeBaner1_3 = 280,
        //[Description("محصولات بنر دوم در صفحه اصلی 3 از 1")]
        //SecondHomeBaner1_3_M = 29,
        //[Description("بنر دوم در صفحه اصلی 3 از 2")]
        //SecondHomeBaner2_3 = 300,
        //[Description("محصولات بنر دوم در صفحه اصلی 3 از 2")]
        //SecondHomeBaner2_3_M = 31,
        //[Description("بنر دوم در صفحه اصلی 3 از 3")]
        //SecondHomeBaner3_3 = 320,
        //[Description("محصولات بنر دوم در صفحه اصلی 3 از 3")]
        //SecondHomeBaner3_3_M = 33,
    }


    public enum ContentTypes
    {
        [Description("پایین صفحه اصلی")]
        BottomHomePage = 1,
        [Description("سیسمونی")]
        Sismoni = 2,
        [Description("عروسک پولیشی")]
        DollPolish = 3,
        [Description("صوتی تصویری")]
        Multimedia = 4,
        [Description("خبری")]
        News = 5,
        [Description("بدون فرمت")]
        Other = 254,
        [Description("غیر فعال")]
        UnActive = 255

    }
    public enum OrderByInvoice
    {
        [Description("شناسه")]
        Id = 1,
        [Description("تاریخ ویرایش")]
        UpdateDate = 2,
        [Description("تاریخ ایجاد")]
        CreateDate = 3,
        [Description("مبلغ")]
        Amount = 4,
        [Description("وضعیت")]
        Status = 5,
        [Description("شماره کاربر")]
        MoboieNumber = 6,
    }

    
    public enum ProductActivationStatus : byte
    {
        
        [Description("جدید ایجاد شده")]
        NewCreated = 1,
        [Description("درخواست اصلاح محتوا")]
        RequestForContent = 2,
        [Description("عدم موجودی")]
        NotAvilable = 3,
        [Description("فعال برای فروش")]
        ActiveForSell = 4,
        [Description("غیرفعال برای فروش")]
        UnActiveAnyReason= 5,



        [Description("نامشخص")]
        NotDetermine = 255

    }
    public enum PromotionCategory
    {

        [Description("گروهای نمایشی در صفحه اصلی")]
        TopHomeCategory = 1
    }

    public class ConstSetting
    {
        public const short BeginRow = 1;
        public const short PageSize = 24;
        public const string GuestRoleName = "Guest";
        public const string AdminRoleName = "Admin";
        public const string CategorySettingRoute = "route";

        public const string NoChenge = "NnnoCchhh";
        public const string NoSendTransactionReferenceID = "-1";

        public const string Access_dinied_route = "Access_dinied_route";
        public const int admin = 2;
        public const int BusinessOwner = 4;
        public const int Marketer = 8;
        public const int Guest = 16;

        public const string Link = "Link";
        public const string Span = "Span";

        //public const string EmailGuest = "baba@jja.com";

       // public const string Password = "teh";
       // public const int UserIdGuest = 12;

        public const decimal MoneyShippingCost = 5000;
        //اگر مبلغ سندبازاریاب پدر کم تر از این باشد بقیه پدر ها سند نمی زند 
        //یعنی حداقل مبلغ سند زنی برای بازاریاب پدر
        public const decimal MinMonyParentMarketing = 500;
        public const int FK_CategoryBaseParent = 2;

        public const double MinParentMarketing = 0;
        public const double MaxParentMarketing = 80;



        public const int DivEmtiaz = 100000;

        public const int TehranIdInProvinceTbl = 8;







    }

    /// <summary>
    /// در واقع نام سر فصلهای حسابداری هستند
    /// </summary>
    public class AccountingTypes
    {
        public const string Payment_User = "Payment_User";//پرداخت کاربر
        public const int Bank_Account_Site_Value = 1362; // userId in DB
        public const string Persent_Marketing_seller = "Persent_Marketing_seller";//سند بازاریابی معرفی کننده
        public const string Persent_Marketing_PresenterBusinessOwner = "Persent_Marketing_PresenterBusinessOwner";//true
        public const string Persent_Marketing_Site = "Persent_Marketing_Site";// درصد بازاریابی سایت
        public const int Persent_Marketing_Site_value = 13;
        public const string Persent_Marketing_Parent = "Persent_Marketing_Parent";//سهم بازاریابی از بازایاب زیر شاخه
        public const string Buy_Invoice = "Buy_Invoice";//تحویل سفارش
        public const string GiveMarketingWithBusinessOwner = "GiveMarketingWithBusinessOwner";//درصد بازاریابی
        public const string Remaining_Marketing = "Remaining_Marketing";// سند باقی مانده مبلغ بازاریابی
        public const string Request_Balance_Money = "Request_Balance_Money";//مبلغ انتقال وجه
        public const int Remaining_Marketing_value = 16;
        public const string Money_Shipping_Cost = "Money_Shipping_Cost";// هزینه انتقال وجه
        public const int Money_Shipping_Cost_value = 19;
        //public const string Payment_BusinessOwner_Eduction_cost = "Payment_BusinessOwner_Eduction_cost";//هزینه آموزش فروشنده
        //public const int Payment_BusinessOwner_Eduction_cost_value = 242;

        //Payment_User


        //  public const  string daryaft_hazineh_Amozesh_site ="daryaft_hazineh_Amozesh_site";
        public const int daryaft_hazineh_Amozesh_site_value = 242;

        // public const string daryaft_hazineh_Amozesh_bazaryab = "daryaft_hazineh_Amozesh_site";
        //public const string pardakht_hazineh_amozesh = "pardakht_hazineh_amozesh";
    }

    public class InvoiceNamesDefault
    {
        public const string Money_Shipping_Cost_BusinessOwner = "Money_Shipping_Cost_BusinessOwner";
        public const int Money_Shipping_Cost_BusinessOwner_Id = 8;
    }

    /// <summary>
    /// نام اکشنهای اصلی
    /// </summary>
    public class ActionNames
    {
        public const string SaveChange = "SaveChange";
        public const string GetRow = "GetRow";
        public const string DeleteRow = "DeleteRow";
        public const string GridFK = "GridFK";
        public const string Login = "Login";
    }

    /// <summary>
    /// نام انتیتی ها 
    /// </summary>
    public class EntityNames
    {
        public const string Product = "Product";

    }
    public class UpdateTagIds
    {
        public const string Grid = "Grid";
        public const string EditorRow = "EditorRow";
        public const string Filter = "Filter";
    }

    public class RolesSystem
    {
        public const int AdminUserId = 218;
        public const string Admin = "Admin";
        public const int AdminValue = 1;
        public const string Admin2 = "Admin2";
        public const int Admin2Value = 2;
        public const string BusinessOwner = "BusinessOwner";
        public const int BusinessOwnerValue = 3;
        public const string Marketer = "Marketer";
        public const int MarketerValue = 4;
        public const string User = "User";
        public const int UserValue = 5;
        public const string Guest = "Guest";
        public const int GuestValue = 6;
    }

    public class ScenarioOrViewName
    {
        public const string AdminView = "AdminView";
        public const string UserView = "UserView";
        public const string UserView2 = "UserView2";
        public const string EditotrRowPartial = "EditotrRowPartial";
        public const string Grid = "Grid";
        public const string ActionResult = "ActionResult";
        public const string Propaganda = "Propaganda";
    }

    public class SearchOptions
    {

        public const string Product = "Product";
        public const int ProductValue = 1;
        public const string BusinessOwne = "BusinessOwne";
        public const int BusinessOwneValue = 2;
        public const string Buyers = "Buyers";
        public const int BuyersValue = 3;
        public const string Marketer = "Marketer";
        public const int MarketerValue = 4;


    }


    public static class Paths
    {
        //public static string DomainName = "79.175.165.228";
        public static string AdminUploadPath = "Images\\AdminUploadImg\\";
        public static string UserUploadFiles = "Images\\UserUploadFiles\\";
        public static string AdminUploadUri = "/Images/AdminUploadImg/";
        public static string AdminSiteFilePath = "Images\\AdminSiteFile\\";
        public static string DefaultPicBussinessOwner = "/Images/DefaultPicBussinessOwner.jpg";
        public static string DefaultPicMarketer = "/Images/DefaultPicMarketer.jpg";
        public static string DefaultProductPic = "/Images/DefaultProductPic.jpg";
        public static string AdminSiteFileUri = "/Images/AdminSiteFile/";
        public static string ErrorLogFileName = "ErrorLogShopping.txt";
        public static string LogPath = "LogPath\\";


    }

    public class EnumListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }



}
