using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    public class CreativeUnitController : AuthorizedControllerBase
    {

        private ICreativeUnitService _creativeUnitService;
        private ICreativeVendorService _CreativeVendorServic;
        private ICampaignService _CampaignService;
        public CreativeUnitController()
        {
            _creativeUnitService = IoC.Instance.Resolve<ICreativeUnitService>();
            _CreativeVendorServic = IoC.Instance.Resolve<ICreativeVendorService>();
            _CampaignService = IoC.Instance.Resolve<ICampaignService>();
        }

        public JsonResult Index(string query)
        {
            var items = _creativeUnitService.GetAll();
            var result = new JsonResult (  items);
            return result;
        }

     [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "q","culture" })]
        public ActionResult GetCreativeVendors(string q, string culture)
        {

            /*var results2 = _CampaignService.GetImpressionMetricTargetings(new Domain.Repositories.Campaign.Creative.ImpressionMetricCriteria {AdGroupId= 110899 });

               var results=_CampaignService.GetImpressionMetric();
            var gffgg=_CampaignService.GetImpressionMetricTargeting(11089999);*/
            return Json(ReturnCreativeVendorResult(q, culture));
        }
        private List<CreativeVendorDto> ReturnCreativeVendorResult(string q, string culture)
        {


            var criteria = new CreativeVendorCriteria() { Value = q, Culture = culture };
            List<CreativeVendorDto> CreativeVendors = _CreativeVendorServic.GetByQuery(criteria);


            return CreativeVendors;
        }

    }
}


