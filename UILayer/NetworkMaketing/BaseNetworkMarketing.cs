using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataLayer.Contract;
using DataLayer.Enums;
using UILayer.Maper;
using ServiceLayer;
using UILayer.Miscellaneous;
using DataLayer;

using Utility;

using UILayer.Controllers;
using DataLayer.EF;

namespace UILayer.NetworkMaketing
{

    public class BaseNetworkMarketingController : Base0Controller
    {
       protected OnlineShopping objectContextNetWorkM;
        public BaseNetworkMarketingController(string entityName, OnlineShopping _onlineShopping)
            : base(entityName,  _onlineShopping)
	{
            objectContextNetWorkM = new OnlineShopping();
	}
      
       
    }
}
        

               