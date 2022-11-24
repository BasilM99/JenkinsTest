using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Telerik.Web.Mvc;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using System.Text.RegularExpressions;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Controllers
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
        public LookupController(ILookupService lookupService, IManufacturerService manufacturerService, IPlatformService platformService, IPartyService PartyService, IAudienceSegmentService AudienceSegmentService)
        {
            this.lookupService = lookupService;
            this.manufacturerService = manufacturerService;
            this.platformService = platformService;
            this.PartyService = PartyService;
            this.AudienceSegmentService = AudienceSegmentService;
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

            var cratiria = new Noqoush.AdFalcon.Domain.Common.Repositories.Core.LookupCriteria()
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

            var cratiria = new Noqoush.AdFalcon.Domain.Common.Repositories.Core.DeviceLookupCriteria()
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
            var result = id.HasValue ? lookupService.GetVendorkeywords((int)id) : new List<CreativeVendorKeywordDto>();
            return View(new GridModel
            {
                Data = result,
                Total = 0
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


        public ActionResult Index(string id)
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
            return View(model);
        }

      


        [GridAction(EnableCustomBinding = true)]
        public ActionResult _Index(string id, string Name = "")
        {
            var result = lookupService.GetAllPageLookup(GetCriteria(id, Name));
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult CostElemetntPage(string Name = "")
        {


            //var result = lookupService.GetAllLookupByType(new LookupCriteriaBase { LookType = "costelement" });
            var result = lookupService.GetCostPageLookup(GetCriteria("costelement", Name));
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });

        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _IndexDevice(string id, string name, int? manufacturerId, int? platformId)
        {
            if (manufacturerId == 0)
                manufacturerId = null;
            if (platformId == 0)
                platformId = null;
            var result = lookupService.GetAllPageLookup(GetDeviceCriteria(id, name, manufacturerId, platformId));
            return View(new GridModel
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
            var result = lookupService.GetParentLocations(typeId);
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

        public ActionResult Item(int id, string type)
        {
            var criteria = new Noqoush.AdFalcon.Domain.Common.Repositories.Core.LookupGetCriteria()
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
            lookupService.SaveLookup(saveModel.LookupDto, id);
            return Content("Done");
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveDevice(string id, DeviceSaveModel saveModel)
        {
            lookupService.SaveLookup(saveModel.LookupDto, id);
            return Content("Done");
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveCostElement(string id, CostElementSaveModel saveModel)
        {
            var costElementDto = saveModel.LookupDto;
            var costElementValuesList = new List<CostElementValueDto>();

            //var flag = saveModel.LookupDto.IsBitwiseOr;
            foreach (var item in Request.Form.AllKeys.Where(p => p.StartsWith("LookupDto.CostModelValue-")))
            {
                CostElementValueDto valueDto = new CostElementValueDto();
                valueDto.CostModelWrapper = new CostModelWrapperDto();
                valueDto.CostModelWrapper.ID = int.Parse(item.Split('-')[1]);

                if (saveModel.LookupDto.TypeId == (int)Domain.Common.Model.Core.CostElement.CalculationType.Percentage)
                {
                    valueDto.Value = decimal.Parse(Request.Form[item]) / 100M;
                }
                else
                {
                    valueDto.Value = decimal.Parse(Request.Form[item]);
                }
             

                costElementValuesList.Add(valueDto);
            }
            costElementDto.Values = costElementValuesList;
            costElementDto.CostItemType = CostItemType.CostElement;
            lookupService.SaveLookup(saveModel.LookupDto, id);
            return Content("Done");
        }


        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveFee(string id, FeeSaveModel saveModel)
        {
            var costElementDto = saveModel.LookupDto;
            var costElementValuesList = new List<CostElementValueDto>();

            foreach (var item in Request.Form.AllKeys.Where(p => p.StartsWith("LookupDto.CostModelValue-")))
            {
                CostElementValueDto valueDto = new CostElementValueDto();
                valueDto.CostModelWrapper = new CostModelWrapperDto();
                valueDto.CostModelWrapper.ID = int.Parse(item.Split('-')[1]);

                if (saveModel.LookupDto.TypeId == (int)Domain.Common.Model.Core.CostElement.CalculationType.Percentage && saveModel.LookupDto.FeeCalculatedFrom!=FeeCalculatedFrom.System)
                {
                    valueDto.Value = decimal.Parse(Request.Form[item]) / 100M;
                }
                else if(saveModel.LookupDto.FeeCalculatedFrom != FeeCalculatedFrom.System)
                {
                    valueDto.Value = decimal.Parse(Request.Form[item]);
                }

                costElementValuesList.Add(valueDto);
            }
            costElementDto.Values = costElementValuesList;
            costElementDto.CostItemType = CostItemType.Fee;
            lookupService.SaveLookup(saveModel.LookupDto, id);
            return Content("Done");
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveManufacturer(string id, ManufacturerSaveModel saveModel)
        {
            lookupService.SaveLookup(saveModel.LookupDto, id);
            return Content("Done");
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SavePlatform(string id, PlatformSaveModel saveModel)
        {
            lookupService.SaveLookup(saveModel.LookupDto, id);
            return Content("Done");
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveAdvertiser(string id, AdvertiserSaveModel saveModel)
        {
            string Message = "";
            bool Result = false;
            try
            {
                Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), saveModel.LookupDto.Name.Value);

                saveModel.LookupDto.Name.Values[0].Value = saveModel.LookupDto.Name.Values[0].Value.Trim();
                saveModel.LookupDto.Name.Values[1].Value = saveModel.LookupDto.Name.Values[1].Value.Trim();

                lookupService.SaveLookup(saveModel.LookupDto, id);
                Result = true;
                Framework.Caching.CacheManager.Current.DefaultProvider.Remove("CacheByAttribute_AdvertiserService");
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
            }

            return Json(new { Message = Message, Result = Result });
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveDeviceCapability(string id, DeviceCapabilitySaveModel saveModel)
        {
            lookupService.SaveLookup(saveModel.LookupDto, id);
            return Content("Done");
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveAttribute(string id, AdCreativeAttributeSaveModel saveModel)
        {
            lookupService.SaveLookup(saveModel.LookupDto, id);
            return Content("Done");
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveAudienceSegment(string id, AdCreativeAttributeSaveModel saveModel)
        {
            lookupService.SaveLookup(saveModel.LookupDto, id);
            return Content("Done");
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveKeyword(string id, KeywordSaveModel saveModel)
        {
            string Message = "";
            bool Result = false;
            try
            {
                Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), saveModel.LookupDto.Name.Value);

                lookupService.SaveLookup(saveModel.LookupDto, id);
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

        public ActionResult SaveLanguage(string id, LanguageSaveModel saveModel)
        {
            string Message = "";
            bool Result = false;
            try
            {
                Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), saveModel.LookupDto.Name.Value);

                lookupService.SaveLookup(saveModel.LookupDto, id);
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

        public ActionResult SaveLocation(string id, LocationSaveModel saveModel)
        {
            var locationDto = saveModel.LookupDto as LocationDto;

            if (locationDto.ParentId.HasValue && locationDto.ParentId.Value == 0)
            {
                locationDto.ParentId = null;
            }

            // (saveModel.LookupDto as LocationDto).Type = LocationType.Continent;
            lookupService.SaveLookup(saveModel.LookupDto, id);

            return Content("Done");
        }
        [DenyRole(Roles = "AccountManager")]

        public ActionResult SaveCreativeVendor(string id, CreativeVendorSaveModel saveModel)
        {
            var keywords = saveModel.InsertedKeyWords[0].Split(',').ToList();
            saveModel.LookupDto.InsertedKeywords = keywords.Select(x => new CreativeVendorKeywordDto { Keyword = x }).Where(x => !string.IsNullOrEmpty(x.Keyword)).ToList();
            keywords = saveModel.DeletedKeyWords[0].Split(',').ToList();
            saveModel.LookupDto.DeletedKeywords = keywords.Select(x => new CreativeVendorKeywordDto { Keyword = x }).Where(x => !string.IsNullOrEmpty(x.Keyword)).ToList();


            lookupService.SaveLookup(saveModel.LookupDto, id);
            Framework.Caching.CacheManager.Current.DefaultProvider.Remove("CacheByAttribute_CreativeVendorService");
            return Content("Done");
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
        [OutputCache(Duration = 3600, VaryByParam = "id;q;culture")]
        public ActionResult GetAudienceSegmentsResult(int id , string q, string culture)
        {

            return Json(ReturnAudienceSegmentsResult(id,q, culture), JsonRequestBehavior.AllowGet);
        }




        private IEnumerable<AudienceSegmentDto> ReturnAudienceSegmentsResult(int id, string q, string culture)
        {

            var audianceSegments = this.AudienceSegmentService.getAudianceSegmentsByDataProvider(id, q, culture);

   
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

                        var audSeg = AudienceSegmentService.get(id);
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
            AudienceSegmentDto Segment = AudienceSegmentService.get(id);

            return Json(Segment);

        }

        [RequireHttps]
        public ActionResult GetCreativeFormatsSecure(string q, string culture)
        {
            return Json(ReturnCreativeFormatsResult(q, culture), JsonRequestBehavior.AllowGet);
        }
       [OutputCache(Duration = 21600, VaryByParam = "q;culture")]
        public ActionResult GetCreativeFormats(string q, string culture)
        {

            return Json(ReturnCreativeFormatsResult(q, culture), JsonRequestBehavior.AllowGet);
        }


        private IEnumerable<CreativeFormatsDto> ReturnCreativeFormatsResult(string q, string culture)
        {
            var criteria = new CreativeFormatsCriteria() { Value = q, Culture = culture };
            var CreativeFormats = lookupService.CreativeFormatsGetByQuery(criteria);
            return CreativeFormats;
        }


    }
}
