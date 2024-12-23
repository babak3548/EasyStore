using DataLayer.EF;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UILayer.Maper;

namespace UILayer.Models
{
    public class SearchResultModel
    {
     
     
        public string TitelSearch;
        public SearchModel SearchModel;
        public short RowCount;
     
    
        public IEnumerable<Product> Model;
        public string StoreName = "";
        

     
    }
}