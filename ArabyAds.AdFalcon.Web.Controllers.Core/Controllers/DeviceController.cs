using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages.Requests.Campaign;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    public class DeviceController : AuthorizedControllerBase
    {
        private IDeviceService deviceService;
        private ICampaignService campaignService;
        public DeviceController()
        {
            deviceService = IoC.Instance.Resolve<IDeviceService>();
            campaignService = IoC.Instance.Resolve<ICampaignService>();
        }

        [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "query" })]
        public JsonResult Search(string query)
        {
            var items = deviceService.SearchByQuery(query);
            var result = new JsonResult (  items);
            return result;
        }

        [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "deviceTypeId", "query" })]
        public JsonResult SearchByQueryOrDeviceType(int? deviceTypeId, string query)
        {
            if (!deviceTypeId.HasValue)
            {
                deviceTypeId = (int)DeviceTypeEnum.Any;
            }

            var items = deviceService.SearchByQueryandDeviceType(new SearchByQueryTreeRequest { DeviceTypeId = deviceTypeId.Value, Query = query });
            var result = new JsonResult(items);
            return result;
        }

        
        [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "deviceTypeId","query" })]
        public JsonResult SearchTree(int? deviceTypeId, string query)
        {
            if (!deviceTypeId.HasValue)
            {
                deviceTypeId = (int)DeviceTypeEnum.Any;
            }

            var items = deviceService.SearchByQueryTree(new SearchByQueryTreeRequest { DeviceTypeId= deviceTypeId.Value, Query= query });
            var tree = TreeModel.GetTreeNodes(items);
            var result = new JsonResult (  tree);
            return result;
        }

        [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "campaignId","adGroupId","DeviceConstraint" })]
        public JsonResult GetTreeData(int campaignId, int? adGroupId, int? DeviceConstraint = null)
        {
            var treeRoots = new List<TreeDto>();

            if (adGroupId.HasValue)
            {
                IList<AdActionTypeConstraintDto> constraints = campaignService.GetTargeting(new GetTargetingRequest { CampaignId = campaignId, AdgroupId = adGroupId.Value }).AdActionTypeDto.Constraints;
                if (DeviceConstraint.HasValue)
                {
                    constraints = constraints.Where(x => x.DeviceConstraint == -1 || x.DeviceConstraint == DeviceConstraint.Value).ToList();
                }
                foreach (var constraint in constraints)
                {
                    var items = deviceService.GetDeviceTree(new GetDeviceTreeRequest { PlatformId= constraint.Platform != null ? constraint.Platform.ID : 0,  DeviceConstraint=    constraint.DeviceConstraint != -1 ? constraint.DeviceConstraint : 0 });
                    treeRoots.AddRange(items);
                }
                // treeRoots.AddRange(deviceService.GetDeviceTree(1, 0));
            }
            else
            {
                var items = deviceService.GetDeviceTree( new GetDeviceTreeRequest { DeviceConstraint= 0,  PlatformId=0 });
                treeRoots.AddRange(items);
            }
            //  foreach
            var tree = TreeModel.GetTreeNodes(treeRoots);
            var result = new JsonResult (  tree);
            return result;
        }
    }
}


