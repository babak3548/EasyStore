using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Miscellaneous;

namespace DataLayer.Contract
{
    public partial class InvoiceContract : IContract
    {
        [Filed("Invoice", "CustomLink_Bridge_Invoice_Product")]
        public CustomLinkContract CustomLink_Bridge_Invoice_Product { get; set; } 
    }
}
