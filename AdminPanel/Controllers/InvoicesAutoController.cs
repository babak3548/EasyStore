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
    public class InvoicesAutoController : Controller
    {
        private readonly OnlineShopping _context;

        public InvoicesAutoController(OnlineShopping context)
        {
            _context = context;
        }

        // GET: InvoicesAuto
        public async Task<IActionResult> Index()
        {
            var onlineShopping = _context.Invoice.Include(i => i.FkBusinessOwnerNavigation).Include(i => i.FkMarketerNavigation).Include(i => i.FkProvinceNavigation).Include(i => i.FkUserNavigation);
            return View(await onlineShopping.ToListAsync());
        }

        // GET: InvoicesAuto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.FkBusinessOwnerNavigation)
                .Include(i => i.FkMarketerNavigation)
                .Include(i => i.FkProvinceNavigation)
                .Include(i => i.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: InvoicesAuto/Create
        public IActionResult Create()
        {
            ViewData["FkBusinessOwner"] = new SelectList(_context.BusinessOwner, "Id", "Name");
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name");
            ViewData["FkProvince"] = new SelectList(_context.Province, "Id", "Id");
            ViewData["FkUser"] = new SelectList(_context.User, "Id", "Mobile");
            return View();
        }

        // POST: InvoicesAuto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegisterDate,UpdateDate,SendingDate,ProcessingDays,Status,TotalSumProductPrice,PaymentToCountinue,ShippingCost,Discount,Vat,RejectedCost,FkUser,FkBusinessOwner,PaymentType,NoteForBusinessOwner,NoteForUser,FkMarketer,TimeBankPayInfo,PaymentBankCode,TransctionRefrenceId,TracingShippingNumber,ShippingCompany,CommentForBusinessman,HistoryStateAndDescription,FkProvince,DeliveryAddress,DeliveryCityName,DeliveryName,DeliveryLastName,DeliveryPostCode,DeliveryTel,DeliveryMobile,DeliveryCompanyName")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkBusinessOwner"] = new SelectList(_context.BusinessOwner, "Id", "Name", invoice.FkBusinessOwner);
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name", invoice.FkMarketer);
            ViewData["FkProvince"] = new SelectList(_context.Province, "Id", "Id", invoice.FkProvince);
            ViewData["FkUser"] = new SelectList(_context.User, "Id", "Mobile", invoice.FkUser);
            return View(invoice);
        }

        // GET: InvoicesAuto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["FkBusinessOwner"] = new SelectList(_context.BusinessOwner, "Id", "Name", invoice.FkBusinessOwner);
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name", invoice.FkMarketer);
            ViewData["FkProvince"] = new SelectList(_context.Province, "Id", "Id", invoice.FkProvince);
            ViewData["FkUser"] = new SelectList(_context.User, "Id", "Mobile", invoice.FkUser);
            return View(invoice);
        }

        // POST: InvoicesAuto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegisterDate,UpdateDate,SendingDate,ProcessingDays,Status,TotalSumProductPrice,PaymentToCountinue,ShippingCost,Discount,Vat,RejectedCost,FkUser,FkBusinessOwner,PaymentType,NoteForBusinessOwner,NoteForUser,FkMarketer,TimeBankPayInfo,PaymentBankCode,TransctionRefrenceId,TracingShippingNumber,ShippingCompany,CommentForBusinessman,HistoryStateAndDescription,FkProvince,DeliveryAddress,DeliveryCityName,DeliveryName,DeliveryLastName,DeliveryPostCode,DeliveryTel,DeliveryMobile,DeliveryCompanyName")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            ViewData["FkBusinessOwner"] = new SelectList(_context.BusinessOwner, "Id", "Name", invoice.FkBusinessOwner);
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name", invoice.FkMarketer);
            ViewData["FkProvince"] = new SelectList(_context.Province, "Id", "Id", invoice.FkProvince);
            ViewData["FkUser"] = new SelectList(_context.User, "Id", "Mobile", invoice.FkUser);
            return View(invoice);
        }

        // GET: InvoicesAuto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.FkBusinessOwnerNavigation)
                .Include(i => i.FkMarketerNavigation)
                .Include(i => i.FkProvinceNavigation)
                .Include(i => i.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: InvoicesAuto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.Id == id);
        }
    }
}
