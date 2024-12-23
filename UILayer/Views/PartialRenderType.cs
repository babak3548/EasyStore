//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//using DataLayer.Miscellaneous;
//using DataLayer.Enums;
//using DataLayer.Contract;
//using System.Reflection;
//using UILayer.Miscellaneous;

//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace UILayer.Views
//{
//    public class PartialRenderType
//    {
//        IContract model;
//        HtmlHelper htmlHelper;
//        string entityName;
//        ModelRow _modelRow;
//        public PartialRenderType(IContract Model, HtmlHelper HtmlHelper, string EntityName, ModelRow modelRow = null)
//        {
//            model = Model;
//            htmlHelper = HtmlHelper;
//            entityName = EntityName;
//            _modelRow = modelRow;
//        }
//        public void SetModel(IContract Model)
//        {
//            model = Model;
//        }
//        public string GetCurrentLangugeName(PropertyInfo PropertyInfo)
//        {
//            return ((PropertyInfo.GetCustomAttributes(typeof(FiledAttribute), true)[0]) as FiledAttribute).LangugeValue;
//        }

//        public string GetCurrentLangugeName(FieldInfo fieldInfo)
//        {
//            return ((fieldInfo.GetCustomAttributes(typeof(FiledAttribute), true)[0]) as FiledAttribute).LangugeValue;
//        }

//        /// <summary>
//        /// اچ تی ام ال استرینق مورد نظر را بر اساس نوع فیلد در ویرایش فیلدها بر می گرداند
//        /// </summary>
//        /// <param name="PropertyInfo"></param>
//        /// <param name="model"></param>
//        /// <param name="htmlHelper"></param>
//        /// <returns></returns>
//        public MvcHtmlString PartialTypeProperties(PropertyInfo PropertyInfo, int Id,bool ReadOnly = false )//correct model and htmlhelper set in ctor
//        {//تغییراتی که باید اعمال شود 1- همه موردها باید از کلاس مدل پارتیال استفاده کنند
//            //2- شرط ردانلی باید از اول برداشته شود 
//            //3- در صورت امکان دیفالت ولیو به صورت استینق باشد و نیاز به رندر نداشته باشد
//            //var htmlHelper = new HtmlHelper(new ViewContext(), new ViewPage());
//            var filedAttribute = ((PropertyInfo.GetCustomAttributes(typeof(FiledAttribute), true)[0]) as FiledAttribute);
//            //if (filedAttribute.LangugeValue == null) filedAttribute.LangugeValue = "";
//            short partialType = filedAttribute.PartialType;

//            var modelPartial = new ModelPartial { PropertyName = PropertyInfo.Name, langugeValue = filedAttribute.LangugeValue,TitleValue=filedAttribute.TitleValue,
//                value = PropertyInfo.GetValue(model, null) 
//            ,IsReadonly=ReadOnly};
//            if (ReadOnly & partialType !=(byte) PartialType.DropDownEnums) partialType = (short)PartialType.Lable;//در صورتی که دسترسی کاربر جاری فقط دیسپلی باشد در این صورت پارامتر رد  انلی ترو میگردد و به طبع ان نمایش به صورت رد لیبل خواد شد
//            //disabled
//            // var ViewData=new ViewDataDictionary();
//            // htmlHelper.ViewData["PropertyInfo"] = PropertyInfo;<input  type ="checkbox" id="" name="" onblur="" />
//            switch (partialType)
//            {
//                case (short)PartialType.DefaultValue:
//                    {
//                        modelPartial.ValidateMethod = "ValidationWithIdElement('" + PropertyInfo.Name + "','" + UIUtility.ResourceManager.GetString("this_fieled_cannot_null") +
//                            "','" + UIUtility.ResourceManager.GetString("min_value_this_field") +
//                            "','" + UIUtility.ResourceManager.GetString("max_value_this_field") +
//                            "','" + UIUtility.ResourceManager.GetString("this_field_must_be_number") +
//                            "','" + UIUtility.ResourceManager.GetString("NationalCode_Is_Wrong") + "')";
//                    return htmlHelper.Partial(PartialType.DefaultValue.ToString(), modelPartial);}
//                case (short)PartialType.Lable: return htmlHelper.Partial(PartialType.Lable.ToString(), model, new ViewDataDictionary() { { "PropertyInfo", PropertyInfo }, { "langugeValue", filedAttribute.LangugeValue } });
//                case (short)PartialType.Editor: return htmlHelper.Partial(PartialType.Editor.ToString(), model, new ViewDataDictionary() { { "PropertyInfo", PropertyInfo } });
//                case (short)PartialType.Date: return htmlHelper.Partial(PartialType.Date.ToString(), modelPartial);
//                case (short)PartialType.UploadImage: return htmlHelper.Partial(PartialType.UploadImage.ToString(), model, new ViewDataDictionary() { { "PropertyName", PropertyInfo.Name },
//                { "UploadActionName", "UploadImage" }, { "langugeValue", filedAttribute.LangugeValue } });
//                case (short)PartialType.UploadFile: return htmlHelper.Partial(PartialType.UploadImage.ToString(), model, new ViewDataDictionary() { { "PropertyName", PropertyInfo.Name }
//                    , { "UploadActionName", "UploadFile" }, { "langugeValue", filedAttribute.LangugeValue } });
//                case (short)PartialType.DropDownEnums: {
//                    var selectedValue = modelPartial.value == null ? "" : modelPartial.value.ToString();
//                    var enumUtility = new EnumUtility();
//                    modelPartial.value = enumUtility.GetLSelectListItem(PropertyInfo.Name, selectedValue);
//                    return htmlHelper.Partial(PartialType.DropDownWithList.ToString(), modelPartial);
//                }
//                case (short)PartialType.Link: { 
//                return htmlHelper.ActionLink(GetCurrentLangugeName(PropertyInfo), "IndexFK", PropertyInfo.GetValue(model, null).ToString() , new { propName = "FK_" + entityName  , Id = Id }, new { id = "Link" });
//                }
//                case (short)PartialType.CostumLink:
//                    {
//                        var customLinkContract = (CustomLinkContract)PropertyInfo.GetValue(model, null);
//                        return htmlHelper.ActionLink(filedAttribute.LangugeValue, customLinkContract.ActionName, customLinkContract.ControllerName + "Controler", new {  Id =customLinkContract.Id }, new { id = "Link" });
//                    }
//                case (short)PartialType.btnFormSubmit :
//                    {
//                        return new MvcHtmlString("<input id='" + modelPartial.PropertyName + "' type='button' value='" 
//                            + (_modelRow.ButtomValueOptional == "" ? modelPartial.langugeValue :UIUtility.ResourceManager.GetString( _modelRow.ButtomValueOptional)) +  
//                            "' onclick=\"ValidateForm('"+(UIUtility.ResourceManager.GetString("Please_fill_correctly_above_fields"))+"')\" />");      }
//                case (short)PartialType.TextArea :
//                    { return new MvcHtmlString("  <textarea  id='"+ PropertyInfo.Name +"' readonly='readonly' name='"+PropertyInfo.Name+"'> "+PropertyInfo.GetValue(model,null)+"</textarea>    <br />"); }
//                case (short)PartialType.TextAreaField:
//                    {
//                        return new MvcHtmlString(" <label for='" + PropertyInfo.Name + "'>" + filedAttribute.LangugeValue + "</label> <textarea  id='" + PropertyInfo.Name +
//                            "'  onblur='StringValidation(\"" + PropertyInfo.Name + "\",1,500,\"" + UIUtility.ResourceManager.GetString("this_fieled_cannot_null") + "\",\"" + UIUtility.ResourceManager.GetString("min_value_this_field") +
//                            "\",\"" + UIUtility.ResourceManager.GetString("max_value_this_field") + "\")' name='" + PropertyInfo.Name + "'> "
//                        + PropertyInfo.GetValue(model, null) +   "</textarea>    <br />"); }
//            case (short)PartialType.HiddenFiled :  
//                    { return new MvcHtmlString("  <input  id='"+ PropertyInfo.Name +"' type='hidden' name='"+PropertyInfo.Name+"' value= '"+PropertyInfo.GetValue(model,null)+ "' /> "); }
//                case (short)PartialType.CheckBoxChecked:
//                    { return new MvcHtmlString(" <label for='" + PropertyInfo.Name + "'>" + filedAttribute.LangugeValue + "</label> <input  type ='checkbox' value='" + PropertyInfo.GetValue(model, null) + "' id='" + PropertyInfo.Name + "' name='" + PropertyInfo.Name 
//                        + "' onblur='ValidationWithIdElement('" + PropertyInfo.Name + "','" + UIUtility.ResourceManager.GetString("this_fieled_cannot_null") +
//                            "','" + UIUtility.ResourceManager.GetString("min_value_this_field") +
//                            "','" + UIUtility.ResourceManager.GetString("max_value_this_field") +
//                            "','" + UIUtility.ResourceManager.GetString("this_field_must_be_number") +
//                            "','" + UIUtility.ResourceManager.GetString("NationalCode_Is_Wrong") + "')' />    <br />"); }
//                case (short)PartialType.DropDownWithList:
//                    {  var propInfo_FK=model.GetType().GetProperty("FK_" + PropertyInfo.Name.Replace("IEnumerable", ""));
//                    if (propInfo_FK == null) propInfo_FK = model.GetType().GetProperty("Fk_" + PropertyInfo.Name.Replace("IEnumerable", ""));
//                    var objselectvalue = propInfo_FK.GetValue(model, null);
//                        var selectedValue = objselectvalue==null ? "":objselectvalue.ToString();
//                        var LSelectListItem = (((IEnumerable<SelectListItem>)PropertyInfo.GetValue(model, null)).ToList());
//                        if (LSelectListItem.FirstOrDefault(a => a.Value == selectedValue) != null) LSelectListItem.FirstOrDefault(a => a.Value == selectedValue).Selected = true;
//                        modelPartial.value = LSelectListItem;
//                        return htmlHelper.Partial(PartialType.DropDownWithList.ToString(), modelPartial);
//                    }
//                case (short)PartialType.AjaxTextBox: return htmlHelper.Partial(PartialType.AjaxTextBox.ToString(), new ModelPartial
//                {
//                    ActionNameAjax = "GetNameEntityByIdAjax",
//                    ControllerNameAjax = entityName,
//                    langugeValue = filedAttribute.LangugeValue
//                    ,PropertyName=filedAttribute.FeildName });
//                case (short)PartialType.Numeric:
//                    {
//                        modelPartial.ValidateMethod = "ValidationNumber('" + modelPartial.PropertyName + "','"+UIUtility.ResourceManager.GetString("this_field_must_be_number")+"')";
//                    return htmlHelper.Partial(PartialType.DefaultValue.ToString(), modelPartial); }
//                case (short)PartialType.Email:
//                    {
//                        modelPartial.ValidateMethod = "emailValidate('" + modelPartial.PropertyName + "','" + UIUtility.ResourceManager.GetString("this_fieled_cannot_null") + "','"
//                            +UIUtility.ResourceManager.GetString("Enter_a_valid_email_address")+ "')";
//                        return htmlHelper.Partial(PartialType.DefaultValue.ToString(), modelPartial);                }
//                case (short)PartialType.ReEmail:
//                    {
//                        modelPartial.ValidateMethod = "ValidationRepeatField('" + PropertyInfo.Name.Replace("Re", "") + "','" + modelPartial.PropertyName + "','" + (UIUtility.ResourceManager.GetString("Repeat_field_not_correct")) + "')";
//                        return htmlHelper.Partial(PartialType.DefaultValue.ToString(), modelPartial);
//                    }
//                case (short)PartialType.Password: return htmlHelper.Partial(PartialType.Password.ToString(), model, new ViewDataDictionary() { { "PropertyInfo", PropertyInfo }, { "langugeValue", filedAttribute.LangugeValue } 
//                    , { "validateMethod", "StringValidation(\"" + PropertyInfo.Name + "\",1,500,\"" + UIUtility.ResourceManager.GetString("this_fieled_cannot_null") + "\",\"" + UIUtility.ResourceManager.GetString("min_value_this_field") +
//                            "\",\"" + UIUtility.ResourceManager.GetString("max_value_this_field") + "\")" }});
//                case (short)PartialType.RePassword: return htmlHelper.Partial(PartialType.Password.ToString(), model, new ViewDataDictionary() { { "PropertyInfo", PropertyInfo }, { "langugeValue", filedAttribute.LangugeValue } 
//                    , { "validateMethod", "ValidationRepeatField('" + PropertyInfo.Name.Replace("Re","")+"','"+ PropertyInfo.Name +  "','" + (UIUtility.ResourceManager.GetString("Repeat_field_not_correct")) +"')" } });
//                default:
//                    {
//                        modelPartial.ValidateMethod = "ValidationWithIdElement('" + PropertyInfo.Name + "','" + UIUtility.ResourceManager.GetString("this_fieled_cannot_null") +
//                          "','" + UIUtility.ResourceManager.GetString("min_value_this_field") +
//                          "','" + UIUtility.ResourceManager.GetString("max_value_this_field") +
//                          "','" + UIUtility.ResourceManager.GetString("this_field_must_be_number") +
//                          "','" + UIUtility.ResourceManager.GetString("NationalCode_Is_Wrong") + "')"; return htmlHelper.Partial(PartialType.DefaultValue.ToString(), modelPartial);
//                    }
//            }
//            //new ViewContext(ControllerContext, new WebFormView("omg"), new ViewDataDictionary(), new TempDataDictionary()), new ViewPage()); h.TextBox("myname"); 
//        }//
//        /// <summary>
//        /// اچ تی ام ال استرینق مورد نظر را بر اساس نوع فیلد در ساختن فیلتیر فیلدها  بر می گرداند
//        /// </summary>
//        /// <param name="PropertyInfo"></param>
//        /// <returns></returns>
//        public MvcHtmlString PartialTypePropertiesFilter(PropertyInfo PropertyInfo)//correct model and htmlhelper set in ctor
//        {
//            var filedAttribute = ((PropertyInfo.GetCustomAttributes(typeof(FiledAttribute), true)[0]) as FiledAttribute);
//            short partialType = filedAttribute.PartialType;

//            switch (partialType)
//            {
//                case (short)PartialType.Date: return htmlHelper.Partial("FilterDate", model, new ViewDataDictionary() { { "PropertyName", PropertyInfo.Name }, { "langugeValue", filedAttribute.LangugeValue } });

//                default: return htmlHelper.Partial(PartialType.EmptyTextBox.ToString(), new ViewDataDictionary() { { "FiledId", PropertyInfo.Name }, { "langugeValue", filedAttribute.LangugeValue } });
//            }
//            //new ViewContext(ControllerContext, new WebFormView("omg"), new ViewDataDictionary(), new TempDataDictionary()), new ViewPage()); h.TextBox("myname"); 
//        }
//        /// <summary>
//        /// اچ تی ام ال استرینق مورد نظر را بر اساس نوع فیلد در گرید بر می گرداند
//        /// </summary>
//        /// <param name="PropertyInfo"></param>
//        /// <param name="model"></param>
//        /// <param name="htmlHelper"></param>
//        /// <returns></returns>
//        public MvcHtmlString PartialTypePropertiesGrid(PropertyInfo PropertyInfo, int Id)
//        {
//            var filedAttribute = ((PropertyInfo.GetCustomAttributes(typeof(FiledAttribute), true)[0]) as FiledAttribute);
//            short partialType = filedAttribute.PartialType;
//            switch (partialType)
//            {
//                case (short)PartialType.Image: return htmlHelper.Partial(PartialType.Image.ToString(), model, new ViewDataDictionary() { { "PropertyInfo", PropertyInfo } });
//                case (short)PartialType.Link:
//                    {
//                        return htmlHelper.ActionLink(GetCurrentLangugeName(PropertyInfo), "IndexFK", PropertyInfo.GetValue(model, null).ToString(), new { propName = "FK_" + entityName, Id = Id, ViewName = (_modelRow as ModelGrid).LinksScenarioOrViewName }, new { id = "Link" });
//                    }
//                case (short)PartialType.CostumLink:
//                    {
//                        var customLinkContract = (CustomLinkContract)PropertyInfo.GetValue(model, null);
//                        return htmlHelper.ActionLink(filedAttribute.LangugeValue, customLinkContract.ActionName, customLinkContract.ControllerName , new { Id = customLinkContract.Id }, new { id = "Link" });
//                    }
//                default:
//                    {
//                        var value = PropertyInfo.GetValue(model, null);
//                        if (value == null) value = "";
//                        if (value.ToString().Length  > 40) value = value.ToString().Substring(0, 39);
//                        return MvcHtmlString.Create(value.ToString());
//                    }

//            }
//        }
//        public MvcHtmlString PartialTypeIEnmerable(FieldInfo fieldInfo)
//        {
//            var filedAttribute = ((fieldInfo.GetCustomAttributes(typeof(FiledAttribute), true)[0]) as FiledAttribute);
//            short partialType = filedAttribute.PartialType;
//            return htmlHelper.Partial(PartialType.DropDown.ToString(), model, new ViewDataDictionary() { 
//            { "fieldInfo", fieldInfo },
//            { "langugeValue", filedAttribute.LangugeValue } ,
//            {"feildName",filedAttribute.FeildName }});
//        }

//        public MvcHtmlString PartialTypeIEnmerableFilter(FieldInfo fieldInfo)
//        {
//            var filedAttribute = ((fieldInfo.GetCustomAttributes(typeof(FiledAttribute), true)[0]) as FiledAttribute);
//            // short partialType = filedAttribute.PartialType;

//            return htmlHelper.Partial(PartialType.DefaultValue.ToString(), new ViewDataDictionary() { { "FiledId", fieldInfo.Name }, { "langugeValue", filedAttribute.LangugeValue } });

//        }


//        public static MvcHtmlString btnSubmit( string langugeValue)
//        {
//            if (!UIUtility.ResourceManager.GetString(langugeValue).IsEmpty()) langugeValue = UIUtility.ResourceManager.GetString(langugeValue);
//            return new MvcHtmlString("<input id='btnFormSubmit' type='submit' value='" + langugeValue +
//                    "' />");
           
//        }
//        public static MvcHtmlString btnSubmit(string name, string langugeValue,string validateFormMethod)
//        {
//            if (!UIUtility.ResourceManager.GetString(langugeValue).IsEmpty()) langugeValue = UIUtility.ResourceManager.GetString(langugeValue);
//            return new MvcHtmlString("<input id='" + name + "' type='button' value='" + langugeValue +
//                    "' onclick='"+validateFormMethod +"')\" />");
//                    //"' onclick=\""+validateFormMethod+"('" + (UIUtility.ResourceManager.GetString("Please_fill_correctly_above_fields")) + "')\" />");

//        }

//      //  public MvcHtmlString checkbox()
//        //{
//            /*return new MvcHtmlString(" <label for='" + PropertyInfo.Name + "'>" + filedAttribute.LangugeValue + "</label> <input  type ='checkbox' value='" + PropertyInfo.GetValue(model, null) + "' id='" + PropertyInfo.Name + "' name='" + PropertyInfo.Name
//                        + "' onblur='ValidationWithIdElement('" + PropertyInfo.Name + "','" + UIUtility.ResourceManager.GetString("this_fieled_cannot_null") +
//                            "','" + UIUtility.ResourceManager.GetString("min_value_this_field") +
//                            "','" + UIUtility.ResourceManager.GetString("max_value_this_field") +
//                            "','" + UIUtility.ResourceManager.GetString("this_field_must_be_number") +
//                            "','" + UIUtility.ResourceManager.GetString("NationalCode_Is_Wrong") + "')' />    <br />");*/
//       // }

//    }
//}