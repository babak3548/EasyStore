using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace DataLayer
{
  // [Serializable]
  //  
   
   // [XmlRoot("urlset")]
 //   [DataContract(Name = "urlset")]
 //   [XmlRootAttribute(ElementName = "urlset", IsNullable = false)]
    [XmlType("link")]
   
   public class urlset :List<url>
     {
     //  public Employee[] Employees;
      // public List<url> urls;
         //[DataMember()]
         //public virtual List<url> urls
         //{
         //    get
         //    {
         //        if (_urls == null)
         //        {
         //            _urls = new List<url>();

         //        }
         //        return _urls;
         //    }
         //    set { _urls = value; }
         //}
    }
}
