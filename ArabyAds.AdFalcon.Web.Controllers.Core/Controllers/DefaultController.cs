using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Web.Controllers.Core.Core.Security;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework.Resources;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    
    [NoHttps]
    public class DefaultController : ControllerBase
    {
        public ActionResult Index()
        {
            return Redirect(string.Format("~/{0}/", Config.CurrentLanguage));
        }
    }
}
