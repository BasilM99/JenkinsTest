using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Administration.Web.Controllers.Model;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Handler;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Telerik.Web.Mvc;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers
{
    [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
    public class AdOpsController : AuthorizedControllerBase
    {
        private ICampaignService _campaignService;
        private ICreativeUnitService _creativeUnitService;
        private ITileImageService _tileImageService;
        private IAdCreativeStatusService _adCreativeStatusService;
        public AdOpsController(
                               )
        {
            _campaignService = IoC.Instance.Resolve<ICampaignService>(); ;
            _creativeUnitService = IoC.Instance.Resolve<ICreativeUnitService>(); ;
            _tileImageService = IoC.Instance.Resolve<ITileImageService>(); ;
            _adCreativeStatusService = IoC.Instance.Resolve<IAdCreativeStatusService>(); ;
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

        [GridAction(EnableCustomBinding = true)]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public virtual ActionResult IndexCamp(AdsSummaryCriteria criteria)
        {
            var model = GetIndexViewModel(criteria);
            return Json(new GridModel
            {
                Data = model.Campaigns,
                Total = model.Campaigns.Count
            });
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
            var Campaigns = _campaignService.GetAdsSummary(criteria);
            foreach (var camp in Campaigns)
            {
              
                foreach (var adgroup in camp.AdGroupsSummary)
                {
                    
                    foreach (var adsum in adgroup.AdsSummary)
                    {
                        adsum.ClickTags = new List<ClickTagTrackerDto>();
                        adsum.WrapperContent = string.Empty;
                        adsum.ThirdPartyTrackers = new List<ThirdPartyTrackerDto>();
                        adsum.ImpressionTrackingJS = new List<string>();
                        adsum.CreativeUnitsContent = new List<AdCreativeUnitDto>();
                        adsum.VideoEndCardCreativeUnitsContent = new List<AdCreativeUnitDto>();
                        adsum.ImageUrls = new List<CreativeUnitDto>();
                        adsum.VideoEndCardsTrackingURL= new List<string>();
                       // adsum.
                    }
                }
            }
            var model = new AdOpsIndexViewModel
                            {
                               Campaigns= Campaigns,
                                Statuses = statuesDropDown,
                                DateFrom = criteria.DateFrom,
                                DateTo = criteria.DateTo,
                                CampaignName = criteria.CampaignName,
                                CompanyName = criteria.CompanyName,
                                AccountName = criteria.AccountName
                            };
            return model;
        }



        [HttpPost]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult IndexGroups(int CampId)
        {
            AdsSummaryCriteria criteria = new AdsSummaryCriteria();
            criteria.CampaignId = CampId;
            var Campaigns = _campaignService.GetAdsSummary(criteria);
            IList<AdGroupSummaryDto> AdGroupsSummary = new List<AdGroupSummaryDto>();
            foreach (var camp in Campaigns)
            {

                foreach (var adgroup in camp.AdGroupsSummary)
                {

                    foreach (var adsum in adgroup.AdsSummary)
                    {
                        adsum.ClickTags = new List<ClickTagTrackerDto>();
                        adsum.WrapperContent = string.Empty;
                        adsum.ThirdPartyTrackers = new List<ThirdPartyTrackerDto>();
                        adsum.ImpressionTrackingJS = new List<string>();
                        adsum.CreativeUnitsContent = new List<AdCreativeUnitDto>();
                        adsum.VideoEndCardCreativeUnitsContent = new List<AdCreativeUnitDto>();
                        adsum.ImageUrls = new List<CreativeUnitDto>();
                        adsum.VideoEndCardsTrackingURL = new List<string>();
                        // adsum.
                    }
                }
                AdGroupsSummary = camp.AdGroupsSummary;
            }
            return Json(new GridModel
            {
                Data = AdGroupsSummary,
                Total = 0
            });
            //return View(model);
        }

        [HttpPost]
        [AuthorizeRole(Roles = "Administrator,AccountManager,AdOps")]
        public ActionResult IndexAds(int CampId, int GroupId)
        {

            AdsSummaryCriteria criteria = new AdsSummaryCriteria();
            criteria.CampaignId = CampId;
            criteria.GroupId = GroupId;
            var Campaigns = _campaignService.GetAdsSummary(criteria);
            IList<AdCreativeSummaryDto> AdsSummary = new List<AdCreativeSummaryDto>();


            foreach (var camp in Campaigns)
            {

                foreach (var adgroup in camp.AdGroupsSummary)
                {

                    foreach (var adsum in adgroup.AdsSummary)
                    {
                        adsum.ClickTags = new List<ClickTagTrackerDto>();
                        adsum.WrapperContent = string.Empty;
                        adsum.ThirdPartyTrackers = new List<ThirdPartyTrackerDto>();
                        adsum.ImpressionTrackingJS = new List<string>();
                        adsum.CreativeUnitsContent = new List<AdCreativeUnitDto>();
                        adsum.VideoEndCardCreativeUnitsContent = new List<AdCreativeUnitDto>();
                        adsum.ImageUrls = new List<CreativeUnitDto>();
                        adsum.VideoEndCardsTrackingURL = new List<string>();
                        // adsum.
                    }
                    AdsSummary = adgroup.AdsSummary;
                }
            }
            return Json(new GridModel
            {
                Data = AdsSummary,
                Total = 0
            });
        }
        #endregion
        #endregion
    }
}
