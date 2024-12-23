using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.EF;
using DataLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DataLayer;
using Utility;
using ServiceLayer;

namespace AdminPanel.Controllers
{
    public class PromotionProductsController : Controller
    {
        private readonly OnlineShopping _context;
        private PromotionProductService _promotionProductService;
        public PromotionProductsController(OnlineShopping context , PromotionProductService promotionProductService)
        {
            _context = context;
            _promotionProductService = promotionProductService;
        }

        // GET: PromotionProducts
        public async Task<IActionResult> Index(int? pageIndex , [Bind("Id,PromotionType,FkProduct,Order,ExpireDateTime,Image,FkCategory")] PromotionProduct searchObj)
        {
            ViewData["PromotionType"] =  new SelectList( EnumUtility.EnumToList<PromotionTypes>(), "Id", "Name");
       
            ViewData["FkCategory"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["FkProduct"] = new SelectList(_context.Product, "Id", "Name");
            ViewBag.searchObj = searchObj;
           // var onlineShopping = _context.PromotionProduct.Include(p => p.Category).Include(p => p.Product);
                if (pageIndex == null || pageIndex <= 0)
                {
                    pageIndex = 1;
                }
        
                var result = _promotionProductService.GetAll(Pagination.Create(pageIndex.Value) , searchObj);

                return View(result);
        }


        // GET: PromotionProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotionProduct = await _context.PromotionProduct
                .Include(p => p.Category)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotionProduct == null)
            {
                return NotFound();
            }

            return View(promotionProduct);
        }

        // GET: PromotionProducts/Create
        public IActionResult Create()
        {
            ViewData["FkCategory"] = new SelectList(_context.Category, "Id", "Name");
            
            ViewData["FkProduct"] = new SelectList(_context.Product, "Id", "Name");
            ViewData["PromotionType"] = new SelectList(EnumUtility.EnumToList<PromotionTypes>().Where(t => t.Id >= 100), "Id", "Name");
            return View();
        }

        public async Task<IActionResult> CreateProduct(PromotionProduct pm,int[] productIds)
        {
 
                foreach (var proId in productIds)
                {
                  var product= await _context.Product.FirstOrDefaultAsync(p => p.Id == proId);
                    var newPromo = new PromotionProduct
                    {
                        ExpireDateTime = pm.ExpireDateTime,
                        FkProduct = proId,
                        Order = product.RankShow,
                        PromotionType = pm.PromotionType,
                        FkCategory = product.FkCategory
                    };
                    _context.Add(newPromo);
                }
            var resSave=    await _context.SaveChangesAsync();
                if (resSave > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                ViewData["FkCategory"] = new SelectList(_context.Category, "Id", "Name", pm.FkCategory);
                ViewData["FkProduct"] = new SelectList(_context.Product, "Id", "Name", pm.FkProduct);
                return RedirectToAction("Index", "Product");
            }
        } 
        // POST: PromotionProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PromotionType,FkProduct,Order,ExpireDateTime,Image,FkCategory,Title,Query")] PromotionProduct promotionProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(promotionProduct);
                await _context.SaveChangesAsync();
                return new RedirectResult(Url.Action("Edit", new { id = promotionProduct.Id }) + "#imgArea");
               // return RedirectToAction(nameof(Index));
            }
            ViewData["FkCategory"] = new SelectList(_context.Category, "Id", "Name", promotionProduct.FkCategory);
            ViewData["FkProduct"] = new SelectList(_context.Product, "Id", "Name", promotionProduct.FkProduct);
           
            return View(promotionProduct);
        }

        [AllowAnonymous]
        [HttpPost]
        [AcceptVerbs()]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, string inputFileName, int promotionProductId)
        {
            if (files.Count == 0) return new RedirectResult(Url.Action("Edit", new { id = promotionProductId }) + "#imgArea");
            // return RedirectToAction("Item", new { id = productId  });
            string physicalPath = new AppSetting().ImagePathOtherFileServer;  // webRootPath.Replace("Upload\\UploadFileAdminSite", Paths.AdminSiteFilePath);

            var formFile = files.FirstOrDefault();

            var promotion = _context.PromotionProduct.FirstOrDefault(c => c.Id == promotionProductId);
            if (promotion == null)
            {
                return NotFound();
            }
            // foreach (var formFile in files) 
            // {
            var fileInfo = new System.IO.FileInfo(formFile.FileName);

            if (formFile.Length < 5 || formFile.Length > 800000 || (fileInfo.Extension != ".jpg" && fileInfo.Extension != ".png" && fileInfo.Extension != ".gif"))
            {
                throw new MyException((byte)ExceptionType.validation, "validation", "سایز فایل نباید بزرگتر از 800 کیلو بایت باشد و کوچکتر 10 بایت نباشد وفرمت عکس جیپق باید باشد");
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
                (physicalPath + promotion.Image).RemoveFile(); // عکس های قبلی رو حذف می کنیم
                promotion.Image = filename;
            }
            
            _context.SaveChanges();

            return new RedirectResult(Url.Action("Edit", new { id = promotionProductId }) + "#imgArea");
            //RedirectToAction("Item" , new { id  = productId });
        }
        // GET: PromotionProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotionProduct = await _context.PromotionProduct.FindAsync(id);
            if (promotionProduct == null)
            {
                return NotFound();
            }
            ViewData["PromotionType"] = (int)promotionProduct.PromotionType <100?
                new SelectList(EnumUtility.EnumToList<PromotionTypes>().Where(t => t.Id < 100), "Id", "Name",(int)promotionProduct.PromotionType) : 
                new SelectList(EnumUtility.EnumToList<PromotionTypes>().Where(t => t.Id >= 100), "Id", "Name" ,(int)promotionProduct.PromotionType);
            ViewData["FkCategory"] = new SelectList(_context.Category, "Id", "Name", promotionProduct.FkCategory);
            ViewData["FkProduct"] = new SelectList(_context.Product, "Id", "Name", promotionProduct.FkProduct);
            return View(promotionProduct);
        }

        // POST: PromotionProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PromotionType,FkProduct,Order,ExpireDateTime,Image,FkCategory,Title,Query")] PromotionProduct promotionProduct)
        {
            if (id != promotionProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //  _context.Update(promotionProduct);
                    var obj = _context.PromotionProduct.FirstOrDefault(p=>p.Id== promotionProduct.Id);
                    obj.FkCategory = promotionProduct.FkCategory;
                    obj.ExpireDateTime = promotionProduct.ExpireDateTime;
                    obj.FkProduct = promotionProduct.FkProduct;
                   // obj.Image = promotionProduct.Image;
                    obj.Order = promotionProduct.Order;
                    obj.PromotionType = promotionProduct.PromotionType;
                    obj.Query = promotionProduct.Query;
                    obj.Title = promotionProduct.Title;
                   // obj. = promotionProduct.FkCategory;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionProductExists(promotionProduct.Id))
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
            ViewData["FkCategory"] = new SelectList(_context.Category, "Id", "Name", promotionProduct.FkCategory);
            ViewData["FkProduct"] = new SelectList(_context.Product, "Id", "Name", promotionProduct.FkProduct);
            return View(promotionProduct);
        }

        // GET: PromotionProducts/Delete/5
    
        public async Task<IActionResult> Delete(int id , PromotionTypes PromotionType, DateTime ExpireDateTime , int FkCategory)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var promotionProduct = await _context.PromotionProduct
            //    .Include(p => p.Category)
            //    .Include(p => p.Product)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (promotionProduct == null)
            //{
            //    return NotFound();
            //}
            var promotionProduct = await _context.PromotionProduct.FindAsync(id);
            _context.PromotionProduct.Remove(promotionProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index),new {
                PromotionType = PromotionType,
                ExpireDateTime = ExpireDateTime,
                FkCategory = FkCategory
            });
        }

        // POST: PromotionProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promotionProduct = await _context.PromotionProduct.FindAsync(id);
            _context.PromotionProduct.Remove(promotionProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionProductExists(int id)
        {
            return _context.PromotionProduct.Any(e => e.Id == id);
        }



    }
}
