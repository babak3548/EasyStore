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
    public partial class MarketerService : BaseService<Marketer>
    {
        Marketer _marketer = new Marketer();
      
        public MarketerService(OnlineShopping OnlineShopping)
            : base(OnlineShopping)
        {
        }



        public IEnumerable<object> SrchMarketerNamTypeActivite(string searchValue, short pageSize, short pageNo, out int count)
        {
            IQueryable<Marketer> query = _OnlineShopping.Marketer.Where(p => p.Active != false)
    // .OrderByDescending(o => o.FkCategory == fK_Category)
    .OrderBy(o => o.Id);

            if (!string.IsNullOrWhiteSpace(searchValue))
                query = query.Where(p=> p.Name.Contains(searchValue)
            || p.WordKey.Contains(searchValue) );


            count = query.Count();
            return query.Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();
        }
    }
}
