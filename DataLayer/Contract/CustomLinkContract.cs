using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Contract
{

   public class CustomLinkContract
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
    }
}
