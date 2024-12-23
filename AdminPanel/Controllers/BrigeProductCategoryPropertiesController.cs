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
    public class BrigeProductCategoryPropertiesController : Controller
    {
        private readonly OnlineShopping _context;

        public BrigeProductCategoryPropertiesController(OnlineShopping context)
        {
            _context = context;
        }

        // GET: BrigeProductCategoryProperties
        public async Task<IActionResult> Index()
        {
            var onlineShopping = _context.BrigeProductCategories.Include(b => b.CategoryProperty).Include(b => b.Product);
            return View(await onlineShopping.ToListAsync());
        }

        // GET: BrigeProductCategoryProperties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brigeProductCategoryProperty = await _context.BrigeProductCategories
                .Include(b => b.CategoryProperty)
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brigeProductCategoryProperty == null)
            {
                return NotFound();
            }

            return View(brigeProductCategoryProperty);
        }

        // GET: BrigeProductCategoryProperties/Create
        public IActionResult Create()
        {
            ViewData["FkCategoryProperty"] = new SelectList(_context.CategoryProperties, "Id", "Name");
            ViewData["FkProduct"] = new SelectList(_context.Product, "Id", "Name");
            return View();
        }

        // POST: BrigeProductCategoryProperties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,value,FkCategoryProperty,FkProduct")] BrigeProductCategoryProperty brigeProductCategoryProperty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brigeProductCategoryProperty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkCategoryProperty"] = new SelectList(_context.CategoryProperties, "Id", "Name", brigeProductCategoryProperty.FkCategoryProperty);
            ViewData["FkProduct"] = new SelectList(_context.Product, "Id", "Brand", brigeProductCategoryProperty.FkProduct);
            return View(brigeProductCategoryProperty);
        }

        // GET: BrigeProductCategoryProperties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brigeProductCategoryProperty = await _context.BrigeProductCategories.FindAsync(id);
            if (brigeProductCategoryProperty == null)
            {
                return NotFound();
            }
            ViewData["FkCategoryProperty"] = new SelectList(_context.CategoryProperties, "Id", "Name", brigeProductCategoryProperty.FkCategoryProperty);
            ViewData["FkProduct"] = new SelectList(_context.Product, "Id", "Brand", brigeProductCategoryProperty.FkProduct);
            return View(brigeProductCategoryProperty);
        }

        // POST: BrigeProductCategoryProperties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,value,FkCategoryProperty,FkProduct")] BrigeProductCategoryProperty brigeProductCategoryProperty)
        {
            if (id != brigeProductCategoryProperty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brigeProductCategoryProperty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrigeProductCategoryPropertyExists(brigeProductCategoryProperty.Id))
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
            ViewData["FkCategoryProperty"] = new SelectList(_context.CategoryProperties, "Id", "Name", brigeProductCategoryProperty.FkCategoryProperty);
            ViewData["FkProduct"] = new SelectList(_context.Product, "Id", "Brand", brigeProductCategoryProperty.FkProduct);
            return View(brigeProductCategoryProperty);
        }

        // GET: BrigeProductCategoryProperties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brigeProductCategoryProperty = await _context.BrigeProductCategories
                .Include(b => b.CategoryProperty)
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brigeProductCategoryProperty == null)
            {
                return NotFound();
            }

            return View(brigeProductCategoryProperty);
        }

        // POST: BrigeProductCategoryProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brigeProductCategoryProperty = await _context.BrigeProductCategories.FindAsync(id);
            _context.BrigeProductCategories.Remove(brigeProductCategoryProperty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrigeProductCategoryPropertyExists(int id)
        {
            return _context.BrigeProductCategories.Any(e => e.Id == id);
        }
    }
}
