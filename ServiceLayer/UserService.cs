using DataLayer;
using DataLayer.Enums;
using System;
using System.Collections.Generic;
//using System.Data.Objects;
using System.Linq;
using System.Text;
using DataLayer.EF;
namespace ServiceLayer
{
   public partial class UserService :BaseService<User>
    {
       InvoiceService _invoiceService ;
          //_invoiceService
       public UserService(OnlineShopping OnlineShopping)
            : base(OnlineShopping)
       {
           _invoiceService = new InvoiceService(OnlineShopping);
       }
       public IEnumerable<Invoice> FindMyLastInvoice(int userId)
       {
           return _invoiceService.Find(i => i.FkUser == userId &&  i.Status == InvoiceStatus.initialize);
       }

        public object SaveInfoUser(int userId, Gender idGender, string firstName, string lastName, string emailName,
            string birthday, bool spicialOffer, bool newsletter)
        {
            var user = FirstOrDefault(u=>u.Id== userId);
            user.Name = firstName ;
            user.Family = lastName;
            user.Gender = idGender;
            user.Email = emailName;
            user.Newsletter = newsletter;
            user.SpicialOffer = spicialOffer;
            try
            {
               user.Birthday = birthday.PersianToDateTime();
            }
            catch (Exception) {  }           
            
             
            user.Name = firstName;
            user.Name = firstName;
            SaveAllChengeOrAllReject(true);

            return user;
        }

        public bool ChangePassword(int id, string password, string newPassword)
        {
            var user = FirstOrDefault(u => u.Id == id);

           if( user.Password == password.MD5Hash())
            {
                user.Password = newPassword.MD5Hash();
                SaveAllChengeOrAllReject(true);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
