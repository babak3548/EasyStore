using DataLayer;
using DataLayer.Contract;
using DataLayer.Enums;
using ServiceLayer;
using UILayer.Maper;
using UILayer.Miscellaneous;
using Utility;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using DataLayer.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace UILayer.Controllers
{
    public class MessageController : Base0Controller
    {
        //
        // GET: /Message/
        MessageService _service;
        Message _entity ;
        public MessageController(  OnlineShopping _onlineShopping)
            : base("Message",  _onlineShopping)
        {
            _service = new MessageService((objectContext) as OnlineShopping);
            _entity = new Message();
        }
        public ActionResult sendSavedMsgToEmailView(int Id,string title="")
        {
            ViewData["titleEmail"] = title;
            return View(_service.FirstOrDefault(s => s.Id == Id));
        }

        public ActionResult sendSavedMsgToEmail(int Id, string title,string Email, string captcha)
          {
            string SessionCaptcha =HttpContext.Session.GetString("capcthText");
            HttpContext.Session.SetString("capcthText", "");
            if (SessionCaptcha != captcha) return sendSavedMsgToEmailView(Id,title);

           // _service.SendMessageToEmail(Email, title, _service.FirstOrDefault(m=>m.Id==Id).Text);
            var emailSender = new Email(AppSetting.LogFilePathShopping);
            emailSender.SendAEmail(Email, title, _service.FirstOrDefault(m => m.Id == Id).Text);
            return RedirectToAction("ShowMessage", "ShowContent", new { message = "پیغام شما با موفقیت ارسال شد " }); 
    }

        public ActionResult ShowMessage(int Id)
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            _entity = _service.FirstOrDefault(m => m.Id == Id);

            if (_entity.FkUserReceiver == SessionUserContract.Id)
            {
                _entity.Readed = true;
                _service.SaveAllChengeOrAllReject(true);
                return View(_entity);
            }
            else { throw new MyException((byte)ExceptionType.UnnormalSenario, "UnnormalSenaruo", " لطفا با روند صحیح  درخواستتان را ارسال نمایید"); ; }
        }
        
        public ActionResult GridView()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            return View(_service.Find(m => m.FkUserReceiver == SessionUserContract.Id & m.Readed == false));
        }

        public ActionResult AllMessageView()
        {
             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 
            return View("GridView", _service.Find(m => m.FkUserReceiver == SessionUserContract.Id));
        }

        public ActionResult SendEmailMessageView(int id, string Email = "", string title = "", string text = "", string message = "", string redirectAddress = "")
        {
            _entity.FkUserReceiver = id;
           _entity.FkUseSender = SessionUserContract.Id;
          // ViewData["EmailSender"] = _entity.FkUseSender != 0 ? CurrentUserContract.Email : Email;
              ViewData["titleEmail"] = title;
              ViewData["message"] = message;
              ViewData["redirectAddress"] = redirectAddress;
           _entity.Date = Culture.CurrentDate;
           return View("SendEmailMessageView", _entity);
           
        }

        public ActionResult SendEmailMessage(int FkUserReceiver, string Email, string title, string Text, string captcha, string redirectAddress)
        {
            string SessionCaptcha =HttpContext.Session.GetString("capcthText");
            HttpContext.Session.SetString("capcthText", "");
            if (SessionCaptcha != captcha) return SendEmailMessageView(FkUserReceiver, Email, title, Text, "کاربر  گرامی لطفا کارکتر های عکس را به طور صحیح وارد نمایید");

           UserService userService= new UserService(objectContext);
           User user = userService.FirstOrDefault(u => u.Id == FkUserReceiver);
            
            string emailSenderP ="<div style='text-align: justify;padding:5px;'>" ;
            emailSenderP += Email != "" ? " <b>ایمیل ارسال کننده : </b>" + Email : "ناشناس";
            emailSenderP += "<br />";

            string emailTitle = "<br/>" + " <b> عنوان :  </b><span>" + title + "</span><br/>" ;

            Text = "<p >" + Text +"</p> ";
            string redirectAddres = "<br/>" + " <a style='float: left;direction: ltr;' href='" + redirectAddress + "' >" + redirectAddress + "</a> </div>";

           // MessageService messageService = new MessageService(objectContext);
            string strBody = emailSenderP + emailTitle+Text + redirectAddres;
            _service.SendMessageToEmail(user.Email, "پیغام از بازدید کننده", strBody);
            
            //ذخیره پیغام برای طرف مقابل
            _service.RegisterAMessage(SessionUserContract.Id, FkUserReceiver, emailTitle + Text);

            //چون در قسمت بالا طرفین سفارش چک شده پس نیاز به چک کردن صاحب انتیتی نیست
            _service.SaveAllChengeOrAllReject(true);

            return RedirectToAction("ShowMessage", "ShowContent", new { message = "پیغام شما با موفقیت ارسال شد " }); 
        }

        public ActionResult RegisterView(object obj = null)
        {
            throw new NotImplementedException();
        }

        public ActionResult Register(Message entity, object obj = null)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public ActionResult EditView(int Id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(Message entity)
        {
            throw new NotImplementedException();
        }
    }
}
