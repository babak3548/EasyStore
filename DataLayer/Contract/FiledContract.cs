using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Miscellaneous;
namespace DataLayer.Contract
{
    public partial class FiledContract : IContract
    {
         [Filed("Filed", "LangugeValue")] 
        public global::System.String LangugeValue { get; set; }

         [Filed("Filed", "TitleValue")]
         public global::System.String TitleValue { get; set; }

    }
}
