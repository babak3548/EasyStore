using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using DataLayer.EF;
using DataLayer.Contract;
using Microsoft.AspNetCore.Http;

namespace UILayer.Maper
{
    public partial class BusinessOwnerMaper 
    {
        //partial void PartialMethodFormCollectionToEntity(ref FormCollection formCollection,ref Bridge_Invoice_Product bridge_Invoice_Product)
        //{
        //    throw new NotImplementedException();
        //}
        partial void PartialMethodFormCollectionToEntity(ref FormCollection formCollection, ref BusinessOwner businessOwner)
        {
                //throw new NotImplementedException();
        }
    }
}