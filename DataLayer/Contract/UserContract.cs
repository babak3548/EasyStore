using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Miscellaneous;
using DataLayer.Enums;
using Microsoft.AspNetCore.Http;

namespace DataLayer.Contract
{
    public partial class UserContract : IContract
    {
     
        public string Name { get; set; }
        public global::System.Boolean AcceptAgreement { get; set; }

        public global::System.Boolean HaveNewMessage { get; set; }
        public string IpComputerCreator { get; set; }
        public string FkProvince { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public int CountWishes { get; set; }
        public int FkActiveInvoice { get; set; }
        public InvoiveUser InvoiveUser { get; set; }
    }

    public class InvoiveUser {
        public int InvoiceId { get; set; }
        public decimal PaymentToCountinue { get; set; }
        public decimal TotalSumProductPrice { get; set; }
        public List<InvoiveItemUser> InvoiveItemUsers { get; set; }
    }
    public class InvoiveItemUser
    {
        public int BIPId { get; set; }
        public int ProductId { get; set; }
        public string ProductImage{ get; set; }
        public string ProductName{ get; set; }
        public string ProductNameForUrl{ get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }


}
