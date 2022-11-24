using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArabyAds.AdFalcon.Web.Controllers.Core
{
    public class MultiCultureAttribute : ActionFilterAttribute
    {
      

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            

            base.OnActionExecuting(filterContext);
        }
    }
}
