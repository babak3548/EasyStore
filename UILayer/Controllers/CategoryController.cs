using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataLayer;
using ServiceLayer;
using UILayer.Maper;
using DataLayer.Contract;
using DataLayer.Enums;

using Utility;
using DataLayer.EF;
using Microsoft.AspNetCore.Mvc;

namespace UILayer.Controllers
{
    public partial class CategoryController :Base0Controller
        //BaseController<Category, CategoryService, CategoryMaper, CategoryContract>
    {
        //
        // GET: /Adminstration/Category/

        CategoryService _service;
        public CategoryController(OnlineShopping onlineShopping):base("",onlineShopping)
        {
            _service = new CategoryService(onlineShopping);
        }

        public ActionResult GetAjaxCatByFk_Cat(int id,int Value2)
        {
           // return "ffff";
           return View(_service.Find(c=>c.FkCategory==id));
        }


    }
}
