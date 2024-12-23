﻿using DataLayer;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceLayer.Maper;
using DataLayer.EF;
using Microsoft.AspNetCore.Mvc;

namespace UILayer.Areas.Adminstration.Controllers
{
    public class ProductAdminController : BaseAdminController<Product, ProductService, ProductMaper>//,ICURDOperation<User>
    {
        //
        // GET: /Adminstration/User/
        public ProductAdminController( OnlineShopping _onlineShopping)
            : base( _onlineShopping)
        {
            _entity = new Product();
            _service = new ProductService(objectContext);
            _maper = new ProductMaper();
        }

        public virtual ActionResult Delete(int Id)
        {
            var entity = _service.FirstOrDefault(b => b.Id == Id);
            _service.Delete(entity);
            _service.SaveAllChengeOrAllReject(true);
            return GridView();
        }



    }
}
