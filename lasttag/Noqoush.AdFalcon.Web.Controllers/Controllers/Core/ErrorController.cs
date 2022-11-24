using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using ControllerBase = Noqoush.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers.Core
{
    [HandleError]
    public class ErrorController : ControllerBase
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult ItemNotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}
