using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Common
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }
        public AuthorizeUserAttribute(HttpContext httpContext)
        {

        }

        protected  bool AuthorizeUser(HttpContext httpContext)
        {
            
            var isAuthorized = true;
            if (!isAuthorized)
            {
                return false;
            }

            string privilegeLevels = ""; //string.Join("", GetUserRights(httpContext.User.Identity.Name.ToString())); // Call another method to get rights of the user from DB

            return privilegeLevels.Contains(this.AccessLevel);
        }
    }
}
