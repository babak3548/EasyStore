using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Enums;
using Utility;
using DataLayer.Contract;
using UILayer.Views;
using DataLayer.Miscellaneous;
using System.Resources;
using System.Reflection;
using DataLayer.EF;
using UILayer.Models;

namespace UILayer.Miscellaneous
{
    
     public  class MyResourceManager : ResourceManager
    {
        static List<MultiLanguageModel> multiLanguages;
        public static List<MultiLanguageModel> MultiLanguages
        {
            get
            {
                if (multiLanguages == null)
                {
                    multiLanguages = UIUtility.MultiLanguages;
                }
                return multiLanguages;
            }
        }

        public MyResourceManager()
        {
           
        }
  
        public override string GetString(string key)
        {
            if (key == "Price") return "قیمت";
           // else return key;
       //  var oldMultiLanguage=   MultiLanguages.FirstOrDefault(m => m.KeyLanguage == key);
            //if (oldMultiLanguage == null)
            //{
            //    var multiLanguage = new MultiLanguage { KeyLanguage = key };
            //    UIUtility.OnlineShopping.MultiLanguage.Add(multiLanguage);
            //    UIUtility.OnlineShopping.SaveChanges();
            //}
            var ml = MultiLanguages.FirstOrDefault(m => m.KeyLanguage == key);
            if (ml!= null && ml.PersianValue != null)
              return  ml.PersianValue;
          else
            return key;
        }
    }
    public static class UIUtility
    {
        public static List<MultiLanguageModel> MultiLanguages { get; set; }
      //  public static OnlineShopping OnlineShopping { get; set; }
          
        //public static string  ErrorMessage { get; set; }
        private static MyResourceManager resourceMan;
        public static string CurrentCulterName { get; set; }

        //public static string ViewName = ScenarioOrViewName.AdminView;
        public static bool CurrentCulter(string CulterName)
        {
            if (CulterName == CurrentCulterName) return true;
            else return false;
        }

        public static string CurrentDate
        {
            get
            { 
                if(CurrentCulterName == "en") return DateTime.Now.ToShortDateString();
                else return Common.GergorianToPersionString(DateTime.Now); 
            }
        }

        public static string CurrentTime
        {
            get
            {
                if (CurrentCulterName == "en") return DateTime.Now.ToShortDateString();
                else return DateTime.Now.TimeOfDay.ToString().Substring(0, 5);
            }
        }


        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static MyResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {  // فرهنگ پیش فرض فارسی می  باشد
                    MyResourceManager temp = new MyResourceManager();
                    resourceMan = temp;
                    CurrentCulterName = "fa";
                }
                return resourceMan;
            }
        }
        /// <summary>
        /// با لود شدن این متد زبان و فرهنگ سیستم تغییر می کند
        /// </summary>
        /// <param name="CulterName">زبان مورد نظر</param>
        public static void LoadCurrentCulter(String CulterName)
        {
            switch (CulterName)
            {
                case "en": { resourceMan = new MyResourceManager(); CurrentCulterName = "en"; break; }
                case "fa": { resourceMan = new MyResourceManager(); CurrentCulterName = "fa"; break; }

                default: { resourceMan = new MyResourceManager(); CurrentCulterName = "fa"; break; }
            }

        }
        public static string CurrentLangugeName
        {
            get
            {
                if (CurrentCulterName == "en") return "english";
                else return "persian";
            }
        }

        public static string CssFardRow(int IRow)
        {
            if ((IRow % 2) == 1) return "RFard";
            else return "";
        }


    }
    /// <summary>
    /// نام و مقدار کلید خارجی
    /// </summary>
    public class FK_pram
    {
        public string PramName { get; set; }
        public int PramValue { get; set; }
    }

    class CustomLinkModel
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
    }

    public class SearchResultContract
    {
        public int Id { get; set; }
        public string ImageAddress { get; set; }
        public string Name { get; set; }
        public string LinkActionName { get; set; }
        public string LinkControlerName { get; set; }
        public Dictionary<string, string> columesDataLong = new Dictionary<string, string>();
        public Dictionary<string, string> columesDataShort = new Dictionary<string, string>();

    }

    public class HtmlActionValues
    {
        public string ActionName { get; set; }
        public string ControlerName { get; set; }
         public object RouteValues  { get; set; }
    }
    public interface IModelViews
    {
         string Message{get;set;}
         FK_pram fK_pram { get; set; }
         string PartialNameEntity { get; set; }
         string UpdateId { get; set; }
         bool IsAjax { get; set; }
         string ControlerName { get; set; }
         string ControlerNameForm { get; set; }
         string ActionNameForm { get; set; }
         string ScenarioOrViewNam { get; set; }
         string PartialViewName { get; set; }
         string ButtomValueOptional { get; set; }
         UserContract CurrentUserContract { get; set; }
         Dictionary<string, object> DicFilterParam { get; set; }
         object Model { get; set; }
    }
    public class ModelRow : IModelViews
    {
         string message = "";
         public string Message { get { return message; } set { message = value; } }

         FK_pram _fK_pram = new FK_pram { PramName = "", PramValue = 0 };
         public FK_pram fK_pram { get { return _fK_pram; } set { _fK_pram = value; } }


          string updateId = UpdateTagIds.Grid;
         public string UpdateId  { get { return updateId; } set { updateId = value; } }

          bool isAjax = true;
         public bool IsAjax   { get { return isAjax; } set { isAjax = value; } }

          string controlerName = string.Empty;
         public string ControlerName { get { return controlerName; } set { controlerName = value; } }

         string partialNameEntity = "";
         public string PartialNameEntity { get { return partialNameEntity; } set { partialNameEntity = value; } }

          string controlerNameForm = string.Empty;
         public string ControlerNameForm { get { return controlerNameForm; } set { controlerNameForm = value; } }

           string actionName = ActionNames.SaveChange;
         public string ActionNameForm { get { return actionName; } set { actionName = value; } }

          string scenarioOrViewNam = string.Empty;
          public string ScenarioOrViewNam { get { return scenarioOrViewNam; } set { scenarioOrViewNam = value; } }

           string partialViewName = string.Empty;
          public string PartialViewName { get { return partialViewName; } set { partialViewName = value; } }

          string buttomValueOptional = string.Empty;
          public string ButtomValueOptional { get { return buttomValueOptional; } set { buttomValueOptional = value; } }

            UserContract currentUserContract;
          public UserContract CurrentUserContract { get { return currentUserContract; } set { currentUserContract = value; } }

            Dictionary<string, object> dicFilterParam;
          public Dictionary<string, object> DicFilterParam { get { return dicFilterParam; } set { dicFilterParam = value; } }

            object model;
            public object Model { get { return model; } set { model = value; } }
        
    }

    public class ModelGrid : ModelRow
    {
        public string UpdateIdGrid ;
        public bool IsAjax2 = true;
        public string EditActionName = ActionNames.GetRow;
        public string DeleteActionName = ActionNames.DeleteRow;
        public string AddActionName = ActionNames.GetRow;
        public string PageNumberActionName = ActionNames.GridFK;
        public string LinksScenarioOrViewName = ScenarioOrViewName.AdminView;


        public int BeginRow;

     

        public ModelGrid()
        {
            UpdateId = UpdateTagIds.EditorRow;
            UpdateIdGrid = UpdateTagIds.Grid;
        }
    }

    public class ModelFillter : ModelRow
    {
     // public   PartialRenderType PartialRenderType;
    }

    public class ModelShowContent
    {
        public IEnumerable<ContentContract> CategoryContent;
        public IEnumerable<ContentContract> CenterContent;

    }

    public class  ModelPartial
    {
       public string PropertyName;
       public string langugeValue;
       public string TitleValue;

       public bool IsReadonly;

       public string ValidateMethod;

       public string ActionNameAjax;
       public string ControllerNameAjax;

       public object value;

    }
}