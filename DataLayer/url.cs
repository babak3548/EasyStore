using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace DataLayer
{
  //   [Serializable]
    [XmlRoot("urlset")]
   public class url
    {
      // [DataMember()]
       //[XmlElement("urlloc")]
        [XmlElement(Order = 1)] 
        public string loc { get; set; }

        [XmlElement(Order = 2)] 
        public string lastmod { get; set; }

        [XmlElement(Order = 3)] 
        public string changefreq { get; set; }

        [XmlElement(Order = 4)] 
        public string priority { get; set; }

    }
}
 //  <loc>
 //<lastmod>
 //<changefreq>
 //<priority>