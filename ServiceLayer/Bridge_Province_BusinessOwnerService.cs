using DataLayer;
using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.EF;
//namespace ServiceLayer
//{
//    class Bridge_Province_BusinessOwner
//    {
namespace ServiceLayer
{
    public partial class Bridge_Province_BusinessOwnerService : BaseService<BridgeProvinceBusinessOwner>
    {
 
        public override bool CheckOwnerEntity(BridgeProvinceBusinessOwner entity, int FkUser)
        {
            if (entity.FkBusinessOwnerNavigation.FkUser == FkUser)
                return true;
            else return false;
        }

        public decimal CalcShipping(int ProvinceId, Invoice invoice)
        {
            double wightInvoice = CalcWightInvoice(invoice);
            return CalcShippingBySite(invoice.ShippingCompany, wightInvoice, (int)invoice.FkProvince, invoice);

            //if (CalcTypeShippings.CalcBySite == invoice.FkBusinessOwnerNavigation.CalcTypeShipping)
            //    return CalcShippingBySite(invoice.ShippingCompany, wightInvoice,(int)invoice.FkProvince,invoice);
            //else if (CalcTypeShippings.CalcByBussinessOwner == invoice.FkBusinessOwnerNavigation.CalcTypeShipping)
            //    return CalcShippingByBussinessOwner(ProvinceId, invoice, wightInvoice);
            //else
            //    return 0;
        }

        private decimal CalcShippingByBussinessOwner(int ProvinceId, Invoice invoice, double wightInvoice)
        {

            decimal moneyShiping = 0;

            var bridge_Province_BusinessOwner = invoice.FkBusinessOwnerNavigation.BridgeProvinceBusinessOwner.FirstOrDefault(b => b.FkProvince == ProvinceId);
            if (bridge_Province_BusinessOwner == null) return 0;
            if (invoice.TotalSumProductPrice > bridge_Province_BusinessOwner.FreeGratherThanMony) { moneyShiping = 0; }
            else
            {
                //این شرط از تقسیم بر صفر جلوگیری می کند
                if (bridge_Province_BusinessOwner.AnyXkg == 0) bridge_Province_BusinessOwner.AnyXkg = 1;
                moneyShiping = Math.Ceiling((decimal)wightInvoice / (decimal)bridge_Province_BusinessOwner.AnyXkg) * bridge_Province_BusinessOwner.AnyXkgmony;
                // ( Math.Ceiling(wightInvoice / bridge_Province_BusinessOwner.AnyXKG) );
            }
            return moneyShiping;
        }

        private decimal CalcShippingBySite(ShippingCompanies TypeShipping, double wightInvoice, int ProvinceId, Invoice invoice)
        {
            //if (TypeShipping == ShippingCompanies.Tepaxs)
            //    return CalcShippingByTepaxs(wightInvoice);
            //else if (TypeShipping == ShippingCompanies.PostIran)
            //    return CalcShippingByPost(wightInvoice);
            //else if (TypeShipping == ShippingCompanies.ShippingWithBusinessOwner)
            //    return CalcShippingByBusinessOwner(wightInvoice, invoice);
            //else if (TypeShipping == ShippingCompanies.Barbari)//همانند شرکت تیپاکس محاسبه می گردد
            //    return CalcShippingByTepaxs(wightInvoice);
            //else if (TypeShipping == ShippingCompanies.Terminal)//همانند شرکت تیپاکس محاسبه می گردد
            //    return CalcShippingByTepaxs(wightInvoice);//
            //else if (TypeShipping == ShippingCompanies.TPG)
            //    return CalcShippingByTPG(wightInvoice,invoice);
            //else if (TypeShipping == ShippingCompanies.Aramex)
            //    return CalcShippingByAramex(wightInvoice, invoice);
            //else
                return 0;
        }





        private static double CalcWightInvoice(Invoice invoice)
        {
            double wightInvoice = 0;
            foreach (var bridge_Invoice_Product in invoice.BridgeInvoiceProduct)
            {
                wightInvoice += bridge_Invoice_Product.FkProductNavigation.WightUnitBuyWithKg * bridge_Invoice_Product.Count;
            }
            return wightInvoice;
        }

        //محاسبه هزینه ارسال  تیپاکس
        private decimal CalcShippingByTepaxs(double wightInvoice)
        {
            return BaseShippingCalcerTotal(wightInvoice, 6000, 1000, 110000, 12000);
        }

        //محاسیه ارسال بسته، با پست پیشتاز درون استانی
        private decimal CalcShippingByPost(double wightInvoice)
        {
            // double remainWight = 0;
            wightInvoice = wightInvoice + 100;//وزن اضافه شده در بسته بندی
            if (wightInvoice <= 250) return 40000;
            else if (wightInvoice <= 500) return 48000;
            else if (wightInvoice <= 1000) return 58000;
            else if (wightInvoice <= 2000) return 77000;
            else
            {
                wightInvoice += 100;// وزن اضافه شده در بسته بندی بزرگتر
                return BaseShippingCalcer(wightInvoice, 2000, 1000, 77000, 17000);
            }
        }

        /// <summary>
        /// هزینه ارسال توسط پیک خود فروشنده
        /// </summary>
        /// <param name="wightInvoice"></param>
        /// <returns></returns>
        private decimal CalcShippingByBusinessOwner(double wightInvoice, Invoice invoice)
        {
            if (invoice.FkProvince == invoice.FkBusinessOwnerNavigation.FkProvince)//درون استانی
            {
                if (invoice.FkProvince == ConstSetting.TehranIdInProvinceTbl)
                    return 100000;//هزینه پیک داخل تهران
                else
                    return 50000;//هزینه پیک در استانهای دیگر
            }
            else// برون استانی بر حسب شرکت تیپاکس محاسبه می گردد
            {
                return CalcShippingByTepaxs(wightInvoice);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wightInvoice"></param>
        /// <returns></returns>
        private decimal CalcShippingByTPG(double wightInvoice, Invoice invoice)
        {
            if (invoice.FkProvince == invoice.FkBusinessOwnerNavigation.FkProvince)//درون استانی
            {
                return BaseShippingCalcerTotal(wightInvoice, 1000, 1000, 110000, 40000);
            }
            else// برون استانی 
            {
                return BaseShippingCalcerTotal(wightInvoice, 1000, 1000, 260000, 40000);
            }
        }

        /// <summary>
        /// آرامیکس درون استانی: زیر 3 کیلو گرم 10700 تومان، بین 3 تا 4 کیلو گرم 21000 تومان و بعد از اون به ازای هر 1 کیلوگرم بیشتر، 1000 تومان به هزینه حمل اضافه می گردد
        ///آرامیکس برون استانی: زیر 3 کیلو گرم 13700 تومان، بین 3 تا 4 کیلو گرم 22000 تومان و بعد از اون به ازای هر 1 کیلوگرم بیشتر، 3000 تومان به هزینه حمل اضافه می گردد
        /// </summary>
        /// <param name="wightInvoice"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        private decimal CalcShippingByAramex(double wightInvoice, Invoice invoice)
        {
            if (invoice.FkProvince == invoice.FkBusinessOwnerNavigation.FkProvince)//درون استانی
            {
                if (wightInvoice <= 3000) return 107000;
                else if (wightInvoice <= 4000) return 210000;
                else
                {
                    return BaseShippingCalcer(wightInvoice, 4000, 1000, 210000, 10000);
                }
            }
            else// برون استانی 
            {
                if (wightInvoice <= 3000) return 137000;
                else if (wightInvoice <= 4000) return 220000;
                else
                {
                    return BaseShippingCalcer(wightInvoice, 4000, 1000, 220000, 30000);
                }
            }
        }

        
        /// <summary>
        ///  به ازای هر وای گرم بیشتر، ام تومان به هزینه حمل اولیه اضافه می گردد
        /// </summary>
        /// <param name="wightInvoice">وزن سفارش</param>
        /// <param name="WightBase">وزن پایه</param>
        /// <param name="AnyWight">به ازای ایکس گرم اضافی</param>
        /// <param name="BaseShippingCast">مبلغ پایه</param>
        /// <param name="VaribleShippingCast">مبلغ برای هر 1000 گرم</param>
        /// <returns></returns>
        private static decimal BaseShippingCalcer(double wightInvoice, int WightBase, int AnyWight, decimal BaseShippingCast, decimal VaribleShippingCast)
        {
            double remainWight = wightInvoice - WightBase;//منهای وزن پایه 
            return BaseShippingCast + (Math.Ceiling((decimal)remainWight / AnyWight) * VaribleShippingCast);//1000به ازای
        }

        /// <summary>
        /// تا ایکس گرم ن تومان هزینه حمل می باشد و به ازای هر وای گرم بیشتر، ام تومان به هزینه حمل اضافه می گردد
        /// </summary>
        /// <param name="wightInvoice">وزن سفارش</param>
        /// <param name="WightBase">وزن پایه</param>
        /// <param name="AnyWight">به ازای ایکس گرم اضافی</param>
        /// <param name="BaseShippingCast">مبلغ پایه</param>
        /// <param name="VaribleShippingCast">مبلغ برای هر 1000 گرم</param>
        /// <returns></returns>
        private static decimal BaseShippingCalcerTotal(double wightInvoice, int WightBase, int AnyWight, decimal BaseShippingCast, decimal VaribleShippingCast)
        {
            if (wightInvoice <= WightBase) 
                return BaseShippingCast;
            else
            {
                return BaseShippingCalcer(wightInvoice, WightBase, AnyWight, BaseShippingCast,VaribleShippingCast);
            }
        }
    }
}
