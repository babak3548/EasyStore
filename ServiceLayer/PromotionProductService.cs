using DataLayer.EF;
using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace ServiceLayer
{
  public  class PromotionProductService : BaseService<PromotionProduct>
    {
        public PromotionProductService(OnlineShopping onlineShopping):base(onlineShopping)
        {

        }

        public List<PromotionProduct> PromotionProductByTypes(params PromotionTypes[] topHomeSliders)
        {
            var list =_OnlineShopping.PromotionProduct.Where(p => topHomeSliders.Contains(p.PromotionType)).ToList();
            return list;
        }

        public IPagedList<PromotionProduct> GetAll(Pagination pagination, PromotionProduct searchObj )
        {
            IQueryable<PromotionProduct> query;
            query = _OnlineShopping.PromotionProduct;

            if (searchObj.FkCategory > 0)
            {
                query = query.Where( q=>q.FkCategory == searchObj.FkCategory);
            }
            if (searchObj.FkProduct > 0)
            {
                query = query.Where(q => q.FkProduct == searchObj.FkProduct);
            }
            if (searchObj.ExpireDateTime > new DateTime(2020,06,01))
            {
             var date=   searchObj.ExpireDateTime.Date;
                query = query.Where(q => q.ExpireDateTime.Date == date);
            }
            if (searchObj.Id > 0)
            {
                query = query.Where(q => q.Id == searchObj.Id);
            }
            if (searchObj.Order > 0)
            {
                query = query.Where(q => q.Order == searchObj.Order);
            }
            if ((int) searchObj.PromotionType > (int)PromotionTypes.NoSetPromotionType )
            {
                query = query.Where(q => q.PromotionType == searchObj.PromotionType);
            }
 
            query = query.OrderByDescending(o => o.Id);

            return PagedList<PromotionProduct>.Create(query, pagination);
        }
    }
}
