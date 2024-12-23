using DataLayer.Enums;
using UILayer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace AnarSoft.UILayer.Hubs
{
    public class HubMangmentController : Base0Controller
    {
        //
        // GET: /HubMangment/
        public HubMangmentController()
            : base("HubMangment")
        {

        }
        public ActionResult listentities()
        {
            ValidateAccessToAction(RolesSystem.AdminValue);
            return View( );
        }

    }
}
