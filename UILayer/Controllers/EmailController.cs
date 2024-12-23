//using DataLayer.Enums;
//using ServiceLayer;
//using Utility;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Microsoft.AspNetCore.Mvc;
//using DataLayer.EF;

//namespace UILayer.Controllers
//{
//    public class EmailController : Base0Controller
//    {
//        public EmailController( OnlineShopping _onlineShopping)
//            : base("Email" ,  _onlineShopping)
//        {

//        }

//        public ActionResult Index()
//        {
//            return View();
//        }
//        /*
//        public ActionResult SendEmailsView()
//        {
//            ValidateAccessToAction(RolesSystem.AdminValue);
//            return View();
//        }

//        public ActionResult SendEmailsByOneToOneView()
//        {
//            ValidateAccessToAction(RolesSystem.AdminValue);
//            return View();
//        }

//        public ActionResult SendEmails(string ListEmails, string subject, int ShowContentId)
//        {
//            ValidateAccessToAction(RolesSystem.AdminValue);

//            List<string> ListEmail = new List<string>();
//            //جمیل سبند کانسترکتور
//          //  Email emailSender = new Email(Server.MapPath("~") + Paths.LogPath + Paths.ErrorLogFileName, true);
//           //یا هو یا میل سرور داخلی  سند کانسترکتور
//            Email emailSender = new Email(Server.MapPath("~") + Paths.LogPath + Paths.ErrorLogFileName);
//            string notValidEmails = "";
//            string emailAdressTrimed = "";
//            ContentService contentService = new ContentService(objectContext);
//            ListEmails = ListEmails.Replace("\r\n", ",");
//            foreach (var emailAdress in ListEmails.Split(','))
//            {
//                emailAdressTrimed = emailAdress.Trim();
//                if (emailSender.IsValidEmail(emailAdressTrimed)) ListEmail.Add(emailAdressTrimed);
//                else notValidEmails += (emailAdress + ",");
//            }


//            emailSender.SendGroupEmail(ListEmail, subject, contentService.FirstOrDefault(i => i.Id == ShowContentId).ShowValue);
//            return RedirectToAction("ShowMessage", "ShowContent", new
//            {
//                message = ". ارسال انجام شد به غیر از لیست زیر که فرمت آنها نادرست است" + notValidEmails
//                    + "تعداد ایمیل ارسالی" + ListEmail.Count
//            });
//        }


//        public ActionResult SendEmailsOneToOne(string ListEmails, string subject, int ShowContentId)
//        {
//            ValidateAccessToAction(RolesSystem.AdminValue);

//            int CountList = 0;
//            Email emailSender = new Email(Server.MapPath("~") + Paths.LogPath + Paths.ErrorLogFileName);//, true
//            string notValidEmails = "";
//            string emailAdressTrimed = "";
//            ContentService contentService = new ContentService(objectContext);
//            ListEmails = ListEmails.Replace("\r\n", ",");
//            string body = contentService.FirstOrDefault(i => i.Id == ShowContentId).ShowValue;
//            foreach (var emailAdress in ListEmails.Split(','))
//            {

//                emailAdressTrimed = emailAdress.Trim();
//                if (emailSender.IsValidEmail(emailAdressTrimed))
//                {
//                    emailSender.SendAEmail(emailAdressTrimed, subject, body);
//                    CountList++;
//                }
//                else notValidEmails += (emailAdress + ",");
//            }



//            return RedirectToAction("ShowMessage", "ShowContent", new
//            {
//                message = ". ارسال انجام شد به غیر از لیست زیر که فرمت آنها نادرست است" + notValidEmails
//                    + "تعداد ایمیل ارسالی" + CountList
//            });
//        }
//         */
//    }
//}
