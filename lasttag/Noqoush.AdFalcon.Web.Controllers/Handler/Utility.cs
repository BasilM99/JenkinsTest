using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Web.Controllers.Utilities;

namespace Noqoush.AdFalcon.Web.Controllers.Handler
{
    public static class Utility
    {
        public static List<SelectListItem> GetSelectList(bool addOptional = true)
        {
            var returnListDropDown = new List<SelectListItem>();
            if (addOptional)
            {
                var optionalItem = new SelectListItem
                    {
                        Value = "",
                        Text = ResourcesUtilities.GetResource("Select", null)
                    };
                returnListDropDown.Add(optionalItem);
            }
            return returnListDropDown;
        }
    }
}
