using DataLayer;
using DataLayer.EF;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.Maper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;


namespace UILayer.Areas.Adminstration.Controllers
{
    public class BusinessOwnerAdminController : BaseAdminController<BusinessOwner, BusinessOwnerService, BusinessOwnerMaper>//,ICURDOperation<Category>
    {

        public BusinessOwnerAdminController( OnlineShopping _onlineShopping)
            : base( _onlineShopping)
        {
            _entity = new BusinessOwner();
            _service = new BusinessOwnerService(objectContext);
            _maper = new BusinessOwnerMaper();
        }
        public ActionResult test()
        {
            return View();
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
