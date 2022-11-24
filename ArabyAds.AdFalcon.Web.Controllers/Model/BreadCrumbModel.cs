using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Web.Controllers.Model
{
    public class BreadCrumbModel
    {
        public string Url { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public bool IsVisible { get; set; }

        public bool ExtensionDropDown { get; set; }
    }
}
