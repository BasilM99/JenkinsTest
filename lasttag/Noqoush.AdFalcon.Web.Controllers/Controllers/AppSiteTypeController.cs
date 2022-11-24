using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Action = Noqoush.AdFalcon.Web.Controllers.Model.Action;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class AppSiteTypeController : AuthorizedControllerBase
    {
        private IAppSiteTypeService _appSiteTypeService;
        public AppSiteTypeController (IAppSiteTypeService appSiteTypeService)
        {
            _appSiteTypeService = appSiteTypeService;
        }

        public ActionResult Index()
        {
            return View(_appSiteTypeService.GetAll());
        }
    }
}
