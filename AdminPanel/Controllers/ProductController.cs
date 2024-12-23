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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using DataLayer;
using DataLayer.Enums;

namespace AdminPanel.Controllers
{
    public class ProductController : BaseController
    {
        private ProductService _productService;
        private CategoryService _categoryService;
        private BusinessOwnerService _businessOwnerService;
        private ContentService _contentService;
        OnlineShopping _context;
        public ProductController(ProductService productService, CategoryService categoryService
            , BusinessOwnerService businessOwnerService, ContentService contentService, OnlineShopping context)
        {
            _productService = productService;
            _categoryService = categoryService;
            _businessOwnerService = businessOwnerService;
            _contentService = contentService;
            _context = context;


        }

        // GET: Product
        public IActionResult Index(int? pageIndex,Product product ,
            ProductActivationStatus productActivationStatus, string addDateStr = "1399/05/01")
        {
            if (pageIndex == null || pageIndex <= 0)
            {
                pageIndex = 1;
            }

            //var product = new Product();
            //product.Id = product.Id;
            //product.Name = product.Name;
            //product.FkCategory = product.FkCategory;
            //product.BeforDiscountPrice = product.BeforDiscountPrice;
            //product.Active = product.Active;

            var ctaegory = _categoryService.GetAll().ToList();
            ViewBag.FkCategory = new SelectList(ctaegory, "Id", "Name", product.FkCategory);

            ViewBag.productActivationStatus = 
                new SelectList(EnumUtility.EnumToList<ProductActivationStatus>().Where(t => t.Id < 250),
                "Id", "Name", (int)productActivationStatus);

            product.AddDate = addDateStr.PersianToGorgian();
            ViewBag.SearchValue = product;
            // await _productService.Search(product, productActivationStatus);
            ViewBag.productActivationStatusSelected = productActivationStatus;
            ViewData["PromotionType"] = new SelectList(EnumUtility.EnumToList<PromotionTypes>().Where(t => t.Id < 100), "Id", "Name");
            var result = _productService.GetAll(Pagination.Create(pageIndex.Value), product, productActivationStatus);
            return View(result);
        }
        //  [Route("Product/Item/{id?}")]
        public async Task<IActionResult> Item(int? id, int? pageIndex)
        {

            if (pageIndex == null || pageIndex <= 0)
            {
                pageIndex = 1;
            }
            ViewBag.pageIndex = pageIndex;
            var ctaegory = _categoryService.GetAll().ToList();
            var businesses = _businessOwnerService.GetAll().ToList();
            var content = _contentService.GetAll().ToList();

            if (id == null)
            {
                ViewData["FkBusinessOwner"] = new SelectList(businesses, "Id", "Name");
                ViewData["FkCategory"] = new SelectList(ctaegory, "Id", "Name");
                ViewData["FkContent"] = new SelectList(content, "Id", "Name");

                return View(new Product());
            }

            var product = _productService.GetById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.BPCList = await IndexBPC(product, product.FkCategory);

            ViewData["FkBusinessOwner"] = new SelectList(businesses, "Id", "Name", product.FkBusinessOwner);
            ViewData["FkCategory"] = new SelectList(ctaegory, "Id", "Name", product.FkCategory);
            ViewData["FkContent"] = new SelectList(content, "Id", "Name", product.FkContent);

            return View(product);
        }
        
         [HttpPost]
        [ValidateAntiForgeryToken]
        //  [AllowAnonymous]   
        //  [AcceptVerbs()]
        public async Task<IActionResult> Item(Product product, int multiColore, int? pageIndex)
        {
            if (pageIndex == null || pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (product.UserStar > 5) product.UserStar = 5;
            if (product.UserStar < 0) product.UserStar = 0;
          
          //  product.Name = product.Name.Trim().Replace('-', '_');

            ViewBag.pageIndex = pageIndex;
            if (multiColore > 0)
            {
                product.AvailableColors = product.AvailableColors + ",#mmm";
            }
            if (ModelState.IsValid)
            {
                product.UpdateDate = DateTime.Now;
                // product.FkBusinessOwner = 8;
                if (product.Id <= 0)
                {
                    product.RegisterDate = DateTime.Now.ToPersianDate();
                    product.AddDate = DateTime.Now;
                    product.NameForUrll = product.NameForUrll.ToLower().Replace(' ','-');
                    _productService.Add(product);

                }
                else
                {//product.AddDate.AddDays(1) > DateTime.Now
                    var pOld=  _productService.FirstOrDefault(p => p.Id == product.Id);

                    // فقط زمان فعال سازی اولیه این فیلد مقدار دهی می شود و تا آخر باقی می ماند 
                    //if ((!pOld.Active) && product.Active) product.NameForUrll = product.Name;
                  //  else product.NameForUrll = pOld.NameForUrll;

                    //pOld = product;
                    _productService.Copy(pOld, product);
                    _context.SaveChanges();
                  
                }
                _productService.SaveChanges();
                return RedirectToAction("Item", new { id = product.Id, pageIndex = pageIndex.Value });
            }

            var ctaegory = _categoryService.GetAll().ToList();
            var businesses = _businessOwnerService.GetAll().ToList();
            var content = _contentService.GetAll().ToList();
            ViewData["FkBusinessOwner"] = new SelectList(businesses, "Id", "Name", product.FkBusinessOwner);
            // ViewData["FkBusinessOwner"] = new SelectList(Enumerable.Empty<BusinessOwner>(), "Id", "Name", product.FkBusinessOwner);
            ViewData["FkCategory"] = new SelectList(ctaegory, "Id", "Name", product.FkCategory);
            ViewData["FkContent"] = new SelectList(content, "Id", "Name", product.FkContent);
            ViewBag.BPCList = await IndexBPC(product, product.FkCategory);

            return View(product);
        }
        [AllowAnonymous]
        [HttpPost]
        [AcceptVerbs()]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, string inputFileName, int productId)
        {
            if (files.Count == 0) return new RedirectResult(Url.Action("Item", new { id = productId }) + "#imgArea");
            // return RedirectToAction("Item", new { id = productId  });
            string physicalPath = new AppSetting().ImagePathInServer;  // webRootPath.Replace("Upload\\UploadFileAdminSite", Paths.AdminSiteFilePath);

            //   var formFile = files.FirstOrDefault();

            foreach (var formFile in files)
            {
                var product = _productService.GetById(productId);
                if (product == null)
                {
                    return NotFound();
                }
                // foreach (var formFile in files) 
                // {
                var fileInfo = new System.IO.FileInfo(formFile.FileName);

                if (formFile.Length < 100000 || formFile.Length > 3000000 || (fileInfo.Extension != ".jpg" && fileInfo.Extension != ".jpeg"))
                {
                    throw new MyException((byte)ExceptionType.validation, "validation", "سایز فایل نباید بزرگتر از دو مگا بایت باشد و کوچکتر 300کیلو نباشد وفرمت عکس جیپق باید باشد");
                }

                var filename = Guid.NewGuid() + ".jpg";

                // Request.Files[InputName].SaveAs(physicalPath + filename);

                if (formFile.Length > 0)
                {
                    var filePath = physicalPath + "\\" + filename;
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
                if (!filename.Replace(".jpg", "").CheckImageRatio())
                {
                    filename.Replace(".jpg", "").RemoveAllInsatnceImg();
                    return new RedirectResult(Url.Action("Item", new { id = productId }) + "#imgArea");
                }
                product.UpdateDate = DateTime.Now;
                // }
                if (inputFileName == "Image")
                {
                    //  product.Image.RemoveAllInsatnceImg(); // عکس های قبلی رو حذف می کنیم
                    product.Image = filename.Replace(".jpg", "");
                }
                else if (inputFileName == "Image1")
                {
                    product.Image1.RemoveAllInsatnceImg();
                    product.Image1 = filename.Replace(".jpg", "");
                }
                else if (inputFileName == "Image2")
                {
                    // product.Image2.RemoveAllInsatnceImg();
                    product.Image2 = string.Concat(product.Image2, ",", filename.Replace(".jpg", ""));
                }
                else if (inputFileName == "Image3")
                {
                    product.Image3.RemoveAllInsatnceImg();
                    product.Image3 = filename.Replace(".jpg", "");
                }
                else if (inputFileName == "Image4")
                {
                    product.Image4.RemoveAllInsatnceImg();
                    product.Image4 = filename.Replace(".jpg", "");
                }
                filename.Replace(".jpg", "").SaveImgDetails();
                filename.Replace(".jpg", "").SaveImgListUI();
                filename.Replace(".jpg", "").SaveImgList();
            }


            //  _productService.Update(product);
            _productService.SaveChanges();

            return new RedirectResult(Url.Action("Item", new { id = productId }) + "#imgArea");
            //RedirectToAction("Item" , new { id  = productId });
        }

        //
        public async Task<IActionResult> DeleteImage(string id, int productId)
        {
            var product = _productService.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }
            product.Image2 = product.Image2.Replace("," + id, "");
            id.RemoveAllInsatnceImg();
            product.UpdateDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return new RedirectResult(Url.Action("Item", new { id = productId }) + "#imgArea");
        }

        #region BrigeProductCategoryProperties

        // GET: BrigeProductCategoryProperties
        private async Task<List<BrigeProductCategoryProperty>> IndexBPC(Product product, int fkCategory)
        {
            var BPCs = _context.BrigeProductCategories.Where(b => b.FkProduct == product.Id
            && b.CategoryProperty.FKCategory == fkCategory).Include(b => b.CategoryProperty).Include(b => b.Product);
            var BPCList = await BPCs.ToListAsync();

            var CPs = _context.CategoryProperties.Include(b => b.FkCategoryNavigation).Where(c => c.FKCategory == fkCategory);

            foreach (var _cp in CPs)
            {
                if (!BPCList.Any(b => b.FkCategoryProperty == _cp.Id))
                {
                    BPCList.Add(new BrigeProductCategoryProperty()
                    {
                        FkProduct = product.Id,
                        FkCategoryProperty = _cp.Id,
                        value = ""
                    ,
                        Product = product,
                        CategoryProperty = _cp
                    });
                }
            }

            return BPCList == null ? new List<BrigeProductCategoryProperty>() : BPCList;
        }

        // POST: BrigeProductCategoryProperties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBPC(string value, int FkCategoryProperty, int FkProduct, int? pageIndex)
        {
            if (pageIndex == null || pageIndex <= 0)
            {
                pageIndex = 1;
            }
            ViewBag.pageIndex = pageIndex;
            var _bpc = await _context.BrigeProductCategories.FirstOrDefaultAsync(b => b.FkProduct == FkProduct && b.FkCategoryProperty == FkCategoryProperty);
            if (_bpc == null)
            {
                _bpc = new BrigeProductCategoryProperty() { FkCategoryProperty = FkCategoryProperty, FkProduct = FkProduct };
                _bpc.value = value;
                _context.Add(_bpc);
            }
            else
            {
                _bpc.value = value;

            }


            await _context.SaveChangesAsync();

            return new RedirectResult(Url.Action("Item", new { id = FkProduct, pageIndex = pageIndex.Value }) + "#propCat");

        }

        #endregion


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalcStarProduct()
        {
            await _productService.IntegrateSatarUserAndComment();
            return RedirectToAction("Index");
        }

        public IActionResult ShowAllProduct(int? pageIndex, Product product,
         ProductActivationStatus productActivationStatus, string addDateStr = "1399/05/01")
        {
            IPagedList<Product> result = searchProduct( pageIndex, product, productActivationStatus, addDateStr);
            return View(result);
        }

        private IPagedList<Product> searchProduct( int? pageIndex, Product product, ProductActivationStatus productActivationStatus, string addDateStr)
        {
            if (pageIndex == null || pageIndex <= 0)
            {
                pageIndex = 1;
            }
            var ctaegory = _categoryService.GetAll().ToList();
            ViewBag.FkCategory = new SelectList(ctaegory, "Id", "Name", product.FkCategory);

            ViewBag.productActivationStatus = new SelectList(EnumUtility.EnumToList<ProductActivationStatus>().Where(t => t.Id < 250), "Id", "Name", (int)productActivationStatus);

            product.AddDate = addDateStr.PersianToGorgian();
            ViewBag.SearchValue = product;
            // await _productService.Search(product, productActivationStatus);
            ViewBag.productActivationStatusSelected = productActivationStatus;
            // ViewData["PromotionType"] = new SelectList(EnumUtility.EnumToList<PromotionTypes>().Where(t => t.Id < 100), "Id", "Name");
            var result = _productService.GetAll(Pagination.Create(pageIndex.Value), product, productActivationStatus);
            return result;
        }

        //       <input name = "pageIndex" value="@Model.PageIndex" type="hidden" />
        //<input name = "Id" value="@p.Id" type="hidden" />
        //<input name = "Name" value="@p.Name" type="hidden" />
        //<input name = "FkCategory" value="@p.FkCategory" type="hidden" />
        //<input name = "BeforDiscountPrice" value="@p.BeforDiscountPrice" type="hidden" />
        //<input name = "Active" value="@p.Active" type="hidden" />
        //<input name = "addDateStr" value="@p.AddDate.ToPersianDate()" type="hidden" />
        //<input name = "productActivationStatus" value="@ViewBag.productActivationStatusSelected" type="hidden" />
        public async Task<IActionResult> UpdetAllProduct(int[] productIds, decimal[] prices, int[] rankShows,
            int? PageIndex, Product _product, ProductActivationStatus productActivationStatus, string addDateStr
            )
        {

            for (int i = 0; i < productIds.Length; i++)
            {
                var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == productIds[i]);
                product.Price = prices[i];
                product.RankShow = rankShows[i];
            }

            var resSave = await _context.SaveChangesAsync();
            if (resSave > 0) //to do pupup message
            {

            }
            IPagedList<Product> result = searchProduct(PageIndex, _product, productActivationStatus, addDateStr);
            return View("ShowAllProduct", result);

 

            //RedirectToAction(nameof(ShowAllProduct));
        }
        public IActionResult GetImageList() {
            AppSetting appSetting = new AppSetting();
         string str=   ExtentionMethodsImage.GetAllFileListFolder();
            ViewBag.imageList = str;
            return View();
        }
        public IActionResult RemoveResizeCopyImage()
        {
            AppSetting appSetting = new AppSetting();
            bool result = ExtentionMethodsImage.RemoveAllFileSizeListFolder(appSetting.ImagePathInServer);

            return RedirectToAction("GetImageList");
        }
        public IActionResult CreateResizeCopyImage()
        {
            AppSetting appSetting = new AppSetting();
            bool result = ExtentionMethodsImage.CreateImagesReSize(appSetting.ImagePathInServer);

            return RedirectToAction("GetImageList");
        }


        //to do delete afte publish url change
        public async Task<string> UpdateAllUrl()
        {
            var ps = _context.Product.ToList();

            foreach (var item in ps)
            {
                item.NameForUrll = item.Id +"-"+ item.Name.Replace(' ', '_');
            }
            await _context.SaveChangesAsync();
            return "عملیات با موفقیت انجام شد.";
            //  @Url.Action("AEntityClient", "Product", new { Id = product.Id, name = product.Name.Replace(' ', '_') }, "https");
        }
    }
}
