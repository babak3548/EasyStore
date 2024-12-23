using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataLayer;
using ServiceLayer;
using UILayer.Maper;
using DataLayer.Contract;
using DataLayer.Enums;
using Utility;

using UILayer.Miscellaneous;
using UILayer.Models;
using System.Net;

using System.Data.SqlClient;
using System.Data;
using UILayer.BankGetWays;

using System.Xml.Serialization;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using DataLayer.EF;
using Microsoft.AspNetCore.Http;


using DataLayer.Models;
using System.Threading.Tasks;
using System.Text.Json;
using DataLayer.EFLog;

namespace UILayer.Controllers
{
    /// <summary>
    /// استProduct کلاس کنترولر موجودیت
    /// </summary> 
    public  class ProductController : Base0Controller
        //<Product, ProductService, ProductMaper, ProductContract>,  ICURDOperation<Product>
    {
        ProductService _service;
        Product _entity;
        LogService _LogService;
        //[RoutePrefix("services")]
        public ProductController(OnlineShopping onlineShopping  ,EasyStoreLog _EasyStoreLog) :base("Product", onlineShopping)
        {
            _service = new ProductService(onlineShopping);
            _LogService = new LogService(_EasyStoreLog);
        }
     
        public string SearchAutocomplete(string id)
        {
            string[] result = new string[4] { "bac", "bad", "baf", "bag" };
            //int i = 0;
            //foreach (var product in _service.SrchAjaxKeyWordFuncProducts(id, 0).Take(15))
            //{
            //    result[i]= product.WordKey;
            //    i++;

            //}
            return "this is test";
        }


        public ActionResult MarketerProducts(string Id = "", int PageNumber = 0)
        {
            var marketerService = new MarketerService(objectContext);
            var marketer = marketerService.FirstOrDefault(m => m.Name == Id);
            ViewData["marketer"] = marketer;
            if (marketer != null)
            {
                ViewData["FK_Marketer"] = marketer.Id;
                var urlReferrer = Request.Headers["Referer"].ToString();
                if (urlReferrer == null || !urlReferrer.ToString().Contains(AppSetting.domainNameMini))
                { addOrChangeCookie("FK_Marketer", marketer.Id.ToString()); }
                var clientGridModel = new SearchResultModel
                {
                    StoreName = "شعبه بازاریابی  " + marketer.Name,
                    TitelSearch = "محصولات",
                    RowCount = (short)_service.Find(p => (p.FkBusinessOwnerNavigation.FkMarketerNavigation == null ? false : p.FkBusinessOwnerNavigation.FkMarketerNavigation.Name == Id) | p.FkBusinessOwnerNavigation.BridgeBusinessOwnerMarketer.Any(b => b.FkMarketerNavigation.Name == Id)).Count(),
                    Model = _service.Find(p => p.Active != false &&
                        (
                        (p.FkBusinessOwnerNavigation.FkMarketerNavigation == null ? false : p.FkBusinessOwnerNavigation.FkMarketerNavigation.Name == Id)
                        | p.FkBusinessOwnerNavigation.BridgeBusinessOwnerMarketer.Any(b => b.FkMarketerNavigation.Name == Id)
                        )
                        )
                    .Skip(PageNumber * ConstSetting.PageSize).Take(ConstSetting.PageSize),
                };
                return View(clientGridModel);
            }
            else
            {
                return RedirectToAction("ShowMessage", "ShowContent", new { message = "شعبه بازاریابی با این نام وجود ندارد لطفا از درست بودن نام شعبه اطمینان حاصل نمایید " });
            }
        }

         public ActionResult BusinessOwnerProducts(string Id, int PageNo = 1)
        {
            //در خواستهای اشتباه خالی اکشن کلاینت گرید را با جای گزاری یک به این اکشن می رسدو و به این ترتیب  کنسل می گردد  
            if (Id == "1") return Ok();

            BusinessOwnerService businessOwnerService = new BusinessOwnerService(objectContext);
            BusinessOwner businessOwnerEntity = businessOwnerService.FirstOrDefault(b => b.Name == Id);
            ViewData["businessOwner"] = businessOwnerEntity;
            if (businessOwnerEntity == null)
            { return RedirectToAction("ShowMessage", "ShowContent", new { message = "شعبه فروشی با این نام وجود ندارد لطفا از درست بودن نام شعبه اطمینان حاصل نمایید " }); }
            else
            {
                var clientGridModel = new SearchResultModel
                {
                    StoreName = "محصولات فروشگاه  " + businessOwnerEntity.Name,
                    TitelSearch = "محصولات",
                    RowCount = (short)_service.Find(p => p.Active != false && p.FkBusinessOwnerNavigation.Name == Id).Count(),
                    Model = _service.Find(p => p.Active != false && p.FkBusinessOwnerNavigation.Name == Id).Skip((PageNo - 1) * ConstSetting.PageSize).Take(ConstSetting.PageSize),
                };
                return View("ClientGrid2", clientGridModel);
            }
        }

        public  ActionResult ProductByCategoryId(int FK_Category, string searchValue = "", int PageNumber = 0)
        {
            throw new NotImplementedException();
        }
        //int x= 1 - 1;
        //var c = 1 / x;
        #region IClientGridController
        public ActionResult testLog()
        {
            int x = 1 - 1;
            var c = 1 / x;
            return View();
        }
        [HttpGet] // [Route("product/{Id:int}-{name}")]
        [Route("product/{nameForUrl}")]
        public async Task<IActionResult> AEntityClient(string nameForUrl)
        {
            

            //SetModelRow(ref _modelRow, "AEntityClient", _entityName, false, "", ScenarioOrViewName.UserView, "AEntityClient", "", _entityName, _entityName, new Dictionary<string, object> { { "SearchValue", SearchValue } }
            //    , null, (_maper).EntityToContract(_service.Find(p => p.Id == Id)));

            _entity = _service.GetProduct(nameForUrl);

            if (_entity== null)
            {
                string idStr = nameForUrl.Split("-")[0];
                int id = 0;
                _LogService.SaveInfo("کلید آدرس ارسالی پیدا نشد و از روش شناسه چک گردید" +
                    (int.TryParse(idStr,out id)? "شناسه تبدیل شد" : "شناسه تبدیل نشد و خطا"), "", "");
                _entity = _service.GetProduct(id);


                if (_entity == null)
                {
                    _LogService.SaveInfo("کلید آدرس ارسالی , شناسه ارسال پیدا نشد و notfund nameForUrl:" + nameForUrl, "", "");
                    return RedirectPermanent(DataLayer.AppSetting.DomainName + "/" + DefualtValue.AllCategory);
                }
                //else
                //{
                //    return RedirectPermanent(DataLayer.AppSetting.DomainName + "/product/" + _entity.NameForUrll);
                //}
            }



            List<PromotionProductModel> allPromotionProductModels = new List<PromotionProductModel>();

             var related = _service.GetRelaetedProduct(_entity.FkCategory);
            ViewBag.RelatedProducts = related;
            var pro1 = _service.GetPromotionProductModels(related );
             allPromotionProductModels.AddRange(pro1);

            var bests = _service.GetBestSellerProduct(_entity.FkCategory);
            ViewBag.BestSellerProducts = bests;
            var pro2 =  _service.GetPromotionProductModels(bests);
            allPromotionProductModels.AddRange(pro2);

            ViewBag.allPromotionProductModels = JsonSerializer.Serialize(allPromotionProductModels);

            if (_entity.Image1 == null | _entity.Image1 == "") _entity.Image1 = _entity.Image;
            if (_entity.Image2 == null | _entity.Image2 == "") _entity.Image2 = _entity.Image;

            var yetaPro = new YektanetProduct { brand = _entity.Brand, category = new string[] { _entity.FkCategoryNavigation.Name }, image =AppSetting.DomainName + _entity.Image.GetUrlListImg(), isAvailable = _entity.Available > 0,
                price = (int)_entity.Price / 10, sku = _entity.Id.ToString(), title = _entity.Name
            };
            yetaPro.discount =(int) (_entity.BeforDiscountPrice > _entity.Price ? _entity.BeforDiscountPrice - _entity.Price : 0 )/10;
            ViewBag.yektanetProduct =  JsonSerializer.Serialize(yetaPro);

            return View( _entity);
        }
       
        #endregion IClientGridController

   




     


    }
}
