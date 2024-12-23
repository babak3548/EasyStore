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
    public class CategoryAdminController : BaseAdminController<Category,CategoryService,CategoryMaper>//,ICURDOperation<Category>
    {

        public CategoryAdminController( OnlineShopping _onlineShopping)
            : base( _onlineShopping)
        {
            _entity = new Category();
            _service = new CategoryService(objectContext);
            _maper = new CategoryMaper();
        }
        public virtual ActionResult Delete(int Id)
        {
            var entity = _service.FirstOrDefault(b => b.Id == Id);
            _service.Delete(entity);
            _service.SaveAllChengeOrAllReject(true);
            return GridView();
        }

        public  ActionResult Register(Category entity, object obj = null)
        {

            _service.Add(entity);
            _service.SaveAllChengeOrAllReject(true);

            string categoryIdsParents = ",";
            _service.GetParentsCategoryIdsInString(entity.Id, ref categoryIdsParents);
            entity.IdsParent =categoryIdsParents;

            _service.SaveAllChengeOrAllReject(true);
            _service.FillCategory();
            return RedirectToAction( "GridView");
        }

        public  ActionResult Edit(Category entity, int Id)
        {
            _entity = _service.GetByProp("Id", Id).FirstOrDefault();
            _maper.EntityToEntity(entity, _entity);

            string categoryIdsParents=",";
            _service.GetParentsCategoryIdsInString(_entity.Id,ref categoryIdsParents);
            _entity.IdsParent = categoryIdsParents;

            _service.SaveAllChengeOrAllReject(true);
            _service.FillCategory();
            return RedirectToAction("GridView");
        }


        /*
public ActionResult ConvertTest()
{

foreach (var entity in _service.Find(c=>c.Id != 2))
{
string categoryIdsParents = ",";
_service.GetParentsCategoryIdsInString(entity.Id, ref categoryIdsParents);
entity.Ids_Parent = categoryIdsParents;


}
_service.SaveAllChengeOrAllReject(true);   

return RedirectToAction("GridView");
}
*/
    }
}
