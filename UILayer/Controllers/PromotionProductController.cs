using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.EF;
using DataLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;

namespace UILayer.Controllers
{
    public class PromotionProductController : Base0Controller
    {
        PromotionProductService _promotionService;
        public PromotionProductController(OnlineShopping _onlineShopping) :base("",  _onlineShopping)
        {
            _promotionService = new PromotionProductService(_onlineShopping);
        }
        public IActionResult Index(PromotionTypes promotionTypes)
        {
          
            return View();
        }
    }
}