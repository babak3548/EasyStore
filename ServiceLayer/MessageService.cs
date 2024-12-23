using DataLayer;
using DataLayer.Enums;
using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.EF;
using  NotifictionService;

namespace ServiceLayer
{
    public partial class MessageService : BaseService<Message>
    {
        const string hedStr1 = "<html><body><div style='direction: rtl; border: 2px solid #D8D7D7; font-size: 10.5pt; font-family: tahoma, arial;line-height: 20pt; float: right;width: 99.5%;'>"
    + " <div style='background-color: rgb(205, 217, 229); text-align: center; width: 100%; padding-top: 7px; padding-bottom: 7px; border-bottom: rgb(193, 193, 194) 2px solid;'>";
   const string hedStr2=  " </div> <div style='text-align: justify; padding: 5px;'> <p>";
   const string footrStr1 = "  </p></div></div></body></html> ";

        public MessageService(OnlineShopping onlineShopping) :base( onlineShopping)
        {

        }
        Message _entity = new Message();

      

        public void RegisterAMessage(int FkUseSender, int FkUserReceiver, string text,short? type=null)
        {
            _entity.Date = Culture.CurrentDate;
            _entity.FkUseSender = FkUseSender;
            _entity.FkUserReceiver = FkUserReceiver;
            _entity.Text = text;
            _entity.Readed = false;
            //_entity.Type = type;
            Add(_entity);
        }

        /// <summary>
        /// ارسال کد فعال سازی به ایمیل
        /// </summary>
        /// <param name="user"></param>
        public void SendActivationCodeToEmail(User user)
        {
      
                var emailSender = new Email(AppSetting.LogFilePathShopping);
 
                string str1 = "<span>کاربر گرامی </span><span>" + user.Name + "</span>   <span>، به فروشگاه ایزی خوش آمده اید</span><br /> ";
                string str2 = "  <a target='_blank' href='"+ AppSetting.DomainName +"/User/ActivateUser/" + user.Id.ToString() + "?ActivateCode=" + user.AtivationCode + "'> برای فعال سازی حساب کاربری بر روی این لینک کلیک نمایید </a>";
                string str3 = "   <br/> <span>یا کد زیرا در سایت وارد نمایید</span><br /> <span>کد فعال سازی:</span><b style='direction: rtl;'>"
                    + user.AtivationCode + "</b>";
                string strBody = str1 + str2 + str3 ;
                SendMessageToEmail(user.Email, "ایمیل فعال سازی حساب کاربری شما در فروشگاه ایزی", strBody);
    
        }
        /// <summary>
        /// ارسال پسورد به ایمیل
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void SendPasswordToEmail(string email, string password)
        {
            try
            {
                var emailSender = new Email(AppSetting.LogFilePathShopping);

                string str1 = "<span>کلمه عبور : </span><b>" + password
                    + "</b>  <br/>  ";
                
                string strBody = str1 ;
                SendMessageToEmail(email, "کلمه عبور جدید در فروشگاه ایزی", strBody);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        //
        public int SendNewPasswordToMobile(string numberGSM, string restorPassword)
        {
            // System.Net.ServicePointManager.Expect100Continue = false; 
            //string strBody = "فروشگاه ایزی کد: " + activateCode;
            return SMSkavenegar.SendOneSMS("restorPassword", numberGSM, restorPassword);
        }
        public int SendActivationCodeToMobile(string numberGSM,string activateCode)
        {
            // System.Net.ServicePointManager.Expect100Continue = false; 
            //string strBody = "فروشگاه ایزی کد: " + activateCode;
          return  SMSkavenegar.SendOneSMS("VerifyPhone", numberGSM, activateCode);
        }


        public int SendPaymentRef(string numberGSM, string refPay)
        {
            // System.Net.ServicePointManager.Expect100Continue = false; 
            //string strBody = "فروشگاه ایزی کد: " + activateCode;
            return SMSkavenegar.SendOneSMS("PaymentRef", numberGSM, refPay);
        }

        /// <summary>
        /// ارسال پیغام با فرمت فروشگاه ایزی
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="strBody"></param>
        public void SendMessageToEmail(string toEmail, string subject, string strBody)
        {
            try
            {
                var emailSender = new Email(AppSetting.LogFilePathShopping);
                string str1 = hedStr1 + subject + hedStr2;
                string str2 = strBody + footrStr1;
                string strBodyFull = str1 + str2;
                emailSender.SendAEmail(toEmail, subject, strBodyFull);
              //  emailSender.SendEmailTest(toEmail, subject, strBodyFull);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// ایجاد و ذخیره ایمیل تبلیغی فروشنده
        /// </summary>
        /// <param name="fkUseSender">آدی کاربری فروشنده</param>
        /// <param name="srcImage">لینک آدرس عکس فروشگاه</param>
        /// <param name="busName">نام فروشگاه</param>
        /// <returns></returns>
        public string CreateMessageAdsBusinessOwner(int fkUseSender, string srcImage, string busName, ref int MessgeId)
        {
            try
            {
                _entity.FkUseSender = fkUseSender;
                _entity.FkUserReceiver = 0;
                _entity.Text = "empty";
                _entity.Readed = true;
                _entity.Date = Culture.CurrentDate;
              //  _entity.Type = (short)TypeMessage.BusinessOwner;

                Add(_entity);
                SaveAllChengeOrAllReject(true);
                
                string str0 = hedStr1 + " فروشگاه " + busName + hedStr2;
                string str1 = "<span>با سلام </span><br />";
                string str2 = " <img src='" + srcImage + "' style='max-width:600px;' /><br />";
                string str3 = "<span>فروش اینترنتی محصولات ما راه اندازی شد از شما دوست گرامی برای بازدید از فروشگاه مان دعوت می نماییم </span><br />";
                string str4 = "<span>لینک مشاهده فروشگاه:</span><a href='" + AppSetting.DomainName + "/" + busName + "'>" + AppSetting.DomainName + "/" + busName + "</a><br />";
                string str5 = "<a style='margin: 2px;padding: 3px;background-color: rgb(123, 160, 209);text-decoration: none;color: black;' href='"
                    + AppSetting.DomainName + "/Message/sendSavedMsgToEmailView/" + _entity.Id + "?title=" + "دعوت به بازدید از فروشگاه"+busName 
                    + "'>ارسال به دوست</a><br />"
                             + " <span>با تشکر از حسن توجه شما</span>";
                string str = str0 + str1 + str2 + str3 + str4 + str5 + footrStr1;
                MessgeId = _entity.Id;

                _entity.Text = str;
               // Add(_entity);
                SaveAllChengeOrAllReject(true);

                return str;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}
