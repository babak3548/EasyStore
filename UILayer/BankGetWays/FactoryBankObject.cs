using DataLayer.EF;
using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.BankGetWays
{
    public class FactoryBankObject
    {
        public BaseBank BankObject { get; set; }
       
        public FactoryBankObject()
        {}

        //public BaseBank FactoryBankObjectMethod(Banks bankCode, OnlineShopping objectContext)
        //{
        //    if (bankCode == Banks.Meli)
        //        BankObject = new MelatController(objectContext);
        //    else if (bankCode == Banks.Pasargad)
        //        BankObject = new PasargadController(new DataLayer.EF.Invoice() , objectContext);
        //    else
        //        BankObject = null;

        //         return BankObject;
        //}

    }
}