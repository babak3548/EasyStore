using DataLayer;
using DataLayer.Enums;
using ServiceLayer;
using UILayer.Controllers;
using Utility;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using System.Web;
using DataLayer.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UILayer.Areas.Adminstration.Controllers
{
    public class BaseAdminController<TEntity, TRepository,TMaper> : Base0Controller
        where TEntity : class
        where TRepository : BaseService<TEntity>
        where TMaper : ServiceLayer.Maper.BaseMaper<TEntity>
    {
        protected OnlineShopping objectContext;
        protected TEntity _entity;
        protected TRepository _service;
        protected TMaper _maper;

  
        //
        // GET: /Adminstration/BaseAdmin/
        public BaseAdminController( OnlineShopping _onlineShopping ) :base("Admin"  ,  _onlineShopping)
        {

            objectContext = new OnlineShopping();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            ValidateAccessAdminOnly();
        }
     
        public virtual ActionResult RegisterView(object obj = null)
        {
            
            return View(_entity);
        }

        public virtual ActionResult Register(TEntity entity, object obj = null)
        {
          
            _service.Add(entity);
            _service.SaveAllChengeOrAllReject(true);
            return GridView();
        }

        public virtual ActionResult GridView()
        {
          
          return View("GridView",_service.GetAll());
        }



        public virtual ActionResult EditView(int Id)
        {
         
            return View(_service.GetByProp("Id", Id).FirstOrDefault());

        }

        public virtual ActionResult Edit(TEntity entity, int Id)
        {
           
            _entity = _service.GetByProp("Id", Id).FirstOrDefault();
            _maper.EntityToEntity(entity, _entity);
            _service.SaveAllChengeOrAllReject(true);
            return RedirectToAction( "GridView");
        }

        public ActionResult Search(TEntity entity)
        {
            var filterDic = new Dictionary<string, object>();
            var properties = typeof(TEntity).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {  ///تشخیص می دهد که پروپرتی های اضافه شده نوع های اولیه باشند ذر واقع همان فیلدهای اضافه شده در دیتابیس باشند 
                if (property.CanRead && (property.PropertyType.IsPrimitive | property.PropertyType.FullName.Contains("System.String")
                    | property.PropertyType.FullName.Contains("System.Decimal")
                    | property.PropertyType.FullName.Contains("System.Nullable")) && (property.GetValue(entity, null) != null &&
                    property.GetValue(entity, null).ToString() != "0" && property.GetValue(entity, null).ToString() != " ")
                   )
                    filterDic.Add(property.Name, property.GetValue(entity, null));
            }
            return View("GridView", _service.GetByFilterAndContians(filterDic));
        }


    }
}
