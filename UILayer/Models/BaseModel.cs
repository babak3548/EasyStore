using DataLayer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class BaseModel
    {


    }


    public interface IContractModelViews
    {

        UserContract CurrentUserContract { get; set; }
        Dictionary<string, object> OtherParam { get; set; }
        IContract Model { get; set; }
    }
    public class ModelRow : IContractModelViews
    {
        //string message = "";
        //public string Message { get { return message; } set { message = value; } }

        //FK_pram _fK_pram = new FK_pram { PramName = "", PramValue = 0 };
        //public FK_pram fK_pram { get { return _fK_pram; } set { _fK_pram = value; } }


        //string updateId = UpdateTagIds.Grid;
        //public string UpdateId { get { return updateId; } set { updateId = value; } }

        //bool isAjax = true;
        //public bool IsAjax { get { return isAjax; } set { isAjax = value; } }

        //string controlerName = string.Empty;
        //public string ControlerName { get { return controlerName; } set { controlerName = value; } }

        //string partialNameEntity = "";
        //public string PartialNameEntity { get { return partialNameEntity; } set { partialNameEntity = value; } }

        //string controlerNameForm = string.Empty;
        //public string ControlerNameForm { get { return controlerNameForm; } set { controlerNameForm = value; } }

        //string actionName = ActionNames.SaveChange;
        //public string ActionNameForm { get { return actionName; } set { actionName = value; } }

        //string scenarioOrViewNam = string.Empty;
        //public string ScenarioOrViewNam { get { return scenarioOrViewNam; } set { scenarioOrViewNam = value; } }

        //string partialViewName = string.Empty;
        //public string PartialViewName { get { return partialViewName; } set { partialViewName = value; } }

        //string buttomValueOptional = string.Empty;
        //public string ButtomValueOptional { get { return buttomValueOptional; } set { buttomValueOptional = value; } }

        UserContract currentUserContract;
        public UserContract CurrentUserContract { get { return currentUserContract; } set { currentUserContract = value; } }

        Dictionary<string, object> otherParam;
        public Dictionary<string, object> OtherParam { get { return otherParam; } set { otherParam = value; } }

        IContract model;
        public IContract Model { get { return model; } set { model = value; } }

    }
}