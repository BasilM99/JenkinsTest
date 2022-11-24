using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class CreativeUnitController : AuthorizedControllerBase
    {

        private ICreativeUnitService _creativeUnitService;
        private ICreativeVendorService _CreativeVendorServic;
        private ICampaignService _CampaignService;
        public CreativeUnitController(ICreativeUnitService creativeUnitService, ICreativeVendorService CreativeVendorServic, ICampaignService CampaignService)
        {
            _creativeUnitService = creativeUnitService;
            _CreativeVendorServic = CreativeVendorServic;
            _CampaignService = CampaignService;
        }

        public JsonResult Index(string query)
        {
            var items = _creativeUnitService.GetAll();
            var result = new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

     [OutputCache(Duration = 21600, VaryByParam = "q;culture")]
        public ActionResult GetCreativeVendors(string q, string culture)
        {

            /*var results2 = _CampaignService.GetImpressionMetricTargetings(new Domain.Repositories.Campaign.Creative.ImpressionMetricCriteria {AdGroupId= 110899 });

               var results=_CampaignService.GetImpressionMetric();
            var gffgg=_CampaignService.GetImpressionMetricTargeting(11089999);*/
            return Json(ReturnCreativeVendorResult(q, culture), JsonRequestBehavior.AllowGet);
        }
        private List<CreativeVendorDto> ReturnCreativeVendorResult(string q, string culture)
        {


            var criteria = new CreativeVendorCriteria() { Value = q, Culture = culture };
            List<CreativeVendorDto> CreativeVendors = _CreativeVendorServic.GetByQuery(criteria);


            return CreativeVendors;
        }

    }
}


