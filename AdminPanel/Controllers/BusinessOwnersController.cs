using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.EF;
using Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DataLayer;
using ServiceLayer;
using DataLayer.Enums;

namespace AdminPanel.Controllers
{
    public class BusinessOwnersController : BaseController
    {
        private readonly OnlineShopping _context;
        BusinessOwnerService _businessOwnerService;
        public BusinessOwnersController (BusinessOwnerService businessOwnerService ,  OnlineShopping context )
        {
            _context = context;
            _businessOwnerService = businessOwnerService;
        }

        // GET: BusinessOwners
        public async Task<IActionResult> Index()
        {
            var onlineShopping = _context.BusinessOwner.Include(b => b.FkMarketerNavigation).Include(b => b.FkProvinceNavigation).Include(b => b.FkUserNavigation);
            return View(await onlineShopping.ToListAsync());
        }

        // GET: BusinessOwners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessOwner = await _context.BusinessOwner
                .Include(b => b.FkMarketerNavigation)
                .Include(b => b.FkProvinceNavigation)
                .Include(b => b.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessOwner == null)
            {
                return NotFound();
            }

            return View(businessOwner);
        }

        // GET: BusinessOwners/Create
        public IActionResult Create()
        {
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name");
            ViewData["FkProvince"] = new SelectList(_context.Province, "Id", "Name");
            ViewData["FkUser"] = new SelectList(_context.User.Where(u => u.FkRole <= 4), "Id", "Name");
            ViewData["TypeSells"] = new SelectList(EnumUtility.EnumToList<TypeSell>(), "Id", "Name");
            ViewData["TypeShippingBussinessOwner"] = new SelectList(EnumUtility.EnumToList<TypeShippingBussinessOwner>(), "Id", "Name");
            return View();
        }

        // POST: BusinessOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (BusinessOwner businessOwner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(businessOwner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name", businessOwner.FkMarketer);
            ViewData["FkProvince"] = new SelectList(_context.Province, "Id", "Name", businessOwner.FkProvince);
            ViewData["FkUser"] = new SelectList(_context.User.Where(u => u.FkRole <= 4), "Id", "Name", businessOwner.FkUser);
            ViewData["TypeSells"] = new SelectList(EnumUtility.EnumToList<TypeSell>(), "Id", "Name", businessOwner.TypeSells);
            ViewData["TypeShippingBussinessOwner"] = new SelectList(EnumUtility.EnumToList<TypeShippingBussinessOwner>(), "Id", "Name", businessOwner.TypeShippingBussinessOwner);
            return View(businessOwner);
        }

        [AllowAnonymous]
        [HttpPost]
        [AcceptVerbs()]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, string inputFileName, int businessOwnerId)
        {
            if (files.Count == 0) return new RedirectResult(Url.Action("Item", new { id = businessOwnerId }) + "#imgArea");
            // return RedirectToAction("Item", new { id = productId  });
            string physicalPath = new AppSetting().ImagePathOtherFileServer;  // webRootPath.Replace("Upload\\UploadFileAdminSite", Paths.AdminSiteFilePath);

            var formFile = files.FirstOrDefault();

            var content = _businessOwnerService.FirstOrDefault(c => c.Id == businessOwnerId);
            if (content == null)
            {
                return NotFound();
            }
            // foreach (var formFile in files) 
            // {
            var fileInfo = new System.IO.FileInfo(formFile.FileName);

            if (formFile.Length > 30000000)
            {
                throw new MyException((byte)ExceptionType.validation, "validation", "سایز فایل نباید بزرگتر از 30 مگا بایت باشد و  نباشد وفرمت عکس جیپق باید باشد");
            }

            var filename = Guid.NewGuid() + fileInfo.Extension;

            if (formFile.Length > 0)
            {
                var filePath = physicalPath + "\\" + filename;
                using (var stream = System.IO.File.Create(filePath))
                {
                    await formFile.CopyToAsync(stream);
                }
            }

            if (inputFileName == "Image")
            {
                //   (physicalPath +  content.BanerImageAdress) .RemoveFile(); // عکس های قبلی رو حذف می کنیم
                content.Image = filename;
            }
            if (inputFileName == "DocumentFile")
            {
                //  (physicalPath + content.VideoAdress).RemoveFile();
                content.DocumentFile = filename;
            }

            _businessOwnerService.Update(content);
            _businessOwnerService.SaveChanges();

            return new RedirectResult(Url.Action("Edit", new { id = businessOwnerId }) );
            //RedirectToAction("Item" , new { id  = productId });
        }

        // GET: BusinessOwners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessOwner = await _context.BusinessOwner.FindAsync(id);
            if (businessOwner == null)
            {
                return NotFound();
            }
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name", businessOwner.FkMarketer);
            ViewData["FkProvince"] = new SelectList(_context.Province, "Id", "Name", businessOwner.FkProvince);
            ViewData["FkUser"] = new SelectList(_context.User.Where(u => u.FkRole <= 4), "Id", "Name", businessOwner.FkUser);
            // 
            ViewData["TypeSells"] = new SelectList(EnumUtility.EnumToList<TypeSell>(), "Id", "Name", businessOwner.TypeSells);
            ViewData["TypeShippingBussinessOwner"] = new SelectList(EnumUtility.EnumToList<TypeShippingBussinessOwner>(), "Id", "Name", businessOwner.TypeShippingBussinessOwner);

            return View(businessOwner);
        }

        // POST: BusinessOwners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  BusinessOwner businessOwner)
        {
            if (id != businessOwner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(businessOwner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessOwnerExists(businessOwner.Id))
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
            ViewData["FkMarketer"] = new SelectList(_context.Marketer, "Id", "Name", businessOwner.FkMarketer);
            ViewData["FkProvince"] = new SelectList(_context.Province, "Id", "Name", businessOwner.FkProvince);
            ViewData["FkUser"] = new SelectList(_context.User.Where(u=>u.FkRole <= 4), "Id", "Name", businessOwner.FkUser);

            ViewData["TypeSells"] = new SelectList(EnumUtility.EnumToList<TypeSell>(), "Id", "Name", businessOwner.TypeSells);
            ViewData["TypeShippingBussinessOwner"] = new SelectList(EnumUtility.EnumToList<TypeShippingBussinessOwner>(), "Id", "Name", businessOwner.TypeShippingBussinessOwner);
            return View(businessOwner);
        }

        // GET: BusinessOwners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessOwner = await _context.BusinessOwner
                .Include(b => b.FkMarketerNavigation)
                .Include(b => b.FkProvinceNavigation)
                .Include(b => b.FkUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessOwner == null)
            {
                return NotFound();
            }

            return View(businessOwner);
        }

        // POST: BusinessOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var businessOwner = await _context.BusinessOwner.FindAsync(id);
            _context.BusinessOwner.Remove(businessOwner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessOwnerExists(int id)
        {
            return _context.BusinessOwner.Any(e => e.Id == id);
        }
    }
}
