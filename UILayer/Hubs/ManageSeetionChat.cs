using DataLayer.Contract;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnarSoft.UILayer.Hubs
{
    public static class ManageSeetionChat
    {
        //static List<SeetionChat> seetionChats = new List<SeetionChat>();
        static List<Person> PersonChats = new List<Person>();
        public static int CountAddedPerson = 0;
        /// <summary>
        /// کاربر وارد شده را به لیس کاربر ان آنلاین اضافه می نمایید
        /// </summary>
        /// <param name="userConnectionIdMainW"></param>
        /// <param name="userSestionId"></param>
        /// <param name="userName"></param>
        public static Person AddConnectionChat(string userConnectionIdMainW, string userSestionId, string userName)
        {
            Person person = PersonChats.FirstOrDefault(p => p.UserSestionId == userSestionId);
            if (person != null)
            {
                person.UserConnectionIdMainW = userConnectionIdMainW;
            }
            else
            {//اگر بر اثر بسته شدن پنجره فعال اصلی سیشن مورد نظر از لیست پاک بشود در حالت فعال شدن
             //   تب اصلی دیگر این سیشن دوباره به لیست اضافه می گردد
                person = new Person { UserSestionId = userSestionId, UserName = userName, UserConnectionIdMainW = userConnectionIdMainW };
                AddPerson(person);
            }
            return person;
        }
        public static List<Person> GetAll()
        {
            return PersonChats;
        }
        private static void AddPerson(Person person)
        {
            CountAddedPerson++;
            person.ConnectDate = DateTime.Now.ToString();
            PersonChats.Add(person);
            NotificationNewUserConnect(person.UserSestionId);
        }

        public static bool ThisUserIsLive(string userSestionId)
        {
            return PersonChats.Any(i => i.UserSestionId == userSestionId);
        }
        //public static string GetUserSestionIdByUserConnectionId(string userConnectionId)
        //{
        //    return PersonChats.FirstOrDefault(i => i.UserConnectionIdMainW == userConnectionId).UserSestionId;
        //}

        internal static Person GetPersonBySestionUserId(string sestionUserId)
        {
            return PersonChats.FirstOrDefault(i => i.UserSestionId == sestionUserId);
        }

        internal static string GetConnectionIdChatW(string sestionUserId)
        {
            return PersonChats.FirstOrDefault(i => i.UserSestionId == sestionUserId).UserConnectionIdChatW;

        }

        internal static string GetConnectionIdMainW(string sestionUserId)
        {
            return PersonChats.FirstOrDefault(i => i.UserSestionId == sestionUserId).UserConnectionIdMainW;

        }
        /// <summary>
        /// اگر کاربر پنجره چت را ببندد کانکشن او را پنجره چت او را پاک کرده و نام کاربر را بر می گرداند
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        internal static void ThisConnectionIdIsDisconnect(string connectionId)
        {
           // string userName = "";
            Person personChatW = PersonChats.FirstOrDefault(p => p.UserConnectionIdChatW == connectionId);
            Person personMainW = PersonChats.FirstOrDefault(p => p.UserConnectionIdMainW == connectionId);
            if (personChatW != null)
            {
                personChatW.UserConnectionIdChatW = "";
                //userName = personChatW.UserName;
                //کاربر اگر پنجره اصلی را هم بسته بود سیشن وی را پاک کن
                if (personChatW.UserConnectionIdMainW == "")
                {
                    RemovePerson(personChatW);

                }

                NotificationUserCloseChatWindow(connectionId,personChatW.UserName);
                                  
            }
            if (personMainW != null)
            {
                personMainW.UserConnectionIdMainW = "";
             //کاربر اگر پنجره چت را هم بسته بود سیشن وی را پاک کن
                if (personMainW.UserConnectionIdChatW == "")
                {
                   RemovePerson(personMainW);
                }
            }
            //return userName;
        }

        private static string RemovePerson(Person personChatW)
        {
            PersonChats.Remove(personChatW);
            NotificationUserLeave(personChatW.UserSestionId);
            return personChatW.UserSestionId;
        }

        internal static void UserLogOut(string UserSestionId)
        {
           Person person= PersonChats.FirstOrDefault(p => p.UserSestionId == UserSestionId);
           PersonChats.Remove(person);
        }



        internal static void ChangeSestionUserId(string SestionUserIdOld, string SestionUserIdNew,string userName)
        {
          Person person=  PersonChats.FirstOrDefault(p => p.UserSestionId == SestionUserIdOld);
          if (person != null)
          {
              person.UserSestionId = SestionUserIdNew;
              person.UserName = userName;
              NotificationChengUser(SestionUserIdOld,SestionUserIdNew);
          }
          
        }
        private static void NotificationChengUser(string SestionUserIdOld, string SestionUserIdNew)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //context.Clients.All.userConnect(SestionUserIdNew);icon chatOn
            context.Clients.All.userChenge(SestionUserIdOld,SestionUserIdNew);
        }
        private static void NotificationNewUserConnect(string SestionUserIdNew)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //context.Clients.All.userConnect(SestionUserIdNew);icon chatOn
            context.Clients.All.userConnect(SestionUserIdNew);
        }
        private static void NotificationUserLeave(string SestionUserId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.userLeave(SestionUserId);
        }
        private static void NotificationUserCloseChatWindow(string ConnectionIdChatWin,string RemoveUserName)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.userCloseChatWindow(ConnectionIdChatWin,RemoveUserName);
            
        }
        internal static void AddSestionUserId(string sestionUserId, string userName)
        {
            Person person= PersonChats.FirstOrDefault(p => p.UserSestionId == sestionUserId);
            if (person == null)
            {
                AddPerson(new Person { UserSestionId = sestionUserId, UserName = userName });
            }
        }
    }
}