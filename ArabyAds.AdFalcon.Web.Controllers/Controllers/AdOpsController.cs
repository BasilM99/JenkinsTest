using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Telerik.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class AdOpsIndexViewModel
    {
        public IList<CampaignSummaryDto> Campaigns { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string AccountName { get; set; }
        public string CampaignName { get; set; }

    }
    public class AdOpsController : AuthorizedControllerBase
    {
        private ICampaignService _campaignService;
        private ICampaignStatusService _campaignStatusService;
        private IAdGroupStatusService _adGroupStatusService;
        private ICreativeUnitService _creativeUnitService;
        private ITileImageService _tileImageService;
        private IAdCreativeStatusService _adCreativeStatusService;
        public AdOpsController(
                                ICampaignService campaignService,
                                ICampaignStatusService campaignStatusService,
                                IAdGroupStatusService adGroupStatusService,
                                ICreativeUnitService creativeUnitService,
                                ITileImageService tileImageService,
                                IAdCreativeStatusService adCreativeStatusService)
        {
            _campaignService = campaignService;
            _campaignStatusService = campaignStatusService;
            _adGroupStatusService = adGroupStatusService;
            _creativeUnitService = creativeUnitService;
            _tileImageService = tileImageService;
            _adCreativeStatusService = adCreativeStatusService;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var s = filterContext.RouteData.Values.Count;
            if (OperationContext.Current.CurrentPrincipal.IsInRole("AdOps"))
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                throw new Exception(ResourcesUtilities.GetResource("NotAuthorized", "Global"));
            }
        }
        #region Index
        #region Helpers
        private List<SelectListItem> GetSelectList()
        {
            var returnListDropDown = new List<SelectListItem>();
            var optionalItem = new SelectListItem { Value = "", Text = ResourcesUtilities.GetResource("Select", null) };
            returnListDropDown.Add(optionalItem);
            return returnListDropDown;
        }
        #endregion
        #region Actions
        public ActionResult Index()
        {
            //load the statues 
            var statues = _adCreativeStatusService.GetAll();
            var statuesDropDown = GetSelectList();
            statuesDropDown.AddRange(statues.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));
            var model = new AdOpsIndexViewModel { Campaigns = new List<CampaignSummaryDto>(), Statuses = statuesDropDown };
            model.Campaigns = new List<CampaignSummaryDto>();//_campaignService.GetAdsSummary(new AdsSummaryCriteria());
            return View(model);
        }

        //[GridAction(EnableCustomBinding = true)]
        //public ActionResult _Index(AdsSummaryCriteria criteria)
        //{
        //    int page = Convert.ToInt32(Request.Form["page"]);
        //    int size = Convert.ToInt32(Request.Form["size"]);
        //    criteria.Page = page;
        //    criteria.Size = size;
        //    var result = _campaignService.GetAdsSummary(criteria);
        //    return View(new GridModel
        //    {
        //        Data = result,
        //        Total = Convert.ToInt32(result.Count)
        //    });
        //}
        [HttpPost]
        public ActionResult Index(AdsSummaryCriteria criteria)
        {
            //load the statues 
            var model = GetIndexViewModel(criteria);
            return View(model);
        }

        private AdOpsIndexViewModel GetIndexViewModel(AdsSummaryCriteria criteria)
        {
            var statues = _adCreativeStatusService.GetAll();
            var statuesDropDown = GetSelectList();
            statuesDropDown.AddRange(statues.Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.Name.ToString() }));

            if ((!criteria.DateFrom.HasValue) && (!criteria.DateTo.HasValue))
            {
                //get current month
                criteria.DateTo = DateTime.Now;

                //var year = criteria.DateTo.Value.Year;
                //var month = criteria.DateTo.Value.Month-1;
                //var day = criteria.DateTo.Value.Day;
                //if (month == 0)
                //{
                //    year=year-1;
                //    month = 12;
                //}
                //if(day==31)
                criteria.DateFrom = criteria.DateTo.Value.AddDays(-30);
            }
            var model = new AdOpsIndexViewModel
                            {
                                Campaigns = _campaignService.GetAdsSummary(criteria),
                                Statuses = statuesDropDown,
                                DateFrom = criteria.DateFrom,
                                DateTo = criteria.DateTo,
                                CampaignName = criteria.CampaignName,
                                AccountName = criteria.AccountName
                            };
            return model;
        }

        #endregion
        #endregion
    }
}
