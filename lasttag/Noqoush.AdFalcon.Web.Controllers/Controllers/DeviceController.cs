using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class DeviceController : AuthorizedControllerBase
    {
        private IDeviceService deviceService;
        private ICampaignService campaignService;
        public DeviceController(IDeviceService _deviceService, ICampaignService _campaignService)
        {
            deviceService = _deviceService;
            campaignService = _campaignService;
        }

        [OutputCache(Duration = 21600, VaryByParam = "query")]
        public JsonResult Search(string query)
        {
            var items = deviceService.SearchByQuery(query);
            var result = new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }
        [OutputCache(Duration = 21600, VaryByParam = "deviceTypeId;query")]
        public JsonResult SearchTree(int? deviceTypeId, string query)
        {
            if (!deviceTypeId.HasValue)
            {
                deviceTypeId = (int)DeviceTypeEnum.Any;
            }

            var items = deviceService.SearchByQueryTree(deviceTypeId.Value, query);
            var tree = TreeModel.GetTreeNodes(items);
            var result = new JsonResult { Data = tree, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        [OutputCache(Duration = 21600, VaryByParam = "campaignId;adGroupId;DeviceConstraint")]
        public JsonResult GetTreeData(int campaignId, int? adGroupId, int? DeviceConstraint = null)
        {
            var treeRoots = new List<TreeDto>();

            if (adGroupId.HasValue)
            {
                IList<AdActionTypeConstraintDto> constraints = campaignService.GetTargeting(campaignId, adGroupId.Value).AdActionTypeDto.Constraints;
                if (DeviceConstraint.HasValue)
                {
                    constraints = constraints.Where(x => x.DeviceConstraint == -1 || x.DeviceConstraint == DeviceConstraint.Value).ToList();
                }
                foreach (var constraint in constraints)
                {
                    var items = deviceService.GetDeviceTree(constraint.Platform != null ? constraint.Platform.ID : 0, constraint.DeviceConstraint != -1 ? constraint.DeviceConstraint : 0);
                    treeRoots.AddRange(items);
                }
                // treeRoots.AddRange(deviceService.GetDeviceTree(1, 0));
            }
            else
            {
                var items = deviceService.GetDeviceTree(0, 0);
                treeRoots.AddRange(items);
            }
            //  foreach
            var tree = TreeModel.GetTreeNodes(treeRoots);
            var result = new JsonResult { Data = tree, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }
    }
}


