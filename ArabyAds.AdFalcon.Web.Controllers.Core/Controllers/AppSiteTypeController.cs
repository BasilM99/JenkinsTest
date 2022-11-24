using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.Framework;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using Action = ArabyAds.AdFalcon.Web.Controllers.Model.Action;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    public class AppSiteTypeController : AuthorizedControllerBase
    {
        private IAppSiteTypeService _appSiteTypeService;
        public AppSiteTypeController ()
        {
            _appSiteTypeService = IoC.Instance.Resolve<IAppSiteTypeService>();
        }

        public ActionResult Index()
        {
            return View(_appSiteTypeService.GetAll());
        }
    }
}
