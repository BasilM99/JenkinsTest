using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Core
{
    public class MultiCultureAttribute : ActionFilterAttribute
    {
      

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            

            base.OnActionExecuting(filterContext);
        }
    }
}
