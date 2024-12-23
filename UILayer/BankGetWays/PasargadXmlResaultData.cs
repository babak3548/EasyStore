using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.BankGetWays
{
    public class resultObj
    {
        public resultObj()
        {
 result=true;
 action=1;
 invoiceNumber=1;
  invoiceDate="1392/02/02";
 transactionReferenceID=1;
 traceNumber = "1";
 referenceNumber = 1;
 transactionDate = "1392/02/02";
 terminalCode = 1;
 merchantCode = 1;
        }     
       // <?xml version=\"1.0\" encoding=\"utf-8\"?>
//<resultObj>
public bool result;//true|false}</result>
public int action;//{1003|1004}</action>
public int invoiceNumber;//{ فاکتور شماره }</invoiceNumber>
public string  invoiceDate;//>{ فاکتور تاريخ }</invoiceDate>
public int transactionReferenceID;//>{ تراکنش شماره }</transactionReferenceID>
public string traceNumber;//>{ پيگيری شماره }</traceNumber>
public int referenceNumber;//>{ ارجاع شماره }</referenceNumber>
public string transactionDate;//>{ تراکنش تاريخ }</transactionDate>
public int terminalCode;//></terminalCode>
public int merchantCode;//></merchantCode>
//public string /resultObj>
    }
}