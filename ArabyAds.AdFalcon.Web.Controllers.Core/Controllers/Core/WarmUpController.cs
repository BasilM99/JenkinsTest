using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.Framework;
using ArabyAds.Framework.Resources;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers.Core
{
    [RequireHttps(Order = 1)]
    public class WarmUpController : ControllerBase
    {
         private IResourceManager resourceManager;
         public WarmUpController()
        {
           this.resourceManager = IoC.Instance.Resolve<IResourceManager>();
        }
         public ActionResult Resources(string setName, string cultureName)
        {
            resourceManager.GetResourceSet(setName, cultureName);
            return Content(string.Empty);
        }
    }
}
