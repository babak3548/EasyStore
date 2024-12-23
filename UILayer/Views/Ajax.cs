using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UILayer.Views
{
    //UILayer.Views
    public static class Ajax
    {
        public static IHtmlContent ActionLink(string linkText, string ajax_url, AjaxOption ajaxOption, string htmlAttributes)
        { //( string v1, string v2, string v3, object p1, object ajaxOption, object p2)

            string str = string.Concat(@" <a ",
" data-ajax='true'",
ajaxOption == null || string.IsNullOrWhiteSpace(ajaxOption.Confirm) ? "" : " data-ajax-confirm = '" + ajaxOption.Confirm + "'",
ajaxOption == null || string.IsNullOrWhiteSpace(ajaxOption.HttpMethod) ? "" : " data-ajax-method = " + "'" + ajaxOption.HttpMethod + "'",
" data-ajax-mode='replace'",
" data-ajax-loading-duration =10 ",
ajaxOption == null || string.IsNullOrWhiteSpace(ajaxOption.LoadingElementId) ? "" : (" data-ajax-loading = " + "'#" + ajaxOption.LoadingElementId + "'"),
ajaxOption == null || string.IsNullOrWhiteSpace(ajaxOption.JsFunc_data_ajax_begin) ? "" : (" data-ajax-begin = " + "'" + ajaxOption.JsFunc_data_ajax_begin + "'"),
ajaxOption == null || string.IsNullOrWhiteSpace(ajaxOption.JsFunc_data_ajax_complete) ? "" : (" data-ajax-complete= " + "'" + ajaxOption.JsFunc_data_ajax_complete + "'"),
ajaxOption == null || string.IsNullOrWhiteSpace(ajaxOption.JsFunc_data_ajax_failure) ? "" : (" data-ajax-failure=" + "'" + ajaxOption.JsFunc_data_ajax_failure + "'"),
ajaxOption == null || string.IsNullOrWhiteSpace(ajaxOption.JsFunc_data_ajax_success) ? "" : (" data-ajax-success= " + "'" + ajaxOption.JsFunc_data_ajax_success + "'"),
string.IsNullOrWhiteSpace(ajaxOption.UpdateTargetId) ? "" : (" data-ajax-update = " + "'#" + ajaxOption.UpdateTargetId + "'"),
string.IsNullOrWhiteSpace(ajax_url) ? "" : (" data-ajax-url  = " + "'" + ajax_url + "'"),
string.IsNullOrWhiteSpace(htmlAttributes) ? "" : htmlAttributes,
" >",
linkText,
" </a> ");
            var htmlString = new HtmlString(str);

            return htmlString;
            //string.IsNullOrWhiteSpace(ajaxOption.Confirm) ? "" : " class='TopIcons'" ,
            //'@Url.Action('_EditBid', 'Bids', new { bidId = Model.BidId, bidType = Model.BidTypeName })'"   
        }

        public static IHtmlContent ActionLink(string linkText, string ajax_url, AjaxOption ajaxOption)
        {
            return ActionLink( linkText,  ajax_url,  ajaxOption , null);
            //throw new NotImplementedException();
        }
    }

    public class AjaxOption
    {
        public string HttpMethod { get; set; }
        public string UpdateTargetId { get; set; }
        public string Confirm { get; set; }
        public string LoadingElementId { get; set; }

        public string JsFunc_data_ajax_begin { get; set; }
        public string JsFunc_data_ajax_complete { get; set; }
        public string JsFunc_data_ajax_failure { get; set; }
        public string JsFunc_data_ajax_success { get; set; }
        public string JsFunc_data_ajax_update { get; set; }

        //data-ajax-begin
    }
}
