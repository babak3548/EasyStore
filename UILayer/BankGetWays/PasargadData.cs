using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UILayer.BankGetWays
{
    public class PasargadData
    {
        
        public int terminalCode { get; set; }//شماره ترمینال
        public int merchantCode { get; set; }//شماره فروشگاه
        public  string redirectAddress { get; set; }

        public  string action { get; set; }
        public int invoiceNumber { get; set; }
        public string invoiceDate { get; set; }
        public decimal amount { get; set; }
        public string timeStamp { get; set; }
        public string sign { get; set; }
        public string xmlString { get; set; }

    }


}
