using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UILayer.Models;

using Microsoft.AspNetCore.Http;
using DataLayer.EF;
using DataLayer.Enums;
using ServiceLayer;
using DataLayer.Models;
using System.Text.Json;
using Utility;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace UILayer.Controllers
{
    //[RoutePrefix("services")]
    public class HomeController : Base0Controller
    {
        private readonly ILogger<HomeController> _logger;
        ContentService _contentService;
        ProductService _service;

        public HomeController(ILogger<HomeController> logger, OnlineShopping _onlineShopping) : base("", _onlineShopping)
        {
            _logger = logger;
            _contentService = new ContentService(_onlineShopping);
            _service = new ProductService(_onlineShopping);
        }
        //[HttpGet]
        //[Route("")]
        //public async Task<IActionResult> Index()
        //{
        //    return await getHomePageInfo();
        //}

        private async Task<IActionResult> getHomePageInfo()
        {

            ProductService productService = new ProductService(objectContext);
            ContentService contentService = new ContentService(objectContext);
            PromotionProductService promotionProductService = new PromotionProductService(objectContext);
            List<PromotionProductModel> allPromotionProductModels = new List<PromotionProductModel>();

            ViewBag.TopHomeCategory = productService.PromotionCategoryByType(PromotionCategory.TopHomeCategory);


            var BestOfferMonth = await productService.PromotionProductByType(PromotionTypes.BestOfferMonth);
            ViewBag.BestOfferMonth = BestOfferMonth;
            allPromotionProductModels.AddRange(BestOfferMonth);

            var BestSellerProducts = await productService.PromotionProductByType(PromotionTypes.BestSellerProducts);
            ViewBag.BestSellerProducts = BestSellerProducts;
            allPromotionProductModels.AddRange(BestSellerProducts);

            var TopScoreMonth = await productService.PromotionProductByType(PromotionTypes.TopScoreMonth);
            ViewBag.TopScoreMonth = TopScoreMonth;
            allPromotionProductModels.AddRange(TopScoreMonth);

            var BottenHomePage = await productService.PromotionProductByType(PromotionTypes.BottenHomePage);
            ViewBag.BottenHomePage = BottenHomePage;
            allPromotionProductModels.AddRange(BottenHomePage);

            var NewProducts = await productService.PromotionProductByType(PromotionTypes.NewProducts);
            ViewBag.NewProducts = NewProducts;
            allPromotionProductModels.AddRange(NewProducts);

            var SpecialProducts = await productService.PromotionProductByType(PromotionTypes.SpecialProducts);
            ViewBag.SpecialProducts = SpecialProducts;
            allPromotionProductModels.AddRange(SpecialProducts);

            ViewBag.allPromotionProductModels = JsonSerializer.Serialize(allPromotionProductModels);

            ViewBag.Content_BottomHomePage = contentService.GetContentByType(ContentTypes.BottomHomePage);
            ViewBag.HomeMainSlider = promotionProductService.PromotionProductByTypes(PromotionTypes.TopHomeSlider1_3, PromotionTypes.TopHomeSlider2_3, PromotionTypes.TopHomeSlider3_3);

            ViewBag._HomeImgTopRow = promotionProductService.PromotionProductByTypes(PromotionTypes.HomeImgTopRow1_3, PromotionTypes.HomeImgTopRow2_3, PromotionTypes.HomeImgTopRow3_3);
            ViewBag._SecondHomeBaner = promotionProductService.PromotionProductByTypes(PromotionTypes.HomeImgSecondRow1_2, PromotionTypes.HomeImgSecondRow2_2);
            //  ViewBag.TopScoreMonth= ViewBag.SpecialProducts = ViewBag.NewProducts = ViewBag.BottenHomePage = ViewBag.BestSellerProducts= ViewBag.SpecialProducts = ViewBag.BestOfferMonth ;



            return View("Index");
        }
        //regex(^(?!.*device).*$)
        // [Route(@"{category:regex(\b(?!images)(?!(robots{{.txt}}))(?!sitemap)\b.+)?}")]
        //[Route(@"{category:regex(\b(?!pages)(?!city)(?!image)(?!search)(?!user)(?!book)(?!camp)\b.+)?}/{*url}")] (.*?)

        [HttpGet]
        //[Route("/{category?}")]
        [Route(@"{category:regex(\b(?!images)(?!sitemap)\b.+)?}")]
        public async Task<IActionResult> Index(string category, string query, decimal Price_min, decimal Price_max,
             PromotionTypes PromotionType, Sorting Sorting
            , int Fk_Marketer, short PageNo, int PageSize)
        {
            Price_min = Price_min * 10;
            Price_max = Price_max * 10;

            if (string.IsNullOrWhiteSpace(category) && string.IsNullOrWhiteSpace(query) && PromotionType == PromotionTypes.NoSetPromotionType)
            {
                return await getHomePageInfo();
            }

            if (category != null && category.Contains("www"))
            {
                return RedirectToActionPermanent("index", new { category = DefualtValue.AllCategory });
            }
            if (category != null && ( category.Contains("girl-dolls") || category.Contains("boyish-dolls") || category.Contains("toy")))
            {
                return RedirectToActionPermanent("index", new { category = "polish-doll" });
            }
            if (category != null && (category.Contains("decorating-accessories-child-room")))
            {
                return RedirectToActionPermanent("index", new { category = "sleeping-doll" });
            }
            SearchModel searchModel = new SearchModel
            {
                category = (category == "product" || string.IsNullOrWhiteSpace(category)
                ? DefualtValue.AllCategory : category),
                Fk_Marketer = Fk_Marketer,
                PageNo = PageNo == 0 ? (short)1 : PageNo,
                PageSize = PageSize == 0 ? ConstSetting.PageSize : PageSize,
                Price_max = Price_max,
                Price_min = Price_min,
                PromotionType = PromotionType,
                query = query,
                Sorting = Sorting == 0 ? Sorting.RankShow : Sorting,
            };

            if (searchModel.Fk_Marketer > 0) { addOrChangeCookie("ret", searchModel.Fk_Marketer.ToString()); }

            CategoryService categoryService = new CategoryService(objectContext);

            //Discription.Contains( (searchModel.category.Trim()))
            ViewBag.subCategories = CategoryService.categoryList;
                //objectContext.Category.OrderByDescending(o=>o.Image);
            ViewBag.selectedCategory = CategoryService.categoryList.FirstOrDefault(c => c.Discription == searchModel.category);
            //if (selectedcat == null)
            //{
            //    ViewBag.selectedCategory = objectContext.Category.FirstOrDefault(c => c.Discription == searchModel.category);
            //}
            //else
            //{
            //    ViewBag.selectedCategory = selectedcat;
            //}

            int count;
            searchModel.query = (searchModel.query != null && searchModel.query.Length > 100 ? searchModel.query.Substring(0, 99) : searchModel.query);

            var iQueyableModal = _service.SrchNamBrandKeywdDiscription(searchModel, out count);

            var ClientGridProduct = new SearchResultModel
            {
                Model = iQueyableModal.ToList(),
                RowCount = (short)count,
                SearchModel = searchModel,
            };

            var PromotionProductForQuikeView = await _service.GetPromotionProductModels(iQueyableModal);
            ViewBag.allPromotionProductModels = JsonSerializer.Serialize(PromotionProductForQuikeView);

            ViewBag.DefaultPrice_max = 2500000;
            //searchModel.Price_max > max ? searchModel.Price_max +20000 : max ; // به تومان
            ClientGridProduct.SearchModel.Price_min = ClientGridProduct.SearchModel.Price_min / 10;
            ClientGridProduct.SearchModel.Price_max = ClientGridProduct.SearchModel.Price_max / 10;
            //  searchModel.PromotionType = PromotionTypes.NoSetPromotionType; //to do remove after show filter items to user
            return View("FullSearch", ClientGridProduct);
        }



    }
}
