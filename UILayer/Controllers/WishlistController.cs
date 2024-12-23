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
    public class WishlistController : Base0Controller
    {
        WishService _wishService;
        public WishlistController(OnlineShopping onlineShopping) : base("Wishlist", onlineShopping)
        {
            _wishService = new WishService(onlineShopping);
        }
        public IActionResult Index()
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User", new { redirectUrl = Url.Action("Index", "Wishlist") });
            var queryWish = _wishService.GetWishUser(SessionUserContract.Id);
            return View(queryWish);
        }

        public IActionResult AddWishlist(int productId)
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User", new { redirectUrl = Url.Action("AddWishlist", "Wishlist", new { productId = productId }) });
            _wishService.AddToWishUser(SessionUserContract.Id, productId);
            updateInvoiceUserAndSesstion(SessionUserContract);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveWishlist(int id)
        {
            if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User", new { redirectUrl = Url.Action("Index", "Wishlist") });
            _wishService.DeActiveWishUser(SessionUserContract.Id, id);
            updateInvoiceUserAndSesstion(SessionUserContract);
            return RedirectToAction("Index");
        }
    }
}
