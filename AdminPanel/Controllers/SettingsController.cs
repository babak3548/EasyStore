using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.EF;
using ServiceLayer;

namespace AdminPanel.Controllers
{
    public class SettingsController : Controller
    {
        private readonly OnlineShopping _context;
        private SettingService _settinsService;
        public SettingsController(OnlineShopping context, SettingService settinsService)
        {
            _context = context;
            _settinsService = settinsService;
        }

        // GET: Settings
        public async Task<IActionResult> Index()
        {
            var onlineShopping = _context.Setting.Include(s => s.FkCategorySettingNavigation);
            return View(await onlineShopping.ToListAsync());
        }

        // GET: Settings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var setting = await _context.Setting
                .Include(s => s.FkCategorySettingNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (setting == null)
            {
                return NotFound();
            }

            return View(setting);
        }

        // GET: Settings/Create
        //public IActionResult Create()
        //{
        //    ViewData["FkCategorySetting"] = new SelectList(_context.CategorySetting, "Id", "Id");
        //    return View();
        //}

        //// POST: Settings/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,GroupName,Value,FkCategorySetting")] Setting setting)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(setting);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["FkCategorySetting"] = new SelectList(_context.CategorySetting, "Id", "Id", setting.FkCategorySetting);
        //    return View(setting);
        //}

        // GET: Settings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var setting = await _context.Setting.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }
            ViewData["FkCategorySetting"] = new SelectList(_context.CategorySetting, "Id", "Id", setting.FkCategorySetting);
            return View(setting);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GroupName,Value,FkCategorySetting")] Setting setting)
        {
            if (id != setting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(setting);
                    _settinsService.ClearCache();
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingExists(setting.Id))
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
            ViewData["FkCategorySetting"] = new SelectList(_context.CategorySetting, "Id", "Id", setting.FkCategorySetting);
            return View(setting);
        }

        //// GET: Settings/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var setting = await _context.Setting
        //        .Include(s => s.FkCategorySettingNavigation)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (setting == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(setting);
        //}

        //// POST: Settings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var setting = await _context.Setting.FindAsync(id);
        //    _context.Setting.Remove(setting);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool SettingExists(int id)
        {
            return _context.Setting.Any(e => e.Id == id);
        }
    }
}
