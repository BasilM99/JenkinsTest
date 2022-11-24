using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
  

    public class AudienceSegmentController : AuthorizedControllerBase
    {
        private IAudienceSegmentService _audienceSegmentService;

        public AudienceSegmentController()
        {
            _audienceSegmentService = IoC.Instance.Resolve<IAudienceSegmentService>();
        }
        [PermissionsAuthorize(Permission = PortalPermissionsCode.Audience, Roles = "Administrator,adops,appops")]
        [OutputCache(Duration = 40, VaryByQueryKeys = new string[] { "allowRegion","ProviderId","CampaignId" })]
        public JsonResult GetTreeData(bool? allowRegion, int? ProviderId, int? CampaignId, bool withPrice =false)
        {
            List<TreeDto> items = null;

            if (ProviderId.HasValue)
                items = !withPrice ? _audienceSegmentService.GetByDataProvider(new GetByDataProviderRequest { Id = ProviderId.Value, showNotSelectable = true }): _audienceSegmentService.GetByDataProviderWithPrice( new GetByDataProviderRequest { Id =ProviderId.Value,  showNotSelectable=   true });
            else
                items = _audienceSegmentService.GetAll(new ValueMessageWrapper<int?> { Value = CampaignId });
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
                        //item.expanded = true;

                    }

                  
                }

              

            }

            /*foreach (var item in tree)
            {

                item.expanded = true;




            }*/


            var result = new JsonResult (  tree);
            return result;
        }
        [PermissionsAuthorize(Permission = PortalPermissionsCode.Audience, Roles = "Administrator,adops,appops")]
        [OutputCache(Duration = 7200, VaryByQueryKeys = new string[] { "allowRegion", "AccountId" })]
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
                        item.expanded = true;
                    }
                }
            }

            var result = new JsonResult (  tree);
            return result;
        }
        [PermissionsAuthorize(Permission = PortalPermissionsCode.Audience, Roles = "Administrator,adops,appops")]
        //[OutputCache(Duration = 21600, VaryByQueryKeys = "allowRegion;ProviderId;CampaignId")]
        public JsonResult GetTreeDataNoCaching(bool? allowRegion, int? ProviderId, int? CampaignId, bool withPrice = false)
        {
            List<TreeDto> items = null;

            if (ProviderId.HasValue)
                items = !withPrice ? _audienceSegmentService.GetByDataProvider(new GetByDataProviderRequest { Id = ProviderId.Value, showNotSelectable = true }) : _audienceSegmentService.GetByDataProviderWithPrice(new GetByDataProviderRequest { Id = ProviderId.Value, showNotSelectable = true });
            else
                items = _audienceSegmentService.GetAll(new ValueMessageWrapper<int?> { Value = CampaignId });
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

            var result = new JsonResult (  tree);
            return result;
        }





        [OutputCache(Duration = 7200, VaryByQueryKeys = new string[] { "allowRegion", "ProviderId", "CampaignId" })]
        public JsonResult GetTreeDataForContextual(bool? allowRegion, int? ProviderId, int? CampaignId, bool withPrice = false)
        {
            List<TreeDto> items = null;

            if (ProviderId.HasValue)
                items = !withPrice ? _audienceSegmentService.GetByDataProvider(new GetByDataProviderRequest { Id = ProviderId.Value, showNotSelectable = true }) : _audienceSegmentService.GetByDataProviderWithPrice(new GetByDataProviderRequest { Id = ProviderId.Value, showNotSelectable = true });
            else
                items = _audienceSegmentService.GetAllForContextual(new ValueMessageWrapper<int?> { Value = CampaignId });
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
                        //item.expanded = true;

                    }


                }
            }


            var result = new JsonResult(tree);
            return result;
        }

        [OutputCache(Duration = 7200, VaryByQueryKeys = new string[] { "allowRegion", "ProviderId", "CampaignId" })]
        public JsonResult GetTreeDataForContextualBrandSafty(bool? allowRegion, int? ProviderId, int? CampaignId, bool withPrice = false)
        {
            List<TreeDto> items = null;

            if (ProviderId.HasValue)
                items = !withPrice ? _audienceSegmentService.GetByDataProvider(new GetByDataProviderRequest { Id = ProviderId.Value, showNotSelectable = true }) : _audienceSegmentService.GetByDataProviderWithPrice(new GetByDataProviderRequest { Id = ProviderId.Value, showNotSelectable = true });
            else
                items = _audienceSegmentService.GetAllForContextualBrandSafty(new ValueMessageWrapper<int?> { Value = CampaignId });
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
                        //item.expanded = true;

                    }


                }
            }


            var result = new JsonResult(tree);
            return result;
        }

        [OutputCache(Duration = 7200, VaryByQueryKeys = new string[] { "allowRegion", "AccountId" })]
        public JsonResult GetTreeDataAccountForContextual(bool? allowRegion, int? AccountId)
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
                        item.expanded = true;
                    }
                }
            }

            var result = new JsonResult(tree);
            return result;
        }

        //[OutputCache(Duration = 21600, VaryByQueryKeys = "allowRegion;ProviderId;CampaignId")]
        public JsonResult GetTreeDataNoCachingForContextual(bool? allowRegion, int? ProviderId, int? CampaignId, bool withPrice = false)
        {
            List<TreeDto> items = null;

            if (ProviderId.HasValue)
                items = !withPrice ? _audienceSegmentService.GetByDataProvider(new GetByDataProviderRequest { Id = ProviderId.Value, showNotSelectable = true }) : _audienceSegmentService.GetByDataProviderWithPrice(new GetByDataProviderRequest { Id = ProviderId.Value, showNotSelectable = true });
            else
                items = _audienceSegmentService.GetAll(new ValueMessageWrapper<int?> { Value = CampaignId });
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

            var result = new JsonResult(tree);
            return result;
        }



        public ActionResult Tree()
        {
            return View();
        }

    }
}


