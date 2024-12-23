using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.EF;
using DataLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;
using Utility;

namespace AdminPanel.Controllers
{
    public class InvoiceController : BaseController
    {
        private InvoiceService _invoiceService;
        private AccountingService _AccountingService;
        private readonly OnlineShopping _context;

       

        public InvoiceController(InvoiceService invoiceService , AccountingService accountingService , OnlineShopping context)
        {
            _invoiceService = invoiceService;
            _AccountingService = accountingService;
            _context = context;
        }
        public IActionResult Index(int? pageIndex,string invoiceId, string userEmail,OrderByInvoice orderByInvoice, int? invoiceStatus)
        {

            if(pageIndex == null || pageIndex <= 0)
            {
                pageIndex = 1;
            }

            if (string.IsNullOrWhiteSpace(invoiceId))
            {
                invoiceId = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                userEmail = string.Empty;
            }
            if(invoiceStatus == null)
            {
                invoiceStatus = -1;
            }
            ViewData["invoiceId"] = invoiceId;
            ViewData["userEmail"] = userEmail;
            ViewData["invoiceStatus"] = invoiceStatus.Value;
            ViewData["selectedOrderBy"] = orderByInvoice;
            
            //orderByInvoice
            ViewBag.orderByInvoice = new SelectList(EnumUtility.EnumToList<OrderByInvoice>(), "Id", "Name",(int) orderByInvoice);

            var data = _invoiceService.GetAll(
                Pagination.Create(pageIndex.Value),
                (int.TryParse(invoiceId, out int x) ? (int?)x: null),
                userEmail,
                invoiceStatus < 0 ? null : invoiceStatus , orderByInvoice);
            return View(data);
        }

        public IActionResult Item(int id)
        {
            var item = _invoiceService.GetbyId(id);
            if (item == null)
                return NotFound();
            ViewData["accounting"] = _AccountingService.GetTansctionsUser(item.FkUser);
            ViewData["Balance"] = _AccountingService.AccountBalance(item.FkUser);
            return View(item);
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

            ViewData["ShippingCompany"] = new SelectList(EnumUtility.EnumToList<ShippingCompanies>(), "Id", "Name", (int)invoice.ShippingCompany);
            ViewData["PaymentType"] = new SelectList(EnumUtility.EnumToList<PaymentType>(), "Id", "Name", (int)invoice.PaymentType);
            ViewData["Status"] = new SelectList(EnumUtility.EnumToList<InvoiceStatus>(), "Id", "Name", (int)invoice.Status);

            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,int Id ,DateTime SendingDate , PaymentType PaymentType 
          , string TracingShippingNumber , string NoteForUser , InvoiceStatus Status , ShippingCompanies ShippingCompany)
        {
            if (id != Id)
            {
                return NotFound();
            }
            var existInv = _context.Invoice.FirstOrDefault(i => i.Id == Id);
            try
                {
         
                existInv.SendingDate = SendingDate;
                existInv.UpdateDate = DateTime.Now;
                existInv.PaymentType = PaymentType;
                existInv.TracingShippingNumber = TracingShippingNumber;
                existInv.NoteForUser = NoteForUser;
                existInv.Status = Status;
                existInv.ShippingCompany = ShippingCompany;
                //آخرین سطر باید باشد
                InvoiceService.AddHistory(existInv , existInv.Status);
               
                await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["FkBusinessOwner"] = new SelectList(_context.BusinessOwner, "Id", "Name", existInv.FkBusinessOwner);
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name", existInv.FkMarketer);
            ViewData["FkProvince"] = new SelectList(_context.Province, "Id", "Id", existInv.FkProvince);
            ViewData["FkUser"] = new SelectList(_context.User, "Id", "Mobile", existInv.FkUser);
            return View(existInv);
        }
        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.Id == id);
        }
        public IActionResult Reject(int id)
        {
            var item = _invoiceService.FirstOrDefault(a => a.Id == id);
            if (item == null)
                return NotFound();
            item.Status = DataLayer.Enums.InvoiceStatus.Rejected;
            _invoiceService.Update(item);
            _invoiceService.SaveChanges();

            return RedirectToAction("Item",new { id= id});
        }
    }
}