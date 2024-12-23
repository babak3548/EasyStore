using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UILayer.Miscellaneous;

namespace UILayer.Views
{

    public static class JavaScript
    {
        static string Please_fill_correctly_above_fields = UIUtility.ResourceManager.GetString("Please_fill_correctly_above_fields");
        static string this_fieled_cannot_null = UIUtility.ResourceManager.GetString("this_fieled_cannot_null");
        static string min_value_this_field = UIUtility.ResourceManager.GetString("min_value_this_field");
        static string max_value_this_field = UIUtility.ResourceManager.GetString("max_value_this_field");
        static string MinValueNumber = UIUtility.ResourceManager.GetString("MinValueNumber");
        static string MaxValueNumber = UIUtility.ResourceManager.GetString("MaxValueNumber");
        static string this_field_must_be_number = UIUtility.ResourceManager.GetString("this_field_must_be_number");
        static string Repeat_field_not_correct = "تکرار پسورد اشتباه می باشد ";
        static string Enter_a_valid_email_address = UIUtility.ResourceManager.GetString("Enter_a_valid_email_address");
         static string NationalCode_Is_Wrong = UIUtility.ResourceManager.GetString("NationalCode_Is_Wrong");
         static string NoFindACase = UIUtility.ResourceManager.GetString("NoFindACase");
         static string select_one_state = UIUtility.ResourceManager.GetString("select_one_state");
         static string langugeName = UIUtility.ResourceManager.GetString("langugeName");
         static string langugeCode = UIUtility.ResourceManager.GetString("langugeCode");
         static string Minimum_click_one_checkbox = "حداقل باید یکی از فیلد ها زیر را انتخاب نمایید";
         static string Message_click_one_redio = "یکی از موارد زیر را انتخاب نمایید";
        

        public static string ValidateForm()
        {
            return "ValidateForm('" + Please_fill_correctly_above_fields + "')";
        }

        public static string StringValidation(string ElementId, int MinLength, int MaxLength)
        {
            return " StringValidation('" + ElementId + "', " + MinLength.ToString() + ", " + MaxLength.ToString() + ", '"
                           + this_fieled_cannot_null + "','"
                           + min_value_this_field + "','"
                           + max_value_this_field + "')";
        }
        public static string ValidationCheckBox(string ElementId)
        {
            return "ValidationCheckBox('" + ElementId + "','" + this_fieled_cannot_null + "')";
        }

        public static string drdValidation(string ElementId)
        {
            return "drdValidation('" + ElementId + "','" + this_fieled_cannot_null + "')";

        }

        public static string ValidationCheckBoxs(string ElementId, string ElementId1)
        {
            return "ValidationCheckBoxs('" + ElementId + "','" + ElementId1 + "','" + select_one_state + "')";

        }
        public static string ValidationNumber(string ElementId)
        {
            return "ValidationNumber('" + ElementId + "','" + this_field_must_be_number + "')";

        }


        public static string ValidationNumberLength(string ElementId, int MinLength, int MaxLength)
        {
            return " ValidationNumberLength('" + ElementId + "', " + MinLength.ToString() + ", " + MaxLength.ToString() + ", '"
                           + this_fieled_cannot_null + "','"
                           + min_value_this_field + "','"
                           + max_value_this_field + "','"
                           + this_field_must_be_number + "')";
        }

        public static string ValidationNumberWithMinMax(string ElementId, double MinValue, double MaxValue)
        {
            //ValidationNumberWithMinMax(ElementId,MinValue,MaxValue,this_field_must_be_number, min_value_this_field, max_value_this_field)
            return " ValidationNumberWithMinMax('" + ElementId + "', " + MinValue.ToString() + ", " + MaxValue.ToString() + ", '"
                           + this_field_must_be_number + "','"
                           + MinValueNumber + "','"
                           + MaxValueNumber + "')";
        }
        ///  SumElement(ElementId1, ElementId1, ElementIdResult)
        public static string SumElement(string ElementId1, string ElementId2, string ElementIdResult)
        {
            return "SumElement('" + ElementId1 + "','" + ElementId2 + "','" + ElementIdResult + "')";

        }
        public static string SubElement(string ElementId1, string ElementId2, string ElementIdResult)
        {
            return "SubElement('" + ElementId1 + "','" + ElementId2 + "','" + ElementIdResult + "')";

        }
        //SubElements(ElementIdBase, ElementIdSum, ElementIdSub, ElementIdResult)
        public static string SubElements(string ElementIdBase, string ElementIdSum, string ElementIdSub, string ElementIdResult,string  ElementIdResult1)
        {
            return "SubElements('" + ElementIdBase + "','" + ElementIdSum + "','" + ElementIdSub + "','" + ElementIdResult + "','" + ElementIdResult1 + "')";

        }
        public static string ValidationRepeatField(string ElementId, string reElementId)
        {
            return "ValidationRepeatField('" + ElementId + "','" + reElementId + "','" + Repeat_field_not_correct + "')";

        }

        public static string emailValidate(string ElementId)
        {
            return "emailValidate('" + ElementId + "','" + this_fieled_cannot_null + "','" + Enter_a_valid_email_address + "')";

        }

        /// <summary>
        /// برای اپلود یک فایل استفاده می گردد
        /// </summary>
        /// <param name="action_url"></param>
        /// <param name="resaultElementId"></param>
        /// <param name="InputPropName"></param>
        /// <returns></returns>
        public static string fileUpload(string action_url, string resaultElementId, string InputPropName)
        {
            return "fileUpload(this.form ,'" + action_url + "','" + resaultElementId + "','" + InputPropName + "'); return false;";
            //"fileUpload(this.form,'@Url.Action(UploadActionName, "Upload", new {InputName=InputPropName})','@propName','@InputPropName'); return false;
        }



        public static string ValidationNationalCode(string ElementId)
        {
            return "ValidationNationalCode('" + ElementId + "','" + NationalCode_Is_Wrong + "')";

        }
        //ValdatChkBoxTypSell( Minimum_click_one_checkbox)
        public static string ValdatChkBoxTypSell()
        {
            return "ValdatChkBoxTypSell('" + Minimum_click_one_checkbox + "')";
        }
        public static string AjaxReqestValidate(string ElementId, string urlValue)
        {
            return "AjaxReqestvalidate('"+ElementId+ "','" + urlValue + "','" + NoFindACase + "')";
        }

        public static string AjaxReqestValidate(string ElementId, string urlValue,string ErrorMessage)
        {
            return "AjaxReqestvalidate('" + ElementId + "','" + urlValue + "','" + ErrorMessage + "')";
        }

        public static string AjaxValidateStr(string ElementId, string urlValue, string ErrorMessage)
        {
            return "AjaxValidateStr('" + ElementId + "','" + urlValue + "','" + ErrorMessage + "')";
        }
        public static string AjaxValidateEmailStr(string ElementId, string urlValue, string ErrorMessage)
        {
            return "AjaxValidateEmailStr('" + ElementId + "','" + urlValue + "','" + ErrorMessage + "')";
        }
        public static string AjaxValidateMobilStr(string ElementId, string urlValue, string ErrorMessage)
        {
            return "AjaxValidateEmailStr('" + ElementId + "','" + urlValue + "','" + ErrorMessage + "')";
        }
        //
        /// <summary>
        /// متد بر اساس تغییر ولیو یک دراپ دان مقدار جدید را به صورت ای جکسی به سرور ارسال می کند ونتیجه بر گشت استرینق خواهد بود 
        /// </summary>
        /// <param name="ElementId">ایدی دراپ دان</param>
        /// <param name="urlValue"></param>
        /// <param name="value2"></param>
        /// <param name="ElementIdResult"></param>
        /// <param name="WaitMessage"></param>
        /// <returns></returns>
        public static string AjaxDropdownAtouPostback(string ElementId, string urlValue, int value2, string ElementIdResult, string WaitMessage)
        {
            return "AjaxDropdownAtouPostback('" + ElementId + "','" + urlValue + "'," + value2 + ",'" + ElementIdResult + "','" + WaitMessage + "')";

        }
/// <summary>
/// متد بر اساس تغییر ولیو یک دراپ دان مقدار جدید را به صورت ای جکسی به سرور ارسال می کند ونتیجه بر گشت اچ تی ام ال خواهد بود 
/// </summary>
/// <param name="ElementId"></param>
/// <param name="urlValue"></param>
/// <param name="value2"></param>
/// <param name="ElementIdResult"></param>
/// <param name="WaitMessage"></param>
/// <returns></returns>
        public static string AjaxDrdAtuPostRutHtm(string ElementId, string urlValue, int value2, string ElementIdResult, string WaitMessage)
        {
            return "AjaxDrdAtuPostRutHtm('" + ElementId + "','" + urlValue + "'," + value2 + ",'" + ElementIdResult + "','" + WaitMessage + "')";

        }
        
        public static string ChngSrcVal (string ElementIdSrc,string  ElementIdDes)
        {
            return "ChngSrcVal('" + ElementIdSrc + "','" + ElementIdDes + "')";
        }
        /* AjaxReqestvalidate(ElementId, urlValue, NoFindACase)  WaitMessage
         * ValidationNationalCode(ElementId, NationalCode_Is_Wrong) 
        
            ////////////////////////////////// ajax validation
            ////////////////////////// custom validation  
            errorMessage(ElementId, message) 
            ValidationWithIdElement(ElementId, this_fieled_cannot_null, min_value_this_field, max_value_this_field, this_field_must_be_number, NationalCode_Is_Wrong)
         checkMelliCode(meli_code)  
       */
        public static string AjaxAutocomplete(string ElementId,string urlValue)
        {
          return  "AjaxAutocomplete('"+ElementId+"','"+ urlValue+"')";
        }

        public static string ConfirimMessage(string message)
        {
            return "return ConfirimMessage('" + message + "');";
        }
        //$(sHtml).printElement();
        public static string printElement(string ElementId)
        {
            return "return ConfirimMessage('" + ElementId + "');";
        }
        public static string datePiker(string ElementId)
        {
            return "datePiker('" + ElementId + "','" + langugeName + "','" + langugeCode + "')";
        }
        //AlessThanB(ElementIdA, ElementIdB,message)
        public static string AlessThanB(string ElementIdA,string ElementIdB,string message)
        {
            return "AlessThanB('" + ElementIdA + "','" + ElementIdB + "','" + message + "')";
        }
        public static string ValdatRedioTypSell(string ElementIdAfterShowErr, string ClassSelctor)
        {
            return "ValdatRedioTypSell('" + ElementIdAfterShowErr + "', '" + ClassSelctor + "', '" + Message_click_one_redio + "')";
        }
        //
        public static string ShowInChk(string ElementId, string ShowElemnId)
        {
            return "ShowInChk('" + ElementId + "', '" + ShowElemnId + "')";
        }
        public static string HideInChk(string ElementId, string HidenElemntId)
        {
            return "HideInChk('" + ElementId + "', '" + HidenElemntId + "')";
        }
        public static string VldtChkBoxACls(string ElementIdAfterShowErr, string ClassSelctor)
        {
            return "VldtChkBoxACls('" + ElementIdAfterShowErr + "', '" + ClassSelctor + "', '" + Message_click_one_redio + "')";
        }
    }
}