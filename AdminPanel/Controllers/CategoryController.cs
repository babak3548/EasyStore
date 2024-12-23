using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.EF;
using ServiceLayer;
using Utility;

namespace AdminPanel.Controllers
{
    public class CategoryController : BaseController
    {
        private CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Category
        public IActionResult Index(int? pageIndex)
        {
            if (!pageIndex.HasValue || pageIndex <= 0)
                pageIndex = 1;

            var result = _categoryService.GetAll(Pagination.Create(pageIndex.Value));
            return View(result);
        }

       
        public IActionResult Create()
        {
            ViewData["FkCategory"] =new SelectList(GetCategories(),"Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            _categoryService.Save(category);
            ViewData["FkCategory"] = new SelectList(GetCategories(), "Value", "Text");
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetCatrgory(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewData["FkCategory"] = new SelectList(GetCategories(), "Value", "Text");
            return View(category);
        }

      

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.Save(category);
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkCategory"] = new SelectList(GetCategories(), "Value", "Text",category.FkCategory);
            return View(category);
        }
        public IActionResult Delete(int id)
        {

            _categoryService.DeleteCategory(id);

            return RedirectToAction("Index");
        }

        private IEnumerable<SelectListItem> GetCategories()
        {
            var allData = _categoryService.GetAll().ToList();
            
            var items = allData.Select(a => new SelectListItem($"{(a.FkCategoryNavigation == null ? a.Name : a.FkCategoryNavigation.Name + "->" + a.Name)}", a.Id.ToString()));
            return items;
        }
    }
}
