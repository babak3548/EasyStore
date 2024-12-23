using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Views
{
    public class UploadFileModel
    {
        /// <summary>
        /// نام فیلد 
        /// </summary>
        public string propName;

        /// <summary>
        /// این فیلد برای نوع فایل آپلودی پر می شود 
        /// </summary>
        public string UploadActionName;
        public string UploadControlName = "Upload";

        public string langugeValue;
        /// <summary>
        /// propName + "Input"
        /// </summary>
        public string InputPropName;
        public string Value;
       
        public string  OnburMethod="";

    }
}