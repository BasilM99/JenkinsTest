using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework.Resources;
using ControllerBase = Noqoush.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class DefaultController : ControllerBase
    {
        public ActionResult Index()
        {
            return Redirect(string.Format("~/{0}/", Config.CurrentLanguage));
        }
    }
}
