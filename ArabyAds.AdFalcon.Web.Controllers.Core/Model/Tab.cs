using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model
{
    public class Tab
    {
        public Action Action { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExternal { get; set; }
    }
}
