using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using DataLayer.Contract;
using ServiceLayer;

using DataLayer.EF;

namespace UILayer.Maper
{
    public partial class Bridge_Invoice_ProductMaper : BaseMaper<BridgeInvoiceProduct, Bridge_Invoice_ProductContract>
    {
         Bridge_Invoice_ProductService _bridge_Invoice_ProductService ; 
         private   InvoiceService  _invoiceService ;
         private   MarketerService  _marketerService;
         private ProductService _productService;

        public Bridge_Invoice_ProductMaper(OnlineShopping objectContext)
        {

            _invoiceService = new InvoiceService(objectContext);

            _marketerService = new MarketerService(objectContext);

            _productService = new ProductService(objectContext);

           _bridge_Invoice_ProductService = new Bridge_Invoice_ProductService(objectContext);

        }

           partial void PartialMethodEntityToContract(ref BridgeInvoiceProduct bridge_Invoice_Product, ref Bridge_Invoice_ProductContract bridge_Invoice_ProductContract)
        {

            bridge_Invoice_ProductContract.Price = bridge_Invoice_Product.FkProductNavigation.Price;
            bridge_Invoice_ProductContract.MoneySum =(decimal ) _bridge_Invoice_ProductService.CalcMoneySumARow((decimal)bridge_Invoice_Product.FkProductNavigation.Price, bridge_Invoice_Product.Count);
         //   bridge_Invoice_ProductContract.TotalMoneySum = bridge_Invoice_ProductContract.TotalMoneySum + bridge_Invoice_ProductContract.MoneySum;
        }
    }
}