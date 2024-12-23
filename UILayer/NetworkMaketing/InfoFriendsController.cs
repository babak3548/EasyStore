using DataLayer;
using ServiceLayer;
using UILayer.Controllers;
using UILayer.Models;
using Utility;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using DataLayer.EF;

namespace UILayer.NetworkMaketing
{
    public partial class InfoFriendsController : BaseNetworkMarketingController
    {
         public InfoFriendsController(OnlineShopping _onlineShopping)
             : base("InfoFriends" ,  _onlineShopping)
        {

        }

        UserService _userService;
      //  InfoFriendsService _InfoFriendsService;



    }
}
