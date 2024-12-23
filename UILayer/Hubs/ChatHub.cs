using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AnarSoft.UILayer.Hubs
{

    [HubName("ChatHub")]
    public class ChatHub : Hub
    {


        #region Methods


        public void syncSesToConnMainW(string sestionUserId, string userName)
        {
            var ConnectionUserIdMainW = Context.ConnectionId;
           var personCurent= ManageSeetionChat.AddConnectionChat(ConnectionUserIdMainW, sestionUserId, userName);
           bool chatWinIsOpen =(personCurent != null && personCurent.UserConnectionIdChatW != "");
           Clients.Client(ConnectionUserIdMainW).chatWinOpen(chatWinIsOpen);
        }

        /// <summary>
        // این متد در زمان لود شدن پنجره چت اجرا می گردد و کانکشن ای دی پنجره چت کاربر ثبت می گردد  
        /// </summary>
        /// <param name="sestionUserId"></param>
        /// <param name="toSestionUserId"></param>
        /// <param name="subject"></param>
        public void syncSesToConnChatWin(string sestionUserId, string toSestionUserId, string subject)
        {
          string connectionId=  Context.ConnectionId;

          Person PersonCurrent = ManageSeetionChat.GetPersonBySestionUserId(sestionUserId);
          Person PersonPartner = ManageSeetionChat.GetPersonBySestionUserId(toSestionUserId);

          PersonCurrent.UserConnectionIdChatW = connectionId;
          //اگر پاتنر هم پنجره چت داشته باشد پیغام ایجاد پنجره چت خصوصی را ایجاد می کند 
          if (PersonPartner.UserConnectionIdChatW != "")
          {
              Clients.Client(PersonCurrent.UserConnectionIdChatW).createWindowPravite(PersonPartner.UserConnectionIdChatW, PersonPartner.UserName, subject, toSestionUserId);
              Clients.Client(PersonPartner.UserConnectionIdChatW).createWindowPravite(PersonCurrent.UserConnectionIdChatW, PersonCurrent.UserName, subject, sestionUserId);
          }//پیغام: "کاربر منتظر باز شدن پنجره چت طرف مقابل باش"
          else { Clients.Client(PersonCurrent.UserConnectionIdChatW).waitPartnerOpWinChat(toSestionUserId); }
        }
        public void requestChatWith(string sestionUserId,string userName, string toSestionUserId, string subject)
        {
            Person PersonCurrent = ManageSeetionChat.GetPersonBySestionUserId(sestionUserId);
            Person PersonPartner = ManageSeetionChat.GetPersonBySestionUserId(toSestionUserId);
           //اگر به هر دلیلی کاربر در خواست کننده از لیست حذف شده بود آن را اضافه می کند
            if (PersonCurrent == null)
            {
                string ConnectionUserIdMainW = Context.ConnectionId;
                PersonCurrent = ManageSeetionChat.AddConnectionChat(ConnectionUserIdMainW, sestionUserId, userName);
            }
            //ایا پارتنر پنجره چت دارد یانه
            if (PersonPartner.UserConnectionIdChatW == "")
            {
                //Clients.Client(PersonPartner.UserConnectionIdMainW).receiveMassage("ttt", "anonan", "rrrrrrrrrrr");
                Clients.Client(PersonPartner.UserConnectionIdMainW).openWindowChat(sestionUserId, subject);
                if (PersonCurrent.UserConnectionIdChatW != "")
                { Clients.Client(PersonCurrent.UserConnectionIdChatW).waitPartnerOpWinChat(toSestionUserId); }
            }
            else if (PersonPartner.UserConnectionIdChatW != "" && PersonCurrent.UserConnectionIdChatW != "")
            {//پیغام ایجاد پنجره چت
                Clients.Client(PersonCurrent.UserConnectionIdChatW).createWindowPravite(PersonPartner.UserConnectionIdChatW, PersonCurrent.UserName, subject,toSestionUserId);
                Clients.Client(PersonPartner.UserConnectionIdChatW).createWindowPravite(PersonCurrent.UserConnectionIdChatW, PersonPartner.UserName, subject,sestionUserId);
            }


        }
        public void receiveMessageServer(string ToUserIdConn, string userName, string Message)
        {
            var UserIdConn = Context.ConnectionId;
          //  Clients.All.receiveMassage(ToUserIdConn, "anonan", Message);
          //  Clients.Caller.receiveMessage(ToUserIdConn, userName, Message);
            Clients.Client(UserIdConn).receiveMessage(ToUserIdConn, userName, Message);
            Clients.Client(ToUserIdConn).receiveMessage(UserIdConn, userName, Message);

        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {

            string connectionId = Context.ConnectionId;
            
            //string RemoveUserConnectionIdMainW = "";
            ManageSeetionChat.ThisConnectionIdIsDisconnect(connectionId);
            //if (RemoveUserName != "")
            //{
            //    Clients.All.onUserDisconnected(connectionId, RemoveUserName);
            //}

            return base.OnDisconnected();
        }

        public static void UserLogOut(string UserSestionId)
        {
            ManageSeetionChat.UserLogOut(UserSestionId);
            // Clients.All.onUserDisconnected(connectionId, RemoveUserName);
        }
        #endregion

        #region private Messages



        #endregion

        //public override System.Threading.Tasks.Task OnConnected()
        //{
        //    string clientId = GetClientId();


        //    Context.ConnectionId

        //    if (seetionChats.IndexOf(clientId) == -1)
        //    {
        //        seetionChats.Add(clientId);
        //    }

        //    ShowUsersOnLine();

        //    return base.OnConnected();
        //}

        //public override System.Threading.Tasks.Task OnReconnected()
        //{
        //    string clientId = GetClientId();
        //    if (seetionChats.IndexOf(clientId) == -1)
        //    {
        //        seetionChats.Add(clientId);
        //    }

        //    ShowUsersOnLine();

        //    return base.OnReconnected();
        //}

        //public override System.Threading.Tasks.Task OnDisconnected()
        //{
        //    string clientId = GetClientId();

        //    if (seetionChats.IndexOf(clientId) > -1)
        //    {
        //        seetionChats.Remove(clientId);
        //    }

        //    ShowUsersOnLine();

        //    return base.OnDisconnected();
        //}


        //private string GetClientId()
        //{
        //    string clientId = "";
        //    if (!(Context.QueryString["clientId"] == null))
        //    {
        //        //clientId passed from application
        //        clientId = Context.QueryString["clientId"].ToString();
        //    }

        //    if (clientId.Trim() == "")
        //    {
        //        //default clientId: connectionId
        //        clientId = Context.ConnectionId;
        //    }
        //    return clientId;

        //}

        //public void Log(string message)
        //{
        //    Clients.All.log(message);
        //}

        //public void ShowUsersOnLine()
        //{
        //    Clients.All.showUsersOnLine(seetionChats.Count);
        //}



    }
}