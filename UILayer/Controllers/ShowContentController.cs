using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataLayer;
using ServiceLayer;
using UILayer.Maper;
using DataLayer.Contract;
using DataLayer.Enums;
using UILayer.Miscellaneous;

using UILayer.Models;
using DataLayer.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UILayer.Controllers
{
    public class ShowContentController : Base0Controller
    //<Content, ContentService, ContentMaper, ContentContract>, ICURDOperation<Content>
    {
        ContentService _service;
        Content _entity;
        MessageService _messageService;
        //
        // GET: /Adminstration/EnterContent/
        public ShowContentController(OnlineShopping _onlineShopping)
            : base("ShowContent", _onlineShopping)
        {
            _service = new ContentService(_onlineShopping);
            _entity = new Content();
            _messageService = new MessageService(_onlineShopping);
        }
        ModelShowContent _modelShowContent = new ModelShowContent();

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            ViewData["TypeView"] = "ShowContent";
        }

        public ActionResult IndexContent(int CatId, int CenterId)
        {
            _modelShowContent.CategoryContent = _contentMaper.EntityToContract(_service.Find(c => c.Id == CatId));
            _modelShowContent.CenterContent = _contentMaper.EntityToContract(_service.Find(c => c.Id == CenterId));
            return View(_modelShowContent);
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        /// <summary>
        /// یک پیغام را نمایش می دهد
        /// </summary>
        /// <param name="message">پیغام</param>
        /// <returns></returns>
        public ActionResult ShowMessage(string message)
        {
            ViewData["ShowContent"] = message;
            return View();
        }


        public ActionResult SuccessPayment(string message, string invoivceId)
        {
            try
            {
                updateInvoiceUserAndSesstion(SessionUserContract); 

                _messageService.SendPaymentRef(SessionUserContract.Mobile, invoivceId);
            }
            catch (Exception)
            {
            }
            ViewData["ShowContent"] = message;
            return View("ShowMessage");
        }



    }
}
