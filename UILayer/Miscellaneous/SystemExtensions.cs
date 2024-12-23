
using DataLayer.EF;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataLayer.Models;

namespace System
{
    public static class SessionExtensions
    {
        public static DataLayer.Contract.UserContract ViewBagUser(this object objViewBag)
        {
            if (objViewBag == null) return null;
            return (DataLayer.Contract.UserContract)objViewBag;
        }
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            if (value == null)
            {
             //   session.SetString(key, null);
                session.Remove(key);
            }
            string valueStr = JsonSerializer.Serialize(value);
            session.SetString(key, valueStr);
        }
        public static void SessionRemove(this ISession session, string key)
        {
            session.Remove(key);
        }
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return  value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
        public static List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ToSelectListItems(this IEnumerable<DataLayer.Contract.SelectListItem> selectListItems)
        {
            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> res = new List<SelectListItem>();
            foreach (var item in selectListItems)
            {
                Microsoft.AspNetCore.Mvc.Rendering.SelectListItem newItem = new SelectListItem();
                newItem.Text = item.Text;
                newItem.Value = item.Value;
                newItem.Selected = item.Selected;

                res.Add(newItem);
            }
            return res;
        }

        public static string OldPrice(this Product product)
        {
            if (product.BeforDiscountPrice > product.Price)
            {
                return $"<span class='old_price'>{product.BeforDiscountPrice.decimalToDigMony()}</span>";
            }
            else
            {
                return "";
            }
        }
        public static string OldPrice(this PromotionProductModel promotionProductModel)
        {
            if (promotionProductModel.BeforDiscountPrice > promotionProductModel.Price)
            {
                return $"<span class='old_price'>{promotionProductModel.BeforDiscountPrice.decimalToDigMony()}</span>";
            }
            else
            {
                return "";
            }
        }

    }

}
