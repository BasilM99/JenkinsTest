using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using Telerik.Web.Mvc;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using System.Text.RegularExpressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArabyAds.AdFalcon.Web.Controllers;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework.Caching;
using ArabyAds.AdFalcon.EventDTOs;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Controllers
{
    [AuthorizeRole(Roles = "Administrator,adops,AppOps,AccountManager")]
    //[DenyRole(Roles = "AppOps")]
    public class LookupController : AuthorizedControllerBase
    {
        private ILookupService lookupService;
        private IManufacturerService manufacturerService;
        private IPlatformService platformService;
        private IPartyService PartyService;
        private IAudienceSegmentService AudienceSegmentService;
        public LookupController()
        {
            this.lookupService = IoC.Instance.Resolve<ILookupService>(); ; ;
            this.manufacturerService = IoC.Instance.Resolve<IManufacturerService>(); ; ;
            this.platformService = IoC.Instance.Resolve<IPlatformService>(); ; ;
            this.PartyService = IoC.Instance.Resolve<IPartyService>(); ; ;
            this.AudienceSegmentService = IoC.Instance.Resolve<IAudienceSegmentService>(); ; ;
        }

        static LookupController()
        {
            // handle cache invalidation events 
            Domain.Configuration.EventBrokerSubscription.SubscribeAsync<InvalidateLocalCache>("LocalCacheInvalidation", (evt) => {
                LocalCacheInvalidation.InvalidateLocalCacheEventHandler(evt.TypeName);
            });
        }

        #region Index
        #region Helpers
        private LookupCriteria GetCriteria(string lookType, string name)
        {
            int page = 1;
            int size = Config.PageSize;

            if (!string.IsNullOrWhiteSpace(Request.Form["page"]))
                page = Convert.ToInt32(Request.Form["page"]);

            if (!string.IsNullOrWhiteSpace(Request.Form["size"]))
                size = Convert.ToInt32(Request.Form["size"]);
            if (!string.IsNullOrWhiteSpace(Request.Form["Name"]))
                name = Request.Form["Name"];

            var cratiria = new ArabyAds.AdFalcon.Domain.Common.Repositories.Core.LookupCriteria()
            {
                Size = size,
                LookType = lookType,
                Name = name,
                Page = page
            };
            return cratiria;
        }

        private DeviceLookupCriteria GetDeviceCriteria(string lookType,
                                                        string name,
                                                        int? manufacturerId,
                                                        int? platformId)
        {
            int page = 1;
            int size = Config.PageSize;
            if (!string.IsNullOrWhiteSpace(Request.Form["page"]))
                page = Convert.ToInt32(Request.Form["page"]);

            if (!string.IsNullOrWhiteSpace(Request.Form["size"]))
                size = Convert.ToInt32(Request.Form["size"]);

            if (!string.IsNullOrWhiteSpace(Request.Form["Name"]))
                name = Request.Form["Name"];

            if (!string.IsNullOrWhiteSpace(Request.Form["manufacturerId"]))
                manufacturerId = Convert.ToInt32(Request.Form["manufacturerId"]);

            if (!string.IsNullOrWhiteSpace(Request.Form["platformId"]))
                platformId = Convert.ToInt32(Request.Form["platformId"]);

            var cratiria = new ArabyAds.AdFalcon.Domain.Common.Repositories.Core.DeviceLookupCriteria()
            {
                Size = size,
                LookType = lookType,
                Name = name,
                Page = page,
                ManufacturerId = manufacturerId,
                PlatformId = platformId
            };
            return cratiria;
        }
        #endregion
        [GridAction(EnableCustomBinding = true)]

        public ActionResult DummySelect(int? id)
        {
            var result = id.HasValue ? lookupService.GetVendorkeywords( new ValueMessageWrapper<int> { Value=id.Value }) : new List<CreativeVendorKeywordDto>();
            return Json(new GridModel
            {
                Data = result,
                Total = result.Count
            });
        }

        public ActionResult LookupSelect(string id)
        {

            string Id = Regex.Replace(id, @"\s+", "").ToLower();

            if (Id == "demographic")
            {
                Id = "agegroup";
            }

            if (string.IsNullOrEmpty(Id))
                throw new NullReferenceException();

            // var result = lookupService.GetAllPageLookup(GetCriteria(id,string.Empty));
            ListViewModel model = null;

            switch (Id)
            {
                case LookupNames.Device:
                    {
                        model = new DeviceListViewModel
                        {
                            Items = new List<LookupDto>(),
                            // LookupTypes = lookupsList,
                            EntityType = Id,
                            FilterView = LookupEntries.FindLookupFilterView(Id),
                            SearchAction = LookupEntries.Lookups[Id].SearchAction,
                            Manufacturers = GetList(LookupNames.Manufacturer, 0),
                            Platforms = GetList(LookupNames.Platform, 0)
                        };
                        break;
                    }
                default:
                    {
                        model = new ListViewModel
                        {
                            Items = new List<LookupDto>(),
                            //result.Items,
                            //LookupTypes = lookupsList,
                            EntityType = Id,
                            FilterView = LookupEntries.FindLookupFilterView(Id),
                            SearchAction = LookupEntries.Lookups[Id].SearchAction
                        };
                        break;
                    }
            }
            var result = lookupService.GetAllPageLookup(GetCriteria(Id, ""));
            model.Items = result.Items.ToList();

            ViewData["total"] = result.TotalCount;
            return PartialView("LookupSelect", model);

        }


        public ActionResult GetLookupData(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("Index", new { id = "currency" });
            }
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "", Text = ResourcesUtilities.GetResource("Select","Global")
                                              }};
            foreach (var lookupEntry in LookupEntries.Lookups)
            {
                var selected = lookupEntry.Key.Equals(id, StringComparison.OrdinalIgnoreCase);
                lookupsList.Add(new SelectListItem()
                {
                    Value = lookupEntry.Key,
                    Text = lookupEntry.Value.DispalyName,
                    Selected = selected
                });
            }

            // var result = lookupService.GetAllPageLookup(GetCriteria(id,string.Empty));
            ListViewModel model = null;

            switch (id.ToLower())
            {
                case LookupNames.Device:
                    {
                        model = new DeviceListViewModel
                        {
                            Items = new List<LookupDto>(),
                            LookupTypes = lookupsList,
                            EntityType = id,
                            FilterView = LookupEntries.FindLookupFilterView(id),
                            SearchAction = LookupEntries.Lookups[id].SearchAction,
                            Manufacturers = GetList(LookupNames.Manufacturer, 0),
                            Platforms = GetList(LookupNames.Platform, 0)
                        };
                        break;
                    }
                default:
                    {
                        model = new ListViewModel
                        {
                            Items = new List<LookupDto>(),
                            //result.Items,
                            LookupTypes = lookupsList,
                            EntityType = id,
                            FilterView = LookupEntries.FindLookupFilterView(id),
                            SearchAction = LookupEntries.Lookups[id].SearchAction
                        };
                        break;
                    }
            }



            ViewData["total"] = 0;//result.TotalCount;
            return Json(model);
        }
        public ActionResult Index(string id)
        {
            //return View();
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("Index", new { id = "currency" });
            }
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "", Text = ResourcesUtilities.GetResource("Select","Global")
                                              }};
            foreach (var lookupEntry in LookupEntries.Lookups)
            {
                var selected = lookupEntry.Key.Equals(id, StringComparison.OrdinalIgnoreCase);
                lookupsList.Add(new SelectListItem()
                {
                    Value = lookupEntry.Key,
                    Text = lookupEntry.Value.DispalyName,
                    Selected = selected
                });
            }

            // var result = lookupService.GetAllPageLookup(GetCriteria(id,string.Empty));
            ListViewModel model = null;

            switch (id.ToLower())
            {
                case LookupNames.Device:
                    {
                        model = new DeviceListViewModel
                        {
                            Items = new List<LookupDto>(),
                            LookupTypes = lookupsList,
                            EntityType = id,
                            FilterView = LookupEntries.FindLookupFilterView(id),
                            SearchAction = LookupEntries.Lookups[id].SearchAction,
                            Manufacturers = GetList(LookupNames.Manufacturer, 0),
                            Platforms = GetList(LookupNames.Platform, 0)
                        };
                        break;
                    }
                default:
                    {
                        model = new ListViewModel
                        {
                            Items = new List<LookupDto>(),
                            //result.Items,
                            LookupTypes = lookupsList,
                            EntityType = id,
                            FilterView = LookupEntries.FindLookupFilterView(id),
                            SearchAction = LookupEntries.Lookups[id].SearchAction
                        };
                        break;
                    }
            }



            ViewData["total"] = 0;//result.TotalCount;
            return View(model);
        }

      


        [GridAction(EnableCustomBinding = true)]
        public ActionResult _Index(string id, string Name = "")
        {
            var result = lookupService.GetAllPageLookup(GetCriteria(id, Name));
            return Json(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult CostElemetntPage(string Name = "")
        {


            //var result = lookupService.GetAllLookupByType(new LookupCriteriaBase { LookType = "costelement" });
            var result = lookupService.GetCostPageLookup(GetCriteriaCost("costelement", Name));
            return Json(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });

        }


        private LookupCriteria GetCriteriaCost(string lookType, string name)
        {
            int page = 1;
            int size = Config.PageSize;

            if (!string.IsNullOrWhiteSpace(Request.Form["page"]))
                page = Convert.ToInt32(Request.Form["page"]);

            if (!string.IsNullOrWhiteSpace(Request.Form["size"]))
                size = Convert.ToInt32(Request.Form["size"]);
            if (!string.IsNullOrWhiteSpace(Request.Form["Name"]))
                name = Request.Form["Name"];
            if (string.IsNullOrWhiteSpace(name))
            {
                name = string.Empty;
            }
            var cratiria = new ArabyAds.AdFalcon.Domain.Common.Repositories.Core.LookupCriteria()
            {
                Size = size,
                LookType = lookType,
                Name = name,
                Page = page
            };
            return cratiria;
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _IndexDevice(string id, string name, int? manufacturerId, int? platformId)
        {
            if (manufacturerId == 0)
                manufacturerId = null;
            if (platformId == 0)
                platformId = null;
            var result = lookupService.GetAllPageLookup(GetDeviceCriteria(id, name, manufacturerId, platformId));
            return Json(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        #endregion
        #region Helpers
        private List<SelectListItem> GetList(string lookupType, int selectedValue)
        {
            var items = lookupService.GetAllLookup(new LookupCriteriaBase { LookType = lookupType });
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "0",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected = selectedValue==0
                                              }};


            lookupsList.AddRange(
                items.Items.Select(
                    item => new SelectListItem()
                    {
                        Value = item.ID.ToString(),
                        Text = item.Name.ToString(),
                        Selected = item.ID == selectedValue
                    }).OrderBy(p => p.Text));
            return lookupsList;
        }
        private List<SelectListItem> GetLocations(int typeId, int selectedValue)
        {
            var result = lookupService.GetParentLocations( new ValueMessageWrapper<int> {  Value=typeId });
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "0",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected = selectedValue==0
                                              }};


            lookupsList.AddRange(
                result.Items.Select(
                    item => new SelectListItem()
                    {
                        Value = item.ID.ToString(),
                        Text = item.Name.ToString(),
                        Selected = item.ID == selectedValue
                    }).OrderBy(p => p.Text));
            return lookupsList;
        }
        //public ActionResult ValidateLangCode(int? id, string code)
        //{
        //    bool Result = false;
        //    try
        //    {
        //        Result = lookupService.ValidateLangCode(id, code);

        //        return Json(new { Result = Result });

        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = Result });
        //    }
        //}

        #endregion
        public ActionResult GetListParent(int typeId)
        {
            var list = GetLocations(typeId, 0);

            return Json(new { list = list });

        }

        public ActionResult GetLoockupItemData(int id, string type)
        {
            var criteria = new ArabyAds.AdFalcon.Domain.Common.Repositories.Core.LookupGetCriteria()
            {
                Id = id,
                LookType = type,
            };
            var item = lookupService.GetLookup(criteria);
            // get view name
            var lookupView = LookupEntries.FindLookupView(type);
            // add default cultures
            if (item.Name == null)
            {
                item.Name = LocalizedStringDto.GetDefault();
                item.Name.GroupKey = LookupEntries.FindLookupGroupKey(type);
            }
            LookupViewModel saveModel = null;

            switch (type.ToLower())
            {
                case LookupNames.Device:
                    {
                        var dItem = (DeviceDto)item;
                        saveModel = new DeviceViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveDevice",
                            ViewName = lookupView,
                            Manufacturers = GetList(LookupNames.Manufacturer, dItem.Manufacturer == null ? 0 : dItem.Manufacturer.ID),
                            Platforms = GetList(LookupNames.Platform, dItem.Platform == null ? 0 : dItem.Platform.ID)
                        };
                        break;
                    }
                case LookupNames.CreativeVendor:
                    {
                        var dItem = (CreativeVendorDto)item;
                        saveModel = new CreativeVendorViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveCreativeVendor",
                            ViewName = lookupView,
                            VendorKeyWord = dItem.Keywords
                        };
                        break;
                    }
                case LookupNames.CostElement:
                    {
                        var dItem = (CostElementDto)item;
                        CostElementViewModel tsaveModel = new CostElementViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveCostElement",
                            ViewName = lookupView,
                            IsDataCostElem = Config.IsDataCostElem,
                            IsThirdPartyCostElem = Config.IsThirdPartyCostElem,
                            IsPlatformCostElem = Config.IsPlatformCostElem,
                            IsAVRCostElem = Config.IsAVRCostElem,
                            IsExchangeDiscrepancyCostElem = Config.IsExchangeDiscrepancyCostElem,
                            IsAdfalconRevenueCostElem = Config.IsAdfalconRevenueCostElem,
                        };
                       // tsaveModel.LookupType = type;
                       // tsaveModel.LookupDto = ((CostElementDto)((CostElementViewModel)saveModel).LookupDto);
                        return Json(new { 
                            saveModel = tsaveModel,
                            LookupDto = dItem,
                            costModels = (IoC.Instance.Resolve<ICostModelWrapperService>()).GetAll()
                        });
                        break;
                    }

                case LookupNames.Fee:
                    {
                        var dItem = (FeeDto)item;
                        FeeViewModel tsaveModel = new FeeViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveFee",
                            ViewName = lookupView,
                            IsDataFee = Config.IsDataFee,
                            IsThirdPartyFee = Config.IsThirdPartyFee,
                            IsPlatformFee = Config.IsPlatformFee,
                            IsAVRFee = Config.IsAVRFee,
                            IsExchangeDiscrepancyFee = Config.IsExchangeDiscrepancyFee,
                            IsAdfalconRevenueFee = Config.IsAdfalconRevenueFee,
                        };
                        return Json(new
                        {
                            saveModel = tsaveModel,
                            LookupDto = dItem,
                            costModels = (IoC.Instance.Resolve<ICostModelWrapperService>()).GetAll()
                        });
                        break;
                    }
                case LookupNames.Manufacturer:
                    {
                        var dItem = (ManufacturerDto)item;
                        saveModel = new ManufacturerViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveManufacturer",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.Platform:
                    {
                        var dItem = (PlatformDto)item;
                        saveModel = new PlatformViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SavePlatform",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.Advertiser:
                    {
                        var dItem = (AdvertiserDto)item;

                        saveModel = new AdvertiserViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveAdvertiser",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.DeviceCapability:
                    {
                        var dItem = (DeviceCapabilityDto)item;
                        saveModel = new DeviceCapabilityViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveDeviceCapability",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.audiencesegment:
                    {
                        var dItem = (AudienceSegmentDto)item;
                        saveModel = new AudienceSegmentViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveAudienceSegment",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.Attributes:
                    {
                        var dItem = (AdCreativeAttributeDto)item;
                        saveModel = new AdCreativeAttributeViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveAttribute",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.Keyword:
                    {
                        var dItem = (KeywordSaveDto)item;
                        saveModel = new KeywordViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveKeyword",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.LocationBase:
                    {
                        var dItem = (LocationDto)item;
                        saveModel = new LocationViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveLocation",
                            ViewName = lookupView,
                            Locations = new List<SelectListItem>(),
                        };
                    }
                    break;
                //case LookupNames.Geographic:
                //    {
                //        var dItem = (item);
                //        saveModel = new LookupViewModel()
                //        {
                //            LookupDto = dItem,
                //            ActionName = "Save",
                //            ViewName = lookupView,
                //            //Locations = new List<SelectListItem>(),
                //        };
                //    }
                //    break;
                //case LookupNames.AgeGroup:
                //    {
                //        var dItem = (AgeGroupDto)item;
                //        saveModel = new AgeGroupViewModel()
                //        {
                //            LookupDto = dItem,
                //            ActionName = "Save",
                //            ViewName = lookupView,
                //            AgeGroups = new List<SelectListItem>(),
                //        };
                //    }
                //    break;
                case LookupNames.language:
                    {
                        var dItem = (LanguageSaveDto)item;
                        saveModel = new LanguageViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveLanguage",
                            ViewName = lookupView,
                        };
                    }
                    break;

                default:
                    {
                        saveModel = new LookupViewModel()
                        {
                            LookupDto = item,
                            ActionName = "Save",
                            ViewName = lookupView
                        };
                        break;
                    }
            }
            saveModel.LookupType = type;
            return Json(saveModel);
        }
        public ActionResult Item(int id, string type)
        {
            var criteria = new ArabyAds.AdFalcon.Domain.Common.Repositories.Core.LookupGetCriteria()
            {
                Id = id,
                LookType = type,
            };
            var item = lookupService.GetLookup(criteria);
            // get view name
            var lookupView = LookupEntries.FindLookupView(type);
            // add default cultures
            if (item.Name == null)
            {
                item.Name = LocalizedStringDto.GetDefault();
                item.Name.GroupKey = LookupEntries.FindLookupGroupKey(type);
            }
            LookupViewModel saveModel = null;

            switch (type.ToLower())
            {
                case LookupNames.Device:
                    {
                        var dItem = (DeviceDto)item;
                        saveModel = new DeviceViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveDevice",
                            ViewName = lookupView,
                            Manufacturers = GetList(LookupNames.Manufacturer, dItem.Manufacturer == null ? 0 : dItem.Manufacturer.ID),
                            Platforms = GetList(LookupNames.Platform, dItem.Platform == null ? 0 : dItem.Platform.ID)
                        };
                        break;
                    }
                case LookupNames.CreativeVendor:
                    {
                        var dItem = (CreativeVendorDto)item;
                        saveModel = new CreativeVendorViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveCreativeVendor",
                            ViewName = lookupView,
                            VendorKeyWord = dItem.Keywords
                        };
                        break;
                    }
                case LookupNames.CostElement:
                    {
                        var dItem = (CostElementDto)item;
                        saveModel = new CostElementViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveCostElement",
                            ViewName = lookupView,
                        };
                        break;
                    }

                case LookupNames.Fee:
                    {
                        var dItem = (FeeDto)item;
                        saveModel = new FeeViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveFee",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.Manufacturer:
                    {
                        var dItem = (ManufacturerDto)item;
                        saveModel = new ManufacturerViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveManufacturer",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.Platform:
                    {
                        var dItem = (PlatformDto)item;
                        saveModel = new PlatformViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SavePlatform",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.Advertiser:
                    {
                        var dItem = (AdvertiserDto)item;

                        saveModel = new AdvertiserViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveAdvertiser",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.DeviceCapability:
                    {
                        var dItem = (DeviceCapabilityDto)item;
                        saveModel = new DeviceCapabilityViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveDeviceCapability",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.audiencesegment:
                    {
                        var dItem = (AudienceSegmentDto)item;
                        saveModel = new AudienceSegmentViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveAudienceSegment",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.Attributes:
                    {
                        var dItem = (AdCreativeAttributeDto)item;
                        saveModel = new AdCreativeAttributeViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveAttribute",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.Keyword:
                    {
                        var dItem = (KeywordSaveDto)item;
                        saveModel = new KeywordViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveKeyword",
                            ViewName = lookupView,
                        };
                        break;
                    }
                case LookupNames.LocationBase:
                    {
                        var dItem = (LocationDto)item;
                        saveModel = new LocationViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveLocation",
                            ViewName = lookupView,
                            Locations = new List<SelectListItem>(),
                        };
                    }
                    break;
                //case LookupNames.Geographic:
                //    {
                //        var dItem = (item);
                //        saveModel = new LookupViewModel()
                //        {
                //            LookupDto = dItem,
                //            ActionName = "Save",
                //            ViewName = lookupView,
                //            //Locations = new List<SelectListItem>(),
                //        };
                //    }
                //    break;
                //case LookupNames.AgeGroup:
                //    {
                //        var dItem = (AgeGroupDto)item;
                //        saveModel = new AgeGroupViewModel()
                //        {
                //            LookupDto = dItem,
                //            ActionName = "Save",
                //            ViewName = lookupView,
                //            AgeGroups = new List<SelectListItem>(),
                //        };
                //    }
                //    break;
                case LookupNames.language:
                    {
                        var dItem = (LanguageSaveDto)item;
                        saveModel = new LanguageViewModel()
                        {
                            LookupDto = dItem,
                            ActionName = "SaveLanguage",
                            ViewName = lookupView,
                        };
                    }
                    break;
          
                default:
                    {
                        saveModel = new LookupViewModel()
                        {
                            LookupDto = item,
                            ActionName = "Save",
                            ViewName = lookupView
                        };
                        break;
                    }
            }
            saveModel.LookupType = type;
            return PartialView("Default", saveModel);
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Save(string id, LookupViewModel saveModel)
        {
            lookupService.SaveLookup( new SaveLookupRequest {  Data=saveModel.LookupDto,  LookType=id });
            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Lookup", Messages = rMessages, Status = ResponseStatus.success });
        }
        public ActionResult SaveLoockupItem(string id, [FromBody]LookupViewModel saveModel)
        {
            lookupService.SaveLookup(new SaveLookupRequest { Data = saveModel.LookupDto, LookType = id });
            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Lookup", Messages = rMessages, Status = ResponseStatus.success });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveDevice(string id, DeviceSaveModel saveModel)
        {
            lookupService.SaveLookup(new SaveLookupRequest {  Data=saveModel.LookupDto,  LookType=id });

            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Device", Messages = rMessages, Status = ResponseStatus.success });
        }

        [DenyRole(Roles = "AccountManager")]
        public ActionResult SaveDeviceLoockupItem(string id, [FromBody]DeviceSaveModel saveModel)
        {
       
            lookupService.SaveLookup(new SaveLookupRequest { Data = saveModel.LookupDto, LookType = id });
            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Device", Messages = rMessages   , Status = ResponseStatus.success });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveCostElement(string id, [FromBody]CostElementSaveModel saveModel)
        {
            var costElementDto = saveModel.LookupDto;
            var costElementValuesList = new List<CostElementValueDto>();

            //var flag = saveModel.LookupDto.IsBitwiseOr;
            //foreach (var item in Request.Form.Keys.Where(p => p.StartsWith("LookupDto.CostModelValue-")))
            //{
            //    CostElementValueDto valueDto = new CostElementValueDto();
            //    valueDto.CostModelWrapper = new CostModelWrapperDto();
            //    valueDto.CostModelWrapper.ID = int.Parse(item.Split('-')[1]);

            //    if (saveModel.LookupDto.TypeId == (int)Domain.Common.Model.Core.CostElement.CalculationType.Percentage)
            //    {
            //        valueDto.Value = decimal.Parse(Request.Form[item]) / 100M;
            //    }
            //    else
            //    {
            //        valueDto.Value = decimal.Parse(Request.Form[item]);
            //    }
             

            //    costElementValuesList.Add(valueDto);
            //}
            //costElementDto.Values = costElementValuesList;
            costElementDto.CostItemType = CostItemType.CostElement;
            lookupService.SaveLookup( new SaveLookupRequest { Data= saveModel.LookupDto,   LookType= id });
            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Cost Element", Messages = rMessages, Status = ResponseStatus.success });
        }


        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveFee(string id, [FromBody]FeeSaveModel saveModel)
        {
            var costElementDto = saveModel.LookupDto;
            //var costElementValuesList = new List<CostElementValueDto>();

            //foreach (var item in Request.Form.Keys.Where(p => p.StartsWith("LookupDto.CostModelValue-")))
            //{
            //    CostElementValueDto valueDto = new CostElementValueDto();
            //    valueDto.CostModelWrapper = new CostModelWrapperDto();
            //    valueDto.CostModelWrapper.ID = int.Parse(item.Split('-')[1]);

            //    if (saveModel.LookupDto.TypeId == (int)Domain.Common.Model.Core.CostElement.CalculationType.Percentage && saveModel.LookupDto.FeeCalculatedFrom!=FeeCalculatedFrom.System)
            //    {
            //        valueDto.Value = decimal.Parse(Request.Form[item]) / 100M;
            //    }
            //    else if(saveModel.LookupDto.FeeCalculatedFrom != FeeCalculatedFrom.System)
            //    {
            //        valueDto.Value = decimal.Parse(Request.Form[item]);
            //    }

            //    costElementValuesList.Add(valueDto);
            //}
            //costElementDto.Values = costElementValuesList;
            costElementDto.CostItemType = CostItemType.Fee;
            lookupService.SaveLookup( new SaveLookupRequest { Data= saveModel.LookupDto,  LookType=id });
            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Fee", Messages = rMessages, Status = ResponseStatus.success });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveManufacturer(string id, [FromBody] ManufacturerSaveModel saveModel)
        {
            lookupService.SaveLookup(new SaveLookupRequest { Data= saveModel.LookupDto,  LookType=id });
            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Manufacturer", Messages = rMessages, Status = ResponseStatus.success });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SavePlatform(string id, [FromBody]PlatformSaveModel saveModel)
        {
            lookupService.SaveLookup(new SaveLookupRequest {  Data=saveModel.LookupDto, LookType= id });
            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Platform ", Messages = rMessages, Status = ResponseStatus.success });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveAdvertiser(string id, [FromBody]AdvertiserSaveModel saveModel)
        {
            string Message = "";
            bool Result = false;
            IList<ResultMessage> rMessages = new List<ResultMessage>();
            try
            {
                Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), saveModel.LookupDto.Name.Value);

                saveModel.LookupDto.Name.Values[0].Value = saveModel.LookupDto.Name.Values[0].Value.Trim();
                saveModel.LookupDto.Name.Values[1].Value = saveModel.LookupDto.Name.Values[1].Value.Trim();

                lookupService.SaveLookup( new SaveLookupRequest {  Data=saveModel.LookupDto,   LookType=  id });
                Result = true;
                Framework.Caching.CacheManager.Current.DefaultProvider.Remove("CacheByAttribute_AdvertiserService");

          
                rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
                return new JsonResult(new { MessageTitle = "Save Advertiser ", Messages = rMessages, Status = ResponseStatus.success });
            }
            catch (Exception e)
            {
                if (e is BusinessException)
                {
                    var ex = e as BusinessException;
                    Message = ex.Errors.FirstOrDefault().Message;
                }
                else
                {
                    Message = e.Message;
                }

                rMessages.Add(new ResultMessage { Type = MessagesType.Error, Message = Message });
                return new JsonResult(new { MessageTitle = "Save Advertiser ", Messages = rMessages, Status = ResponseStatus.businessException });
            }

            //return Json(new { Message = Message, Result = Result });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveDeviceCapability(string id, [FromBody]DeviceCapabilitySaveModel saveModel)
        {
            lookupService.SaveLookup(new SaveLookupRequest {  Data=saveModel.LookupDto,  LookType= id });
            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Device Cap ", Messages = rMessages, Status = ResponseStatus.success });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveAttribute(string id, [FromBody]AdCreativeAttributeSaveModel saveModel)
        {
            lookupService.SaveLookup(new SaveLookupRequest { Data= saveModel.LookupDto, LookType= id });
            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "SaveAttribute ", Messages = rMessages, Status = ResponseStatus.success });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveAudienceSegment(string id, AdCreativeAttributeSaveModel saveModel)
        {
            lookupService.SaveLookup(new SaveLookupRequest {  Data=saveModel.LookupDto, LookType= id });
            return Content("Done");
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveKeyword(string id, [FromBody]KeywordSaveModel saveModel)
        {
            string Message = "";
            bool Result = false;
            try
            {
                Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), saveModel.LookupDto.Name.Value);

                lookupService.SaveLookup(new SaveLookupRequest { Data = saveModel.LookupDto, LookType= id });
                Result = true;

            }
            catch (Exception ex)
            {
                if (ex is BusinessException)
                {
                    var ex1 = ex as BusinessException;
                    Message = ex1.Errors.FirstOrDefault().Message;
                }
                else
                {
                    Message = ex.Message;
                }
            }
            return Json(new { Message = Message, Result = Result });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveLanguage(string id, [FromBody]LanguageSaveModel saveModel)
        {
            string Message = "";
            bool Result = false;
            try
            {
                Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), saveModel.LookupDto.Name.Value);

                lookupService.SaveLookup(new SaveLookupRequest { Data = saveModel.LookupDto,  LookType=id });
                Result = true;
                Framework.Caching.CacheManager.Current.DefaultProvider.Remove("CacheByAttribute_LanguageService");
            }
            catch (Exception ex)
            {
                if (ex is BusinessException)
                {
                    var ex1 = ex as BusinessException;
                    Message = ex1.Errors.FirstOrDefault().Message;
                }
                else
                {
                    Message = ex.Message;
                }
            }
            return Json(new { Message = Message, Result = Result });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveLocation(string id, [FromBody]LocationSaveModel saveModel)
        {
            var locationDto = saveModel.LookupDto as LocationDto;

            if (locationDto.ParentId.HasValue && locationDto.ParentId.Value == 0)
            {
                locationDto.ParentId = null;
            }

            // (saveModel.LookupDto as LocationDto).Type = LocationType.Continent;
            lookupService.SaveLookup( new SaveLookupRequest { Data = saveModel.LookupDto, LookType = id });

            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Location", Messages = rMessages, Status = ResponseStatus.success });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveCreativeVendor(string id, [FromBody]CreativeVendorSaveModel saveModel)
        {
            var keywords = saveModel.InsertedKeyWords;
            saveModel.LookupDto.InsertedKeywords = keywords.Select(x => new CreativeVendorKeywordDto { Keyword = x }).Where(x => !string.IsNullOrEmpty(x.Keyword)).ToList();
            keywords = saveModel.DeletedKeyWords;
            saveModel.LookupDto.DeletedKeywords = keywords.Select(x => new CreativeVendorKeywordDto { Keyword = x }).Where(x => !string.IsNullOrEmpty(x.Keyword)).ToList();


            lookupService.SaveLookup( new SaveLookupRequest { Data= saveModel.LookupDto, LookType= id });
            Framework.Caching.CacheManager.Current.DefaultProvider.Remove("CacheByAttribute_CreativeVendorService");

            IList<ResultMessage> rMessages = new List<ResultMessage>();
            rMessages.Add(new ResultMessage { Type = MessagesType.Success, Message = "Done" });
            return new JsonResult(new { MessageTitle = "Save Device Cap ", Messages = rMessages, Status = ResponseStatus.success });
        }


        public ActionResult Providers(int? providerId, bool success = false)
        {

            ListViewModel model = new ListViewModel();

            var obj = PartyService.QueryByCriteria(new PartyCriteria
            {
                Visible= true,
                Type = PartyType.DP
            });

            if (obj != null && obj.Items != null && obj.Items.FirstOrDefault() != null)
            {
                model.Type = providerId.HasValue ? (int)providerId : (int)obj.Items.FirstOrDefault().ID;

                model.LookupTypes = obj.Items.Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString(), Selected = x.ID == model.Type }).ToList();
            }
            if (success)
            {
                ViewBag.Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), ResourcesUtilities.GetResource("Node", "Audience"));

            }

            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                             new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Audiences", "Audience"),
                                                  Order = 1
                                              }
                                      };
            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion


            return View(model);
        }
        public ActionResult ProviderTree(int providerId)
        {
            var tree = new TreeViewModel()
            {
                Url = Url.Action("GetTreeData", "AudienceSegment", new { ProviderId = providerId }),
                Name = "ProviderTreeList",
                Id = "ProviderTreeList",
                IsAjax = true,
            };
            ViewBag.ondblclick = "Edit(this)";
            return PartialView("tree", tree);
        }
        [OutputCache(Duration = 3600, VaryByQueryKeys = new string[] { "id","q","culture" })]
        public ActionResult GetAudienceSegmentsResult(int id , string q, string culture)
        {

            return Json(ReturnAudienceSegmentsResult(id,q, culture));
        }




        private IEnumerable<AudienceSegmentDto> ReturnAudienceSegmentsResult(int id, string q, string culture)
        {

            var audianceSegments = this.AudienceSegmentService.getAudianceSegmentsByDataProvider(new GetAudienceSegByDataProviderRequest { Id= id, q=q, cultures= culture });

   
            return audianceSegments;
        }
        public ActionResult SegmentSaveForm()
        {
            AudienceSegmentViewModel model = new AudienceSegmentViewModel
            {
                AudienceSegment = new AudienceSegmentDto(),
            };
            model.AudienceSegment.Name = new LocalizedStringDto
            {
                Values = new List<LocalizedValueDto>()
            };


            model.AudienceSegment.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = "" });
            model.AudienceSegment.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = "" });
            model.AudienceSegment.IsSelectedable = true;

            return PartialView("AudienceSegmentSave", model);
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveSegmentGet(int? id, string en, string ar, string OperatorSegmentCode, decimal Price, string Description, bool IsSelectedable, int ProviderId, string roots, int CodeUQ)
        {
            try
            {
                if (!id.HasValue)
                {
                    if (!string.IsNullOrEmpty(roots))
                    {
                        var list = roots.Split(',').ToList();
                        List<int> rootsIds = list.Where(x => !string.IsNullOrEmpty(x)).Select(z => Convert.ToInt32(z)).ToList();

                        foreach (int root in rootsIds)
                        {
                            AudienceSegmentDto obj = new AudienceSegmentDto
                            {
                                OperatorSegmentCode = OperatorSegmentCode,
                                Price = Price,
                                CodeUQ = CodeUQ,
                                Description = Description,
                                IsSelectedable = IsSelectedable,
                                ProviderId = ProviderId,
                                ParentId = root,
                                Name = new LocalizedStringDto
                                {
                                    GroupKey = "AudienceSegment",
                                    Value = en,
                                    Values = new List<LocalizedValueDto>()

                                }
                            };
                            obj.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = en });
                            obj.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = ar });

                            AudienceSegmentService.Save(obj);
                        }
                    }
                    else
                    {
                        AudienceSegmentDto obj = new AudienceSegmentDto
                        {
                            OperatorSegmentCode = OperatorSegmentCode,
                            CodeUQ = CodeUQ,
                            Price = Price,
                            Description = Description,
                            IsSelectedable = IsSelectedable,
                            ProviderId = ProviderId,
                            Name = new LocalizedStringDto
                            {
                                GroupKey = "AudienceSegment",
                                Value = en,
                                Values = new List<LocalizedValueDto>()

                            }
                        };
                        obj.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = en });
                        obj.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = ar });

                        AudienceSegmentService.Save(obj);
                    }

                }
                else
                {
                    AudienceSegmentDto obj = new AudienceSegmentDto
                    {
                        OperatorSegmentCode = OperatorSegmentCode,
                        ID = (int)id,
                        Price = Price,
                        CodeUQ = CodeUQ,
                        Description = Description,
                        IsSelectedable = IsSelectedable,
                        ProviderId = ProviderId,
                        Name = new LocalizedStringDto
                        {
                            GroupKey = "AudienceSegment",
                            Value = en,
                            Values = new List<LocalizedValueDto>()

                        }
                    };
                    obj.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = en });
                    obj.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = ar });

                    AudienceSegmentService.Save(obj);


                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e.Message });

            }

            return Json(new { result = true });
        }
        [DenyRole(Roles = "AccountManager")]

        [HttpPost]
        public ActionResult SaveSegment(AudienceSegmentDto obj)
        {
            try
            {

                string roots = obj.roots;
                string en = obj.en;
                string ar = obj.ar;
                if (obj.ID == 0)
                {
                    if (!string.IsNullOrEmpty(roots))
                    {
                        var list = roots.Split(',').ToList();
                        List<int> rootsIds = list.Where(x => !string.IsNullOrEmpty(x)).Select(z => Convert.ToInt32(z)).ToList();

                        foreach (int root in rootsIds)
                        {
                            AudienceSegmentDto obj1 = new AudienceSegmentDto
                            {
                                OperatorSegmentCode = obj.OperatorSegmentCode,
                                Price = obj.Price,
                                CodeUQ = obj.CodeUQ,
                                Description = obj.Description,
                                IsSelectedable = obj.IsSelectedable,
                                ProviderId = obj.ProviderId,
                                IsDeleted = obj.IsDeleted,
                                ParentId = root,
                                IsPermissionNeed = obj.IsPermissionNeed,
                                Name = new LocalizedStringDto
                                {
                                    GroupKey = "AudienceSegment",
                                    Value = en,
                                    Values = new List<LocalizedValueDto>()

                                }
                            };
                            obj1.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = en });
                            obj1.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = ar });

                            AudienceSegmentService.Save(obj1);
                        }
                    }
                    else
                    {
                        AudienceSegmentDto obj1 = new AudienceSegmentDto
                        {
                            OperatorSegmentCode = obj.OperatorSegmentCode,
                            CodeUQ = obj.CodeUQ,
                            Price = obj.Price,
                            Description = obj.Description,
                            IsSelectedable = obj.IsSelectedable,
                            ProviderId = obj.ProviderId,
                            IsDeleted = obj.IsDeleted,
                            ParentId = obj.ParentId,
                            IsPermissionNeed = obj.IsPermissionNeed,

                            Name = new LocalizedStringDto
                            {
                                GroupKey = "AudienceSegment",
                                Value = en,
                                Values = new List<LocalizedValueDto>()

                            }
                        };
                        obj1.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = en });
                        obj1.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = ar });

                        AudienceSegmentService.Save(obj1);
                    }

                }
                else
                {
                    AudienceSegmentDto obj1 = new AudienceSegmentDto
                    {
                        OperatorSegmentCode = obj.OperatorSegmentCode,
                        ID = obj.ID,
                        Price = obj.Price,
                        CodeUQ = obj.CodeUQ,
                        Description = obj.Description,
                        IsSelectedable = obj.IsSelectedable,
                        ProviderId = obj.ProviderId,
                        IsDeleted = obj.IsDeleted,
                        ParentId = obj.ParentId,
                        IsPermissionNeed = obj.IsPermissionNeed,

                        Name = new LocalizedStringDto
                        {
                            GroupKey = "AudienceSegment",
                            Value = en,
                            Values = new List<LocalizedValueDto>()

                        }
                    };
                    obj1.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "en-US", Value = en });
                    obj1.Name.Values.Add(new LocalizedValueDto { ID = 0, Culture = "ar-JO", Value = ar });

                    AudienceSegmentService.Save(obj1);


                }

                //object routeValues = new { param1 = param1, param2 = param2 };

                //string url = Url.Action("AjaxHtmlOutputMethod", "Controller", routeValues);

                //Response.RemoveOutputCacheItem(url);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e.Message });

            }

            return Json(new { result = true });
        }

        [HttpPost]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult DeleteSegment(int[] Segments)
        {
            try
            {


                if (Segments != null && Segments.Length > 0)

                {

                    foreach (int id in Segments)
                    {

                        var audSeg = AudienceSegmentService.get(new ValueMessageWrapper<int> { Value = id });
                        audSeg.IsDeleted = true;
                        AudienceSegmentService.Save(audSeg);
                    }
                }

            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e.Message });

            }

            return Json(new { result = true });
        }

        public ActionResult GetSegment(int id)
        {
            AudienceSegmentDto Segment = AudienceSegmentService.get(new ValueMessageWrapper<int> { Value = id });

            return Json(Segment);

        }

        [RequireHttps]
        public ActionResult GetCreativeFormatsSecure(string q, string culture)
        {
            return Json(ReturnCreativeFormatsResult(q, culture));
        }
       [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "q","culture" })]
        public ActionResult GetCreativeFormats(string q, string culture)
        {

            return Json(ReturnCreativeFormatsResult(q, culture));
        }


        private IEnumerable<CreativeFormatsDto> ReturnCreativeFormatsResult(string q, string culture)
        {
            var criteria = new CreativeFormatsCriteria() { Value = q, Culture = culture };
            var CreativeFormats = lookupService.CreativeFormatsGetByQuery(criteria);
            return CreativeFormats;
        }


    }
}
