using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Noqoush.AdFalcon.Administration.Web.Controllers.Model;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Handler;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using ControllerBase = Noqoush.AdFalcon.Web.Controllers.Core.ControllerBase;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;

namespace Noqoush.AdFalcon.Administration.Web.Controllers
{
    [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
    public class AdOpsController : AuthorizedControllerBase
    {
        private ICampaignService _campaignService;
        private ICreativeUnitService _creativeUnitService;
        private ITileImageService _tileImageService;
        private IAdCreativeStatusService _adCreativeStatusService;
        public AdOpsController(
                                ICampaignService campaignService,
                                ICreativeUnitService creativeUnitService,
                                ITileImageService tileImageService,
                                IAdCreativeStatusService adCreativeStatusService)
        {
            _campaignService = campaignService;
            _creativeUnitService = creativeUnitService;
            _tileImageService = tileImageService;
            _adCreativeStatusService = adCreativeStatusService;
        }
        #region Index
        #region Actions

        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult Index()
        {
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("AdOps","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AccountManagement","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "AdOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            ViewBag.isAdmin = true;
            //load the statues 
            var statues = _adCreativeStatusService.GetAll();
            var statuesDropDown = Utility.GetSelectList();
            statuesDropDown.AddRange(statues.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));
            var model = new AdOpsIndexViewModel { Campaigns = new List<CampaignSummaryDto>(), Statuses = statuesDropDown };
            model.Campaigns = new List<CampaignSummaryDto>();
            return View(model);
        }

        [HttpPost]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult Index(AdsSummaryCriteria criteria)
        {
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                              {
                                                  new BreadCrumbModel()
                                                      {
                                                          Text = ResourcesUtilities.GetResource("AdOps","SiteMapLocalizations"),
                                                          Order = 2,
                                                      },
                                                  new BreadCrumbModel()
                                                      {
                                                          Text =ResourcesUtilities.GetResource("AccountManagement","SiteMapLocalizations"),
                                                          Order = 1,
                                                          Url = Url.Action("Index", "AdOps")
                                                      }
                                              };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            //load the statues 
            var model = GetIndexViewModel(criteria);
            return View(model);
        }

        private AdOpsIndexViewModel GetIndexViewModel(AdsSummaryCriteria criteria)
        {
            ViewBag.isAdmin = true;
            var statues = _adCreativeStatusService.GetAll();
            var statuesDropDown = Utility.GetSelectList();
            statuesDropDown.AddRange(statues.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));

            if ((!criteria.DateFrom.HasValue) && (!criteria.DateTo.HasValue))
            {
                //get current month
                criteria.DateTo = Framework.Utilities.Environment.GetServerTime();

                criteria.DateFrom = criteria.DateTo.Value.AddDays(-30);
            }
            var model = new AdOpsIndexViewModel
                            {
                                Campaigns = _campaignService.GetAdsSummary(criteria),
                                Statuses = statuesDropDown,
                                DateFrom = criteria.DateFrom,
                                DateTo = criteria.DateTo,
                                CampaignName = criteria.CampaignName,
                                CompanyName = criteria.CompanyName,
                                AccountName = criteria.AccountName
                            };
            return model;
        }

        #endregion
        #endregion
    }
}
