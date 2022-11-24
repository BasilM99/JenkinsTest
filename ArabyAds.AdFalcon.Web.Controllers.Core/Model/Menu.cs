using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllerss.Model
{
    

    public class SideMenuItem
    {
        public string id { get; set; }
        public string label { get; set; }
        public string title { get; set; }
        public string href { get; set; }
        public string descNum { get; set; }
        public bool isdivider { get; set; }

        public string icon { get; set; }
        // public bool href { get; set; }
        public string classs { get; set; }
        public List<SideMenuItem> Items { get; set; }
        public bool showBranchesLine { get; set; }
        public bool active { get; set; }
        public bool IsExternal { get; set; }
        public bool IsAbsoluteExternal { get; set; }
        public bool IsUserSettings { get; set; }
        public int moduleId { get; set; }
        public bool Logout { get; set; }

    }
}
