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
    public class MarketersController : BaseController
    {
        private readonly OnlineShopping _context;

        public MarketersController(OnlineShopping context)
        {
            _context = context;
        }

        // GET: Marketers
        public async Task<IActionResult> Index()
        {
            var onlineShopping = _context.Marketer.Include(m => m.FkMarketerNavigation).Include(m => m.FkUserNavigation);
            return View(await onlineShopping.ToListAsync());
        }

        // GET: Marketers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketer = await _context.Marketer
                .Include(m => m.FkMarketerNavigation)
                .Include(m => m.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marketer == null)
            {
                return NotFound();
            }

            return View(marketer);
        }

        // GET: Marketers/Create
        public IActionResult Create()
        {
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name");
            ViewData["FkUser"] = new SelectList(_context.User, "Id", "Name");
            return View();
        }

        // POST: Marketers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Specialty,WordKey,FkUser,FkMarketer,Image,WebSiteAddress,Tel,Active,Yahoo,Gmail,Skype,PaymentPorsantAmount")] Marketer marketer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marketer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name", marketer.FkMarketer);
            ViewData["FkUser"] = new SelectList(_context.User, "Id", "Name", marketer.FkUser);
            return View(marketer);
        }

        // GET: Marketers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketer = await _context.Marketer.FindAsync(id);
            if (marketer == null)
            {
                return NotFound();
            }
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name", marketer.FkMarketer);
            ViewData["FkUser"] = new SelectList(_context.User, "Id", "Name", marketer.FkUser);
            return View(marketer);
        }

        // POST: Marketers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Specialty,WordKey,FkUser,FkMarketer,Image,WebSiteAddress,Tel,Active,Yahoo,Gmail,Skype,PaymentPorsantAmount")] Marketer marketer)
        {
            if (id != marketer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marketer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarketerExists(marketer.Id))
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
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name", marketer.FkMarketer);
            ViewData["FkUser"] = new SelectList(_context.User, "Id", "Name", marketer.FkUser);
            return View(marketer);
        }

        // GET: Marketers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketer = await _context.Marketer
                .Include(m => m.FkMarketerNavigation)
                .Include(m => m.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marketer == null)
            {
                return NotFound();
            }

            return View(marketer);
        }

        // POST: Marketers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marketer = await _context.Marketer.FindAsync(id);
            _context.Marketer.Remove(marketer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarketerExists(int id)
        {
            return _context.Marketer.Any(e => e.Id == id);
        }
    }
}
