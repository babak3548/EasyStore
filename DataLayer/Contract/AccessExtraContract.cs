using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Contract
{
   public  class AccessExtraContract
    {
      
        public global::System.Int32 Id { get; set; }

        public global::System.Int64 DisplayMode { get; set; }

        public string RoleName { get; set; }

        public string FiledName { get; set; }

        public string EntityName { get; set; }
    }
}
