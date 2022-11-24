using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Tree
{
    public class TreeAttribute
    {
        public string id { get; set; }
        public bool selected { get; set; }
        public string Key { get; set; }
        public string state { get; set; }
        public string style { get; set; }
        public bool isRoot { get; set; }
    }
}
