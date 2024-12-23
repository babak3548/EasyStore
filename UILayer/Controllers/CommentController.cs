using DataLayer;
using DataLayer.Contract;
using DataLayer.Enums;
using ServiceLayer;
using ServiceLayer.Maper;
using UILayer.Controllers;
using UILayer.Maper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNetCore.Mvc;
using DataLayer.EF;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http.Headers;

namespace UILayer.Controllers
{
    public class CommentController : BaseController<Comment, CommentService, UILayer.Maper.CommentMaper, CommentContract>, ICURDOperation<Comment>
    {
        //
        // GET: /Comment/


        public CommentController(OnlineShopping _onlineShopping)
            : base("CommentController",  _onlineShopping)
        {
            _service = new CommentService(objectContext);

        }

        public ActionResult RegisterScore(int id  ,int score )
        {
            if (score > 5 || score < 1) throw new Exception("امتیاز بیشتر از 5 نمی تواند باشد");

            string cookiName = "Comment" + id.ToString();
           string lastCookiName = getValeCookie(cookiName);
            string referer = Request.Headers["Referer"].ToString();
         //   string redirectUrl =((HttpRequestHeaders)Request.Headers).HeaderReferer;
                if (lastCookiName != null &&  lastCookiName.Contains("isSet"))
            {
                return Redirect(referer);
            }
            var comment=   objectContext.Comment.FirstOrDefault(f => f.Id == id);
            comment.VotePositive = comment.VotePositive.HasValue ? (comment.VotePositive * comment.VoteCount + score * 1) / (comment.VoteCount + 1) : score;
            comment.VoteCount += 1;
            objectContext.SaveChanges();
            addOrChangeCookie(cookiName, "isSet");
           
            return Redirect(referer);
        }
        /// <summary>
        ///  لیست یک نوع از کامنت ها را بر می گرداند
        /// </summary>
        /// <param name="commentType"> نوع کامنت</param>
        /// <returns></returns>
        public ActionResult ListQusetions(short? commentType=null,string title="")
        {
            ViewData["title"] = title;
            ViewData["commentType"] = commentType;
            return View(_service.Find(c => c.FkComment == null & c.CommentType == commentType & c.Active != false & c.CommentType != (short)CommentType.WorkToUs));
        }
        /// <summary>
        ///ویو ارسال یک کامنت را فراهم می آورد
        /// </summary>
        /// <param name="commentType">نوع کامنت</param>
        /// <returns></returns>
        public ActionResult AQusetion(short? commentType = null, string title = "")
        {
            ViewData["title"] = title;
            _entity = new Comment();
            _entity.CommentType = commentType;
            return View(_entity);
        }
        /// <summary>
        /// لیست جوابهای یک سوال را نمایش و امکان پاسخ به سوال را به کاربر می دهد
        /// </summary>
        /// <param name="Id">شناسه سوال</param>
        /// <returns></returns>
        public ActionResult Answers(int Id, string title = "")
        {
            ViewData["title"] = title;
            ViewData["Qusetion"] = _service.FirstOrDefault(c => c.Id == Id);
            return View(_service.Find(c => c.FkComment == Id & c.Active != false));
        }

        /// <summary>
        /// یک جواب را ذخیره میکند
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult AddAnswer(Comment entity)
        {
            _service.Add(entity);
            _service.SaveAllChengeOrAllReject(true);
           // return RedirectToAction("ShowMessage", "ShowContent", new { message = "کاربر گرامی پاسخ شما ذخبره گردید در اولین فرصت بررسی و پاسخ شما به جمع پاسخ ها  " });
           return RedirectToAction("Answers", new  {Id=entity.FkComment });
        }
        #region ICURDOperation
        public ActionResult RegisterView(object obj = null)
        {
            throw new NotImplementedException();

        }
        /// <summary>
        /// یک سوال یا پیشنهاد را ذخیره می نمایید
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult Register(Comment entity, object obj = null)
        {
            entity.RegisterDate = DateTime.Now;
            entity.ComputerIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            entity.Active = false;
            _service.Add(entity);
            _service.SaveAllChengeOrAllReject(true);
            return RedirectToAction("ShowMessage", "ShowContent", new { message = "کاربر گرامی از اینکه نظر یا سوال خود را با ما در میان گذاشتید از شما متشکریم. پس از بررسی در سایت نمایش داده می شود." });
            //if (entity.CommentType == (short)CommentType.WorkToUs)
            //    return RedirectToAction("ShowMessage", "ShowContent", new { message = "کاربر گرامی پیشنهاد شما ذخبره گردید و نظر ما به ایمیل شما ارسال خواهد شد  " });
            //else
            //return RedirectToAction("ListQusetions", new { commentType = entity.CommentType });



        }

        public ActionResult GridView()
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

        public ActionResult Edit(Comment entity)
        {
            throw new NotImplementedException();
        }

        #endregion ICURDOperation


    }
}
