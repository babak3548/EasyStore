using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace DataLayer.Models
{
    //
    public class SearchModel
    {
        Sorting sorting=Sorting.RankShow;
        public Sorting Sorting { get { return sorting; } set { sorting = value; } }
        short pageNo=1;
        public short PageNo { get { return pageNo; } set { pageNo = value; } }

        string q="";
        public string query { get { return q; } set { q = value; } }
        decimal price_min=0;
        public decimal Price_min { get { return price_min; } set { price_min = value; } }
        decimal price_max=500000000;
        public decimal Price_max { get { return price_max; } set { price_max = value; } }
        string Category= DefualtValue.AllCategory;
        public string category { get { return Category; } set { Category = value; } }
        int fk_Marketer = 0;
        public int Fk_Marketer { get { return fk_Marketer; } set { fk_Marketer = value; } }
        PromotionTypes promotionTypes = PromotionTypes.NoSetPromotionType;
        public PromotionTypes PromotionType { get { return promotionTypes; } set { promotionTypes = value; } }


        public int pageSize=ConstSetting.PageSize ;
        public int PageSize { get { return pageSize; } set { pageSize = value; } }
    }
}
