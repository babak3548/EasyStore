using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.EF;

namespace AdminPanel.Controllers
{
    public class CategoryPropertiesController : Controller
    {
        private readonly OnlineShopping _context;

        public CategoryPropertiesController(OnlineShopping context)
        {
            _context = context;
        }

        // GET: CategoryProperties
        public async Task<IActionResult> Index()
        {
            var onlineShopping = _context.CategoryProperties.Include(c => c.FkCategoryNavigation);
            return View(await onlineShopping.ToListAsync());
        }

        // GET: CategoryProperties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryProperty = await _context.CategoryProperties
                .Include(c => c.FkCategoryNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryProperty == null)
            {
                return NotFound();
            }

            return View(categoryProperty);
        }

        // GET: CategoryProperties/Create
        public IActionResult Create()
        {
            ViewData["FKCategory"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Discription,FKCategory")] CategoryProperty categoryProperty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryProperty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FKCategory"] = new SelectList(_context.Category, "Id", "Name", categoryProperty.FKCategory);
            return View(categoryProperty);
        }

        // GET: CategoryProperties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryProperty = await _context.CategoryProperties.FindAsync(id);
            if (categoryProperty == null)
            {
                return NotFound();
            }
            ViewData["FKCategory"] = new SelectList(_context.Category, "Id", "Name", categoryProperty.FKCategory);
            return View(categoryProperty);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Discription,FKCategory")] CategoryProperty categoryProperty)
        {
            if (id != categoryProperty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryProperty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryPropertyExists(categoryProperty.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FKCategory"] = new SelectList(_context.Category, "Id", "Name", categoryProperty.FKCategory);
            return View(categoryProperty);
        }

        // GET: CategoryProperties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryProperty = await _context.CategoryProperties
                .Include(c => c.FkCategoryNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryProperty == null)
            {
                return NotFound();
            }

            return View(categoryProperty);
        }

        // POST: CategoryProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryProperty = await _context.CategoryProperties.FindAsync(id);
            _context.CategoryProperties.Remove(categoryProperty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryPropertyExists(int id)
        {
            return _context.CategoryProperties.Any(e => e.Id == id);
        }
    }
}
