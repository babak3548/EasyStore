using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DataLayer.Contract;
using Utility;


namespace DataLayer.Miscellaneous
{
    public sealed class FiledAttribute : Attribute
    {

        public string FeildName { get; set; }
        public short  OrderByValue { get; set; }
        public short  PartialType { get; set; }
        public string EntityName { get; set; }
        public string LangugeValue { get; set; }
        public string TitleValue { get; set; }
        public static List<FiledContract> FiledContractList { get; set; }
        public FiledAttribute( string entityName,string feildName)
        {
            FeildName = feildName; 
            EntityName = entityName;
            var filed = FiledContractList.FirstOrDefault(f => f.Name == feildName && f.Entity == entityName);
           if (filed != null)
           {
               PartialType = filed.PartialType.ToByte(0);
               LangugeValue = filed.LangugeValue;
               OrderByValue = filed.OrderByValue;
               TitleValue = filed.TitleValue;
           }
        }




    }



}
