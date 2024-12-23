using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Miscellaneous;

namespace DataLayer.Contract
{
    public partial class Bridge_Invoice_ProductContract : IContract
    {
        [Filed("Bridge_Invoice_Product", "MoneySum")]
        public Nullable<global::System.Decimal> MoneySum { get; set; }
        [Filed("Bridge_Invoice_Product", "Price")]
        public Nullable<global::System.Decimal> Price { get; set; }
        //[Filed("Bridge_Invoice_Product", "TotalMoneySum")]
        //public Nullable<global::System.Decimal> TotalMoneySum { get; set; }
    }
}
