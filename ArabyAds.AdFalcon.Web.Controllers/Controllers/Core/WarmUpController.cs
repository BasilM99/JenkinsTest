using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Noqoush.Framework.Resources;
using ControllerBase = Noqoush.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers.Core
{
    public class WarmUpController : ControllerBase
    {
         private IResourceManager resourceManager;
         public WarmUpController(IResourceManager resourceManager)
        {
           this.resourceManager = resourceManager;
        }
         public ActionResult Resources(string setName, string cultureName)
        {
            resourceManager.GetResourceSet(setName, cultureName);
            return Content(string.Empty);
        }
    }
}
