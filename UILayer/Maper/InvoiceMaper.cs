using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using DataLayer.EF;
using DataLayer.Contract;
using ServiceLayer;
using Microsoft.AspNetCore.Http;

namespace UILayer.Maper
{
   
    public partial class InvoiceMaper : BaseMaper<Invoice, InvoiceContract>
    {
        Bridge_Invoice_ProductService _bridge_Invoice_ProductService; 
        public InvoiceMaper(OnlineShopping objectContext)
        {
            _businessownerService = new BusinessOwnerService(objectContext);

            _userService = new UserService(objectContext);
            _bridge_Invoice_ProductService = new Bridge_Invoice_ProductService(objectContext);
        }



        partial void PartialMethodEntityToContract(ref Invoice invoice, ref InvoiceContract invoiceContract)
        {
            invoiceContract.CustomLink_Bridge_Invoice_Product = new CustomLinkContract { Id = invoice.Id, ActionName = "AInvoice", ControllerName = "Invoice" };
        }

        partial void PartialMethodFormCollectionToEntity(ref FormCollection formCollection, ref Invoice invoice)
        {
            invoice.FkUser =( invoice.FkUser==0 ?  Convert.ToInt32(formCollection["FK_User"]) : invoice.FkUser);
            invoice.FkBusinessOwner = (invoice.FkBusinessOwner == 0 ? Convert.ToInt32(formCollection["FK_BusinessOwner"]) : invoice.FkBusinessOwner);
        }
    }
}