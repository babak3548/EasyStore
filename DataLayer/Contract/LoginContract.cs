using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Miscellaneous;

namespace DataLayer.Contract
{
    public class LoginContract : IContract
    {
        [Filed("User", "Id")]
        public int Id { get; set; }

         [Filed("User", "Email")] 
        public global::System.String Email { get; set; }

       [Filed("User", "Password")]
        public global::System.String Password { get; set; }


       [Filed("User", "btnFormSubmit")]
       public string btnFormSubmit { get { return "btnFormSubmit"; } } 
       // [Filed("LoginContract", "IncorrectEmailOrPassWord")]
       //public string IncorrectEmailOrPassWord { get; set; }
    }
}
