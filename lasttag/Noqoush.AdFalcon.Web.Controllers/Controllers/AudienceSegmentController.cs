using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    [PermissionsAuthorize(Permission = PortalPermissionsCode.Audience, Roles = "Administrator,adops,appops")]

    public class AudienceSegmentController : AuthorizedControllerBase
    {
        private IAudienceSegmentService _audienceSegmentService;

        public AudienceSegmentController(IAudienceSegmentService AudienceSegmentService)
        {
            _audienceSegmentService = AudienceSegmentService;
        }
        [OutputCache(Duration = 7200, VaryByParam = "allowRegion;ProviderId;CampaignId")]
        public JsonResult GetTreeData(bool? allowRegion, int? ProviderId, int? CampaignId, bool withPrice =false)
        {
            List<TreeDto> items = null;

            if (ProviderId.HasValue)
                items = !withPrice ? _audienceSegmentService.GetByDataProvider(ProviderId.Value, true): _audienceSegmentService.GetByDataProviderWithPrice(ProviderId.Value, true);
            else
                items = _audienceSegmentService.GetAll(CampaignId);
            var tree = TreeModel.GetTreeNodes(items);

            // This block of code to remove regions in some cases like reports for now
            // This code should be revamped
            if (allowRegion.HasValue && !allowRegion.Value)
            {
                foreach (var countries in tree.Select(p => p.children))
                {
                    foreach (var item in countries)
                    {
                        if (item.children != null)
                        {
                            item.children = null;
                        }
                    }
                }
            }

            var result = new JsonResult { Data = tree, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        [OutputCache(Duration = 7200, VaryByParam = "allowRegion;AccountId")]
        public JsonResult GetTreeDataAccount(bool? allowRegion, int? AccountId)
        {
            List<TreeDto> items = null;

      
                items = _audienceSegmentService.GetAll(null);
            var tree = TreeModel.GetTreeNodes(items);

            // This block of code to remove regions in some cases like reports for now
            // This code should be revamped
            if (allowRegion.HasValue && !allowRegion.Value)
            {
                foreach (var countries in tree.Select(p => p.children))
                {
                    foreach (var item in countries)
                    {
                        if (item.children != null)
                        {
                            item.children = null;
                        }
                    }
                }
            }

            var result = new JsonResult { Data = tree, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        //[OutputCache(Duration = 21600, VaryByParam = "allowRegion;ProviderId;CampaignId")]
        public JsonResult GetTreeDataNoCaching(bool? allowRegion, int? ProviderId, int? CampaignId, bool withPrice = false)
        {
            List<TreeDto> items = null;

            if (ProviderId.HasValue)
                items = !withPrice ? _audienceSegmentService.GetByDataProvider(ProviderId.Value, true) : _audienceSegmentService.GetByDataProviderWithPrice(ProviderId.Value, true);
            else
                items = _audienceSegmentService.GetAll(CampaignId);
            var tree = TreeModel.GetTreeNodes(items);

            // This block of code to remove regions in some cases like reports for now
            // This code should be revamped
            if (allowRegion.HasValue && !allowRegion.Value)
            {
                foreach (var countries in tree.Select(p => p.children))
                {
                    foreach (var item in countries)
                    {
                        if (item.children != null)
                        {
                            item.children = null;
                        }
                    }
                }
            }

            var result = new JsonResult { Data = tree, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }
        public ActionResult Tree()
        {
            return View();
        }

    }
}


