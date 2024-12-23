using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class MangerModel
    {//@Html.Partial(Model.MenuPartialName)
        public string Title { get; set; }
        public string MenuPartialName { get; set; }
        public string BodyPartialName { get; set; }
        public object Entity { get; set; }
    }
}