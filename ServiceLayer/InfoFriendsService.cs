using DataLayer;
using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using AnarSoft.Utility.Utilities;
using System.Web.Script.Serialization;
using DataLayer.EF;
namespace ServiceLayer
{
    public  class InfoFriendsService : BaseService<InfoFriend>
    {

        InfoFriends _entity = new InfoFriends();
        public InfoFriendsService(OnlineShopping OnlineShopping)
            : base(OnlineShopping)
        {
        }
        /// <summary>
        /// ذخیره لیست دوستان و ارتباط آنها
        /// </summary>
        /// <param name="JsonList"></param>
        /// <param name="FK_User_Finder"></param>
        public void SaveListFerindsUser(string JsonList, int FK_User_Finder)
        {
            try
            {
            Bridge_User_InfoFriendsService b_U_I_service = new Bridge_User_InfoFriendsService(_OnlineShopping);

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<InfoFrendJson> personsTotal = js.Deserialize<List<InfoFrendJson>>(JsonList);

           // RemoveRepeatEmails(personsTotal);

            foreach (var friend in personsTotal)//حلقه ذخیره اطلاعات دوستان 
            {
                _entity=null;
                var list = GetAll().ToList();
                _entity = list.FirstOrDefault(i => i.Email.Trim().ToLower() == friend.address.Trim().ToLower());
                    //FirstOrDefault(i => i.Email == friend.address);
                if (_entity == null)//اگر ایمیل قبلا به ثبت نرسیده بود
                {
                    InfoFriends infoFriends = new InfoFriends();
                    infoFriends.Email = friend.address;
                    infoFriends.Name = friend.fullName;
                    infoFriends.FK_User_Finder = FK_User_Finder;
                    Add(infoFriends);
                }
                else//وگرنه یک ارتباط به ایمیل ثبت شده وصل کن
                {
                    //اگر ارتباط قبلا به ثبت نرسیده بود ثبت  کن
                    if (b_U_I_service.FirstOrDefault(b => b.FK_User == FK_User_Finder && b.FK_InfoFriends == _entity.Id) == null)
                    {
                        AddB_U_I(FK_User_Finder, b_U_I_service, _entity.Id);
                    }
                }
            }
            SaveAllChengeOrAllReject(true);

            foreach (var InfoFriend in Find(f => f.FK_User_Finder == FK_User_Finder))
            {
                //اگر ارتباط قبلا به ثبت نرسیده بود ثبت  کن
                if (b_U_I_service.FirstOrDefault(b => b.FK_User == FK_User_Finder && b.FK_InfoFriends == InfoFriend.Id) == null)
                {
                    AddB_U_I(FK_User_Finder, b_U_I_service, InfoFriend.Id);
                }
            }
            SaveAllChengeOrAllReject(true);
            }
            catch (Exception ex)
            {//اگر به هر دلیلی عملیات شکست خورد ادمه نده
                ex = null;
            }
        }

        private static void RemoveRepeatEmails(List<InfoFrendJson> personsTotal)
        {
            for (int i = 0; i < personsTotal.Count; i++)
            {
                for (int j = i + 1; j < personsTotal.Count; j++)
                {
                    if (personsTotal[i].address == personsTotal[j].address)
                    {
                        personsTotal.RemoveAt(j);
                    }
                }

            }
        }

        private static void AddB_U_I(int FK_User_Finder, Bridge_User_InfoFriendsService b_U_I_service, int Id_InfoFriends)
        {
            Bridge_User_InfoFriends B_U_I = new Bridge_User_InfoFriends();
            B_U_I.FK_InfoFriends = Id_InfoFriends;
            B_U_I.FK_User = FK_User_Finder;
            b_U_I_service.Add(B_U_I);
        }
    }

    
    public class InfoFrendJson {
        public string address { get; set; }
        public string fullName { get; set; }
    }
}
