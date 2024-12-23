using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.EF;

namespace ServiceLayer
{
    public partial class ProvinceService : BaseService<Province>
    {
        public ProvinceService(OnlineShopping onlineShopping):base(onlineShopping )
        {

        }
        public List<Province> ProvinceShippingBusinessOwner(int FK_BusinessOwner,int BusinessOwner_FK_Province)
        {
            List<Province> ProvinceShipping =Find(p => p.BridgeProvinceBusinessOwner.Any(b => b.FkBusinessOwner == FK_BusinessOwner)).ToList();

            if (ProvinceShipping.Count <= 0)
            { ProvinceShipping.Add(FirstOrDefault(p => p.Id == BusinessOwner_FK_Province)); }
            return ProvinceShipping;
        }
    }
}
