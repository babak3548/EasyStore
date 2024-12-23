using DataLayer;
using DataLayer.Enums;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using Utility;
using DataLayer.EF;
namespace ServiceLayer
{
      partial class BusinessOwnerService : BaseService<BusinessOwner>
    {

             //     Bridge_Invoice_Product _bridge_Invoice_Product = new Bridge_Invoice_Product();
      //  Bridge_Invoice_ProductService _bridge_Invoice_ProductService ;
      //  BusinessOwnerService _businessOwnerService;

          BusinessOwner _businessOwner = new BusinessOwner();
        
          public BusinessOwnerService(OnlineShopping OnlineShopping)
              : base(OnlineShopping)
          {
             
          }







          public IEnumerable<url> CreateSiteMapList(string lastModifiedProd)
          {
              var resultUrl = GetAll().Where(b => b.Active != false & b.Product.Any(p=>p.Active!=false)).OrderBy(b => b.Id).Select(b => new url
              {
                  changefreq = "monthly",
                  lastmod = "",
                  priority = "0.5",
                  loc = AppSetting.DomainName  +"/" + b.Name
              });

              return resultUrl;
          }

        public IEnumerable<object> SrchBusOwnNamTypeActivite(string searchValue, short pageSize, short pageNo, out int count)
        {
            IQueryable<BusinessOwner> query = _OnlineShopping.BusinessOwner.Where(p => p.Active != false)
    // .OrderByDescending(o => o.FkCategory == fK_Category)
    .OrderBy(o => o.Id);

            if (!string.IsNullOrWhiteSpace(searchValue))
                query = query.Where(p => p.Name.Contains(searchValue) || p.Discription.Contains(searchValue)
            || p.WordKey.Contains(searchValue) || p.Discription.Contains(searchValue));

            count = query.Count();
            return query.Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();


        }

        private object Find(Func<BusinessOwner, bool> func)
          {
              throw new NotImplementedException();
          }


         
    }
}
