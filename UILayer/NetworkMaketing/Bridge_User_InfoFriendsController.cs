using DataLayer;
using ServiceLayer;
using UILayer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.EF;

namespace UILayer.NetworkMaketing
{
    public partial class Bridge_User_InfoFriendsController  : BaseNetworkMarketingController
    {
        //BridgeUserInfoFriends _entity;
        //Bridge_User_InfoFriendsService _service;

        public Bridge_User_InfoFriendsController(OnlineShopping _onlineShopping)
            : base("Bridge_User_InfoFriends",  _onlineShopping)
        {
            //_entity = new Bridge_User_InfoFriends();
            //_service = new Bridge_User_InfoFriendsService(objectContextNetWorkM);
        }





    }
}
