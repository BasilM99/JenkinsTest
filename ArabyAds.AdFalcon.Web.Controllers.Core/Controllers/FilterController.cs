using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;

using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

using ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Web.Controllers.Model.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using ArabyAds.Framework;
using ArabyAds.Framework.Utilities;
using Telerik.Web.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using Telerik.Web.Mvc.Extensions;
using Action = ArabyAds.AdFalcon.Web.Controllers.Model.Action;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using System.Globalization;

using System.Text.RegularExpressions;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Web.Controllers.Handler;
using Telerik.Web.Mvc.UI;
using System.Web;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using ArabyAds.AdFalcon.Web.Controllers.Model.User;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Web.Controllers.Model.PMPDeal;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using Newtonsoft.Json;
using ArabyAds.AdFalcon.Web.Controllers.Model.Advertiser;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Account;
using ArabyAds.AdFalcon.Exceptions;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite.Performance;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;


using ArabyAds.AdFalcon.Web.Controllers.Model.QueryBuilder;
using ArabyAds.AdFalcon.Domain.Common.Model.QueryBuilder;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QB;
using System.Dynamic;
using ArabyAds.AdFalcon.Web.Controllers.Model.Report;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider }, AuthorizeRoles = "Administrator,AccountManager,AdOps,AppOps", DenyImpersonationOnly = true)]
    public class FilterController : AuthorizedControllerBase
    {
        private IReportService _ReportService;
        protected ILookupService _lookupService;
        protected const string _nativeAdIconsGroup = "9";
        protected const string _nativeAdImagesGroup = "10";
        protected const string _separator = "&";
        protected const string _bidSeparator = ",";
        private const string IMPRESSIONEVENT = "000imp";
        private const string CLICKEVENT = "000clk";
        private IAppSiteTypeService _appSiteTypeService;
        protected IKeywordService _keywordService;
        protected ICampaignService _campaignService;
        protected IObjectiveTypeService _objectiveTypeService;
        protected ICreativeUnitService _creativeUnitService;
        protected IAgeGroupService _ageGroupService;
        protected IAudienceSegmentService _audienceSegmentService;
        private IAppSiteService _appSiteService;
        protected IAdvertiserService _AdvertiserService;

        protected ITileImageService _tileImageService;
        protected IAdCreativeStatusService _adCreativeStatusService;
        protected IDeviceCapabilityService _deviceCapabilityService;
        protected IRichMediaRequiredProtocolService _richMediaRequiredProtocolService;
        protected ILocationService _locationService;
        protected IDeviceTypeService _deviceTypeService;
        protected IPlatformService _platformService;
        protected ICostModelWrapperService _CostModelWrapperService;
        protected IVideoTypeService _videoTypeService;
        private ITrackingEventService _trackingEventService;
        protected IAccountService _accountService;
        protected IVideoDeliveryMethodsService _videoDeliveryMethodsService;
        protected IAppMarketingPartnerService _appMarketingPartnerServic;

        protected ICreativeVendorService _creativeVendorService;
        protected ILanguageService _languageService;
        protected IUserService _userService;
        protected IReportService _reportService;

        protected IPartyService _partyService;
        private ResultDataSetQBDto dataSet = null;
        private int selectedFactId;
        private Function function;
        private string Query = "";
        private string warningDiv = "<div><strong>{0}</strong> were excluded, invalid input</div>";
        private string limit = " limit 100 ;";
        private StringBuilder warnings = new StringBuilder();
        private string table;
        private int pagenumber = 0;
        private StringBuilder tableBody = new StringBuilder();
        private readonly JsonSerializerOptions _jsonOptions;

        public FilterController(IOptions<JsonOptions> jsonOptions)
        {
            _jsonOptions = jsonOptions.Value.JsonSerializerOptions;
            _lookupService = IoC.Instance.Resolve<ILookupService>();
            _audienceSegmentService = IoC.Instance.Resolve<IAudienceSegmentService>();
            _keywordService = IoC.Instance.Resolve<IKeywordService>();
            _campaignService = IoC.Instance.Resolve<ICampaignService>();
            _objectiveTypeService = IoC.Instance.Resolve<IObjectiveTypeService>();
            _creativeUnitService = IoC.Instance.Resolve<ICreativeUnitService>();
            _ageGroupService = IoC.Instance.Resolve<IAgeGroupService>();
            _tileImageService = IoC.Instance.Resolve<ITileImageService>();
            _adCreativeStatusService = IoC.Instance.Resolve<IAdCreativeStatusService>();
            _deviceCapabilityService = IoC.Instance.Resolve<IDeviceCapabilityService>();
            _richMediaRequiredProtocolService = IoC.Instance.Resolve<IRichMediaRequiredProtocolService>();
            _locationService = IoC.Instance.Resolve<ILocationService>();
            _deviceTypeService = IoC.Instance.Resolve<IDeviceTypeService>();
            _trackingEventService = IoC.Instance.Resolve<ITrackingEventService>();
            _platformService = IoC.Instance.Resolve<IPlatformService>();
            _CostModelWrapperService = IoC.Instance.Resolve<ICostModelWrapperService>();
            _videoTypeService = IoC.Instance.Resolve<IVideoTypeService>();
            _videoDeliveryMethodsService = IoC.Instance.Resolve<IVideoDeliveryMethodsService>();
            _appSiteTypeService = IoC.Instance.Resolve<IAppSiteTypeService>();
            _appMarketingPartnerServic = IoC.Instance.Resolve<IAppMarketingPartnerService>();
            _accountService = IoC.Instance.Resolve<IAccountService>();
            this._appSiteService = IoC.Instance.Resolve<IAppSiteService>();
            _AdvertiserService = IoC.Instance.Resolve<IAdvertiserService>();
            _userService = IoC.Instance.Resolve<IUserService>();
            _creativeVendorService = IoC.Instance.Resolve<ICreativeVendorService>();
            _languageService = IoC.Instance.Resolve<ILanguageService>();
            _reportService = IoC.Instance.Resolve<IReportService>();
            this._partyService = IoC.Instance.Resolve<IPartyService>();
        }


        private bool checkAdPermissions(PortalPermissionsCode Code)
        {

            bool result = _accountService.checkAdPermissions(new ValueMessageWrapper<PortalPermissionsCode> { Value = Code }).Value;

            return result;
        }

        //Fill Dims Types
        private FilterViewModel GetModel(int factid, string reportType = "ad")
        {

            bool forPublisher = reportType == "app";
            FilterViewModel model = new FilterViewModel();

            #region dropDwonlists
            model.Facts = new List<SelectListItem>();
            model.Dimensions = new List<SelectListItem>();
        //    model.Dimensions.Add(new SelectListItem { Value = "-1", Text = "Please Select", Selected = true });


            List<FactDto> Facts = this._reportService.GetAllFacts().ToList();
            if (Config.IsAdministrationApp)
            {
                model.Facts.AddRange(Facts.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.DisplayName }).OrderBy(x => x.Value).ToList());
            }
            else
            {
                model.Facts.AddRange(Facts.Where(x => x.IsForWeb == true).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.WebDisplayName }).OrderBy(x => x.Value).ToList());

            }
            var fact = Facts.Where(x => x.Id == factid).SingleOrDefault();
            var dimensions = fact.Dimensions;
            if (forPublisher)
                dimensions = dimensions.Where(x => x.SupportedByPublisher).ToList();
            else
                dimensions = dimensions.Where(x => x.SupportedByAdvertiser).ToList();
            if (Domain.Configuration.IsAdminOrAdOps)
            {
                model.Dimensions.AddRange(dimensions.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).OrderBy(x => x.Text).ToList());
            }
            else
            {
                model.Dimensions.AddRange(dimensions.Where(M => M.Id != 25).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).OrderBy(x => x.Text).ToList());

            }

            selectedFactId = factid;
            #endregion
            DateTime today = ArabyAds.Framework.Utilities.Environment.GetServerTime();
            var yesterday = today.AddDays(-1);

            model.Dto = yesterday.ToShortDateString();
            model.Dfrom = new DateTime(yesterday.Year, yesterday.Month, 1).ToShortDateString();
            model.DimensionsTree = new TreeViewMode
            {
                Code = "Dimensions",
                Name = "Dimensions",
                ControllerName = "Tree",
                ActionName = "Get",
                IsAjax = true,
                Id = "1"
            };
            model.MeasuresTree = new TreeViewMode
            {
                Code = "Measures",
                Name = "Measures",
                ControllerName = "Tree",
                IsAjax = true,
                ActionName = "Get",
                Id = "2"
            };

            model.DimensionsMaxSelect = Convert.ToInt32(JsonConfigurationManager.AppSettings["DimensionsMaxSelect"]);

            model.SchedulingViewModel = GetCampaignSchadulingReportModel(0);

            model.CriteriaIDs = Config.ConfigForCriteriaReportBuilder;
            model.SearchstringCriteriaIDs = Config.ConfigForCriteriaSearchReportBuilder;
            model.ConfigForMeasureDimensionFilter = Config.ConfigForMeasureDimensionFilter;
            return model;

        }
        public ActionResult SubFilter(int id, int factid = 1)
        {
            var model = GetModel(factid);

            model.Id = ++id;
            return PartialView(model);
        }
        public ActionResult SubFilterForTargeting(int id, int factid = 1)
        {
            var model = new FilterViewModel();
            model.Dimensions = new List<SelectListItem>();
            model.Dimensions.Add(new SelectListItem { Value = "-1", Text = "Please Select", Selected = true });
          var dimtype=  _reportService.GetDimensionsType();
            model.Dimensions.AddRange(dimtype.Select(x => new SelectListItem { Value = x.DimensionType.ToString(), Text = x.Name }).OrderBy(x => x.Text).ToList()); ;
            model.Id = ++id;

            return PartialView(model);
        }
        public ActionResult SubFilterForBidModifier(int id, int factid = 1)
        {
            var model = new FilterViewModel();
            model.Dimensions = new List<SelectListItem>();
            model.Dimensions.Add(new SelectListItem { Value = "-1", Text = "Please Select", Selected = true });
            var dimtype = _reportService.GetDimensionsType();
            model.Dimensions.AddRange(dimtype.Select(x => new SelectListItem { Value = x.DimensionType.ToString(), Text = x.Name }).OrderBy(x => x.Text).ToList()); ;
            model.Id = ++id;
            //
            return PartialView(model);
        }


        
        public ActionResult MultiSelectQB(int id, string selectedId, string ListOfIds)
        {

            Model.QueryBuilder.Select2ViewModel model = new Model.QueryBuilder.Select2ViewModel
            {
                Code = id.ToString(),
                ActionName = "GetSelect2Elements",
                Id = selectedId,
                ListOfIds = ListOfIds
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 400, VaryByQueryKeys = new string[] { "id", "selectedId", "ListOfIds" })]
        public ActionResult MultiSelectBid(int id, string selectedId, string ListOfIds)
        {
            if (selectedId == "null" || string.IsNullOrWhiteSpace(selectedId))
                return  new EmptyResult();
            var dimdto = _reportService.GetDimensionIdByType(new ValueMessageWrapper<int> { Value = Convert.ToInt32(selectedId) });
            if (Convert.ToInt32(selectedId) != (int)ArabyAds.AdFalcon.Domain.Common.Model.Campaign.DimentionType.Geofence && Convert.ToInt32(selectedId) != (int)ArabyAds.AdFalcon.Domain.Common.Model.Campaign.DimentionType.Time)
            {
                Model.QueryBuilder.Select2ViewModel model = new Model.QueryBuilder.Select2ViewModel
                {
                    Code = id.ToString(),
                    ActionName = "GetSelect2ElementsForBid",
                    Id = dimdto.Id.ToString(),
                    ListOfIds = ListOfIds,
                    callBackFunction = "callBackModifierDimVal"
                };
                return PartialView(model);
            }
            else if (Convert.ToInt32(selectedId) == (int)ArabyAds.AdFalcon.Domain.Common.Model.Campaign.DimentionType.Time)
            {
                if (string.IsNullOrEmpty(ListOfIds) || ListOfIds == "undefined")
                {
                    ListOfIds = string.Empty;
                    return PartialView("TimeControl", new ArabyAds.AdFalcon.Web.Controllers.Model.Core.TimeViewModel { Name = "BidModfierSelect_" + id.ToString(), Hour = null, Min = null, callBackFunction = "callBackModifierDimVal",Code = id.ToString() });
                }
          

                
            else
                {
                    DateTime dateTime = DateTime.ParseExact(ListOfIds, "HH:mm",
                                           CultureInfo.InvariantCulture);

                    return PartialView("TimeControl", new ArabyAds.AdFalcon.Web.Controllers.Model.Core.TimeViewModel { Name = "BidModfierSelect_" + id.ToString(), Hour = dateTime.Hour, Min = dateTime.Minute, callBackFunction = "callBackModifierDimVal" ,Code=id.ToString()});
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ListOfIds) || ListOfIds == "undefined")
                    ListOfIds = string.Empty;
                return PartialView("TextControl", new ArabyAds.AdFalcon.Web.Controllers.Model.Core.TextViewModel { Name = "BidModfierSelect_" + id.ToString(), Value = ListOfIds, callBackFunction = "callBackModifierDimVal",Code = id.ToString() });


            }
        }
        
        [NonAction]
        public IList<Select2> GetSelect2ElementsForBidInternal(string id, string q, string t, int selectedFactIdVar, int page, string Ids, string lang = "en")
        {
            List<Select2> list = new List<Select2>();
            string subId = string.Empty;
            int dimID = Convert.ToInt32(id);
            DimensionDto dim = this._reportService.GetDimensionById(new ValueMessageWrapper<int> { Value = dimID });
            FactDto fact = this._reportService.GetFactById(new ValueMessageWrapper<int> { Value = selectedFactIdVar });
            List<int> TagIds = new List<int>();
            // List<int> SubIds = new List<int>();
            if (!string.IsNullOrEmpty(Ids))

                TagIds = Ids.Split(',').Select(int.Parse).ToList();
            else

                Ids = string.Empty;
            string firstPart = string.Empty;
            if (!string.IsNullOrEmpty(t))

            {
                //SubIds = t.Split(',').Select(int.Parse).ToList();
                int lastindexSimi = t.LastIndexOf(":");
                firstPart = t.Substring(0, lastindexSimi);
                int firstindexSimi = firstPart.LastIndexOf(":");
                subId = firstPart.Substring(0, firstindexSimi);
                firstPart = firstPart.Substring(firstindexSimi + 1);
                t = t.Substring(lastindexSimi + 1);


            }
            else
                t = string.Empty;

            if (q == null)
            { q = string.Empty; }
            if (dim.IsSql)
            {
                string script = string.Empty;

                if (dim.IsScoped)
                {
                    var idsSting = string.Format(dim.ScopeTableName + ".Id = ({0})", Ids);

                    script = string.Format(dim.Source, dim.Attributes, dim.IsEnum ? fact.Name : dim.ScopeTableName, dim.FilterCol, q.ToLower(), "=" + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
                    if (!string.IsNullOrEmpty(Ids))
                    {

                        script = script + " AND " + idsSting;
                    }

                    if (!string.IsNullOrEmpty(t))
                    {
                        var idsSting1 = string.Format(firstPart, t);
                        script = script.Replace("search", " AND " + idsSting1);

                    }
                    script = script.Replace("search", "  ");

                }
                else
                {

                    var idsSting = string.Format(dim.TableName + ".Id = ({0})", Ids);
                    script = string.Format(dim.Source, dim.Attributes, dim.IsEnum ? fact.Name : dim.TableName, dim.FilterCol, q.ToLower());

                    if (!string.IsNullOrEmpty(Ids) && (dim.Id != 7 && dim.Id != 8 && dim.Id != 9))
                    {

                        script = script + " AND " + idsSting;
                    }
                    else if (!string.IsNullOrEmpty(Ids))
                    {
                        script = script.Replace("search", "search AND " + idsSting);

                    }

                    if (!string.IsNullOrEmpty(t))
                    {

                        var idsSting1 = string.Format(firstPart, t);
                        script = script.Replace("search", " AND " + idsSting1);




                    }


                    script = script.Replace("search", "  ");
                }

                List<DataQBDto> Data = new List<DataQBDto>();


                if (!dim.IsScoped)
                    Data = this._reportService.GetResultofDataQBDto(new GetResultofDataQBDtoRequest { Script = script, OptionalDrop = string.Empty, MethodName = dim.TableName, Page = page, Ids = Ids });
                else
                    Data = this._reportService.GetResultofDataQBDtoWithScoping(new GetResultofDataQBDtoWithScopingRequest { Script = script, OptionalDrop = string.Empty, MethodName = dim.ScopeTableName, Page = page, Ids = Ids });


                if (Data != null && Data.Count > 0 && !string.IsNullOrEmpty(Data.First().ParentName) && string.IsNullOrEmpty(Ids))
                {
                    if (!string.IsNullOrEmpty(Data.First().SuperParentName))
                    {
                        var DataSuperParentName = Data.GroupBy(user => user.SuperParentName);
                        foreach (var groupRoot in DataSuperParentName)
                        {
                            var DataParentName = groupRoot.GroupBy(m => m.ParentName);
                            Select2 rootvar = new Select2 { text = groupRoot.Key, TotalCount = Data.First().TotalCount };
                            rootvar.children = new List<Select2>();
                            foreach (var item in DataParentName)
                            {
                                Select2 itemvar = new Select2 { text = item.Key, TotalCount = Data.First().TotalCount };
                                itemvar.children = new List<Select2>();
                                rootvar.children.Add(itemvar);
                                foreach (var x in item)
                                {
                                    Select2 itemOneVar = new Select2 { text = x.Name, id = x.Id, TotalCount = x.TotalCount };
                                    itemvar.children.Add(itemOneVar);
                                }
                            }
                            list.Add(rootvar);
                        }
                    }
                    else
                    {
                        // var DataSuperParentName = Data.GroupBy(user => user.SuperParentName);
                        // foreach (var groupRoot in DataSuperParentName)
                        {
                            var DataParentName = Data.GroupBy(m => m.ParentName);
                            // Select2 rootvar = new Select2 { Text = groupRoot.Key };
                            //  rootvar.children = new List<Select2>();
                            foreach (var item in DataParentName)
                            {
                                Select2 itemvar = new Select2 { text = item.Key };
                                itemvar.children = new List<Select2>();
                                //rootvar.children.Add(itemvar);
                                foreach (var x in item)
                                {
                                    Select2 itemOneVar = new Select2 { text = x.Name, id = x.Id, TotalCount = x.TotalCount };
                                    itemvar.children.Add(itemOneVar);

                                }

                                list.Add(itemvar);
                            }

                        }
                    }

                }
                else
                {
                    list = Data.Select(x => new Select2 { text = x.Name, id = x.Id, TotalCount = x.TotalCount }).ToList();
                }
            }
            else
            {
                List<string> sourceItmes = dim.Source.Split(',').ToList();
                if (sourceItmes.Count() > 0)
                {
                    sourceItmes = sourceItmes.Where(x => x.ToLower().Contains(q.ToLower())).ToList();

                }
                if (TagIds.Count == 0)
                {
                    for (int i = 0; i < sourceItmes.Count; i++)
                    {

                        list.Add(new Select2 { text = sourceItmes[i], id = Convert.ToInt32(dim.Attributes.Split(',').ToArray()[i]), TotalCount = sourceItmes.Count() });

                    }
                }
                else
                {

                    for (int i = 0; i < sourceItmes.Count; i++)
                    {
                        if (TagIds.Contains(Convert.ToInt32(dim.Attributes.Split(',').ToArray()[i])))
                        {
                            list.Add(new Select2 { text = sourceItmes[i], id = Convert.ToInt32(dim.Attributes.Split(',').ToArray()[i]), TotalCount = sourceItmes.Count() });
                        }
                    }

                }
            }
            return list;
        }

        [OutputCache(Duration = 400, VaryByQueryKeys = new string[] { "id", "q", "t", "selectedFactIdVar", "page" , "Ids", "lang" })]
        public ActionResult GetSelect2ElementsForBid(string id, string q, string t, int selectedFactIdVar, int page, string Ids, string lang = "en")
        {
            var list = GetSelect2ElementsForBidInternal(id, q, t, selectedFactIdVar, page, Ids, lang);
            return Json(list);
        }

        [NonAction]
        public IList<Select2> GetSelect2ElementsInternal(string id, string q, string t, int selectedFactIdVar, int page, string Ids)
        {
            List<Select2> list = new List<Select2>();
            string subId = string.Empty;
            int dimID = Convert.ToInt32(id);
            DimensionDto dim = this._reportService.GetDimensionById(new ValueMessageWrapper<int> { Value = dimID });
            FactDto fact = this._reportService.GetFactById(new ValueMessageWrapper<int> { Value = selectedFactIdVar });
            List<int> TagIds = new List<int>();
            // List<int> SubIds = new List<int>();
            if (!string.IsNullOrEmpty(Ids))

                TagIds = Ids.Split(',').Select(int.Parse).ToList();
            else

                Ids = string.Empty;
            string firstPart = string.Empty;
            if (!string.IsNullOrEmpty(t))

            {
                //SubIds = t.Split(',').Select(int.Parse).ToList();
                int lastindexSimi = t.LastIndexOf(":");
                firstPart = t.Substring(0, lastindexSimi);
                int firstindexSimi = firstPart.LastIndexOf(":");
                subId = firstPart.Substring(0, firstindexSimi);
                firstPart = firstPart.Substring(firstindexSimi + 1);
                t = t.Substring(lastindexSimi + 1);

                /*
                if (t.IndexOf("2,") == 0)
                {
                    subId = "2";
                  t=  t.Replace("2,", string.Empty);
                }
                else if (t.IndexOf("1,") == 0)
                {
                    subId = "1";
                    t = t.Replace("1,", string.Empty);
                }
                else if (t.IndexOf("21,") == 0)
                {
                    subId = "21";
                    t = t.Replace("21,", string.Empty);
                }
                else if (t.IndexOf("6,") == 0)
                {
                    subId = "6";
                    t = t.Replace("6,", string.Empty);
                }
                else if (t.IndexOf("7,") == 0)
                {
                    subId = "7";
                    t = t.Replace("7,", string.Empty);
                }
                else if (t.IndexOf("9,") == 0)
                {
                    subId = "9";
                    t = t.Replace("9,", string.Empty);
                }
                else if (t.IndexOf("8,") == 0)
                {
                    subId = "8";
                    t = t.Replace("8,", string.Empty);
                }
                else if (t.IndexOf("11,") == 0)
                {
                    subId = "11";
                    t = t.Replace("11,", string.Empty);
                }
                else if (t.IndexOf("12,") == 0)
                {
                    subId = "12";
                    t = t.Replace("12,", string.Empty);
                }
                else if (t.IndexOf("13,") == 0)
                {
                    subId = "13";
                    t = t.Replace("13,", string.Empty);
                }

                else if (t.IndexOf("27,") == 0)
                {
                    subId = "27";
                    t = t.Replace("27,", string.Empty);
                }*/

            }
            else
                t = string.Empty;

            if (q == null)
            { q = string.Empty; }
            if (dim.IsSql)
            {
                string script = string.Empty;

                if (dim.IsScoped)
                {
                    var idsSting = string.Format(dim.ScopeTableName + ".Id in ({0})", Ids);

                    script = string.Format(dim.Source, dim.Attributes, dim.IsEnum ? fact.Name : dim.ScopeTableName, dim.FilterCol, q.ToLower(), "=" + Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
                    if (!string.IsNullOrEmpty(Ids))
                    {
                        int searchcount= script.IndexOf("search");
                        script = script.Insert(searchcount, " AND " + idsSting+"  ");
                    }

                    if (!string.IsNullOrEmpty(t))
                    {
                        var idsSting1 = string.Format(firstPart, t);
                        script = script.Replace("search", " AND " + idsSting1);

                        /*
                                                if (subId == "1")
                                                {
                                                    var idsSting1 = string.Format( "campaigns.Id in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting1);
                                                }
                                                else if (subId == "2")
                                                {
                                                    var idsSting2 = string.Format("adgroups.Id in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting2);

                                                }
                                                else if (subId == "6")
                                                {
                                                    var idsSting2 = string.Format("ads.Id in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting2);

                                                }
                                                else if (subId == "21")
                                                {
                                                    var idsSting2 = string.Format("campaigns.AssociationAdvId in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting2);

                                                }
                                                else if (subId == "7")
                                                {
                                                    var idsSting2 = string.Format("Country.Id in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting2);

                                                }
                                                else if (subId == "9")
                                                {
                                                    var idsSting2 = string.Format("Region.Id in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting2);

                                                }
                                                else if (subId == "8")
                                                {
                                                    var idsSting2 = string.Format("City.Id in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting2);

                                                }
                                                else if (subId == "11")
                                                {
                                                    var idsSting2 = string.Format("manufacturerid in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting2);

                                                }
                                                else if (subId == "12")
                                                {

                                                        var idsSting2 = string.Format("devicetypeid in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting2);


                                                }
                                                else if (subId == "13")
                                                {
                                                    var idsSting2 = string.Format("platformid in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting2);

                                                }
                                                else if (subId == "27")
                                                {
                                                    var idsSting2 = string.Format("dim_devices.Id in ({0})", t);
                                                    script = script.Replace("search", " AND " + idsSting2);

                                                }
                                                */

                    }
                    script = script.Replace("search", "  ");

                }
                else
                {

                    var idsSting = string.Format(dim.TableName + ".Id in ({0})", Ids);
                    script = string.Format(dim.Source, dim.Attributes, dim.IsEnum ? fact.Name : dim.TableName, dim.FilterCol, q.ToLower());

                    if (!string.IsNullOrEmpty(Ids) && (dim.Id != 7 && dim.Id != 8 && dim.Id != 9))
                    {


                        int searchcount = script.IndexOf("search");
                        if(searchcount>0)
                        script = script.Insert(searchcount, " AND " + idsSting + "  ");
                        else
                        script = script + " AND " + idsSting;
                    }
                    else if (!string.IsNullOrEmpty(Ids))
                    {
                        script = script.Replace("search", "search AND " + idsSting);

                    }

                    if (!string.IsNullOrEmpty(t))
                    {

                        var idsSting1 = string.Format(firstPart, t);
                        script = script.Replace("search", " AND " + idsSting1);

                        /*
                        if (subId == "1")
                        {
                            var idsSting1 = string.Format("campaigns.Id in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting1);
                        }
                        else if (subId == "2")
                        {
                            var idsSting2 = string.Format("adgroups.Id in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting2);

                        }
                        else if (subId == "6")
                        {
                            var idsSting2 = string.Format("ads.Id in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting2);

                        }
                        else if (subId == "21")
                        {
                            var idsSting2 = string.Format("campaigns.AssociationAdvId in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting2);

                        }
                        else if (subId == "7")
                        {
                            var idsSting2 = string.Format("Country.Id in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting2);

                        }
                        else if (subId == "9")
                        {
                            var idsSting2 = string.Format("Region.Id in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting2);

                        }
                        else if (subId == "8")
                        {
                            var idsSting2 = string.Format("City.Id in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting2);

                        }
                        else if (subId == "11")
                        {
                            var idsSting2 = string.Format("manufacturerid in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting2);

                        }
                        else if (subId == "12")
                        {

                            var idsSting2 = string.Format("devicetypeid in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting2);


                        }
                        else if (subId == "13")
                        {
                            var idsSting2 = string.Format("platformid in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting2);

                        }
                        else if (subId == "27")
                        {
                            var idsSting2 = string.Format("dim_devices.Id in ({0})", t);
                            script = script.Replace("search", " AND " + idsSting2);

                        }

                        */



                    }


                    script = script.Replace("search", "  ");
                }

                List<DataQBDto> Data = new List<DataQBDto>();


                if (!dim.IsScoped)
                    Data = this._reportService.GetResultofDataQBDto(new GetResultofDataQBDtoRequest { Script = script, OptionalDrop = string.Empty, MethodName = dim.TableName, Page = page });
                else
                    Data = this._reportService.GetResultofDataQBDtoWithScoping(new GetResultofDataQBDtoWithScopingRequest { Script = script, OptionalDrop = string.Empty, MethodName = dim.ScopeTableName, Page = page });


                if (Data != null && Data.Count > 0 && !string.IsNullOrEmpty(Data.First().ParentName) && string.IsNullOrEmpty(Ids))
                {
                    if (!string.IsNullOrEmpty(Data.First().SuperParentName))
                    {
                        var DataSuperParentName = Data.GroupBy(user => user.SuperParentName);
                        foreach (var groupRoot in DataSuperParentName)
                        {
                            var DataParentName = groupRoot.GroupBy(m => m.ParentName);
                            Select2 rootvar = new Select2 { text = groupRoot.Key, TotalCount = Data.First().TotalCount };
                            rootvar.children = new List<Select2>();
                            foreach (var item in DataParentName)
                            {
                                Select2 itemvar = new Select2 { text = item.Key, TotalCount = Data.First().TotalCount };
                                itemvar.children = new List<Select2>();
                                rootvar.children.Add(itemvar);
                                foreach (var x in item)
                                {
                                    Select2 itemOneVar = new Select2 { text = x.Name, id = x.Id, TotalCount = x.TotalCount };
                                    itemvar.children.Add(itemOneVar);
                                }
                            }
                            list.Add(rootvar);
                        }
                    }
                    else
                    {
                        // var DataSuperParentName = Data.GroupBy(user => user.SuperParentName);
                        // foreach (var groupRoot in DataSuperParentName)
                        {
                            var DataParentName = Data.GroupBy(m => m.ParentName);
                            // Select2 rootvar = new Select2 { Text = groupRoot.Key };
                            //  rootvar.children = new List<Select2>();
                            foreach (var item in DataParentName)
                            {
                                Select2 itemvar = new Select2 { text = item.Key };
                                itemvar.children = new List<Select2>();
                                //rootvar.children.Add(itemvar);
                                foreach (var x in item)
                                {
                                    Select2 itemOneVar = new Select2 { text = x.Name, id = x.Id, TotalCount = x.TotalCount };
                                    itemvar.children.Add(itemOneVar);

                                }

                                list.Add(itemvar);
                            }

                        }
                    }

                }
                else
                {
                    list = Data.Select(x => new Select2 { text = x.Name, id = x.Id, TotalCount = x.TotalCount }).ToList();
                }
            }
            else
            {
                List<string> sourceItmes = dim.Source.Split(',').ToList();
                if (sourceItmes.Count() > 0)
                {
                    sourceItmes = sourceItmes.Where(x => x.ToLower().Contains(q.ToLower())).ToList();

                }
                if (TagIds.Count == 0)
                {
                    for (int i = 0; i < sourceItmes.Count; i++)
                    {

                        list.Add(new Select2 { text = sourceItmes[i], id = Convert.ToInt32(dim.Attributes.Split(',').ToArray()[i]), TotalCount = sourceItmes.Count() });

                    }
                }
                else
                {

                    for (int i = 0; i < sourceItmes.Count; i++)
                    {
                        if (TagIds.Contains(Convert.ToInt32(dim.Attributes.Split(',').ToArray()[i])))
                        {
                            list.Add(new Select2 { text = sourceItmes[i], id = Convert.ToInt32(dim.Attributes.Split(',').ToArray()[i]), TotalCount = sourceItmes.Count() });
                        }
                    }

                }
            }
            return list;
        }
        //fill dim values 
        public ActionResult GetSelect2Elements(string id, string q, string t, int selectedFactIdVar, int page, string Ids)
        {
            var list = GetSelect2ElementsInternal(id,  q, t, selectedFactIdVar, page, Ids);
            return Json(list);
        }



        [HttpPost]
        public ActionResult SaveReportSchedulingReport([FromBody]CampaignReportSchedulingSaveModel CampaignReportScheduling)
        {
            ResponseStatus status = ResponseStatus.success;
            int id = 0;


            IList<int> TempColumns = new List<int>();

            try
            {
                var AllReportRecipient = !string.IsNullOrEmpty(CampaignReportScheduling.AllReportRecipient) ? CampaignReportScheduling.AllReportRecipient.Split(',').ToList() : new List<string>();
                ReportSchedulerDto reportSchedulerDto = new ReportSchedulerDto
                {
                    AllReportRecipient = AllReportRecipient.Select(x => new ReportRecipientDTO { Email = x }).ToList(),
                    Name = CampaignReportScheduling.Name,
                    EndDate = CampaignReportScheduling.SchedulingEndtDate.HasValue ? (DateTime?)new DateTime(CampaignReportScheduling.SchedulingEndtDate.Value.Year, CampaignReportScheduling.SchedulingEndtDate.Value.Month, CampaignReportScheduling.SchedulingEndtDate.Value.Day, 23, 59, 0) : null,
                    StartDate = CampaignReportScheduling.SchedulingStartDate,
                    TimeSentAt = CampaignReportScheduling.TimeSentAt.HasValue ? CampaignReportScheduling.TimeSentAt.Value : Framework.Utilities.Environment.GetServerTime(),
                    RecurrenceType = CampaignReportScheduling.RecurrenceType,
                    EmailIntroduction = CampaignReportScheduling.EmailIntroduction,
                    ReportSectionType = CampaignReportScheduling.ReportSectionType,
                    MatixColumns = TempColumns,
                    DateRecurrenceType = CampaignReportScheduling.DateRecurrenceType,
                    WeekDay = CampaignReportScheduling.WeekDay,
                    MonthDay = CampaignReportScheduling.MonthDay,
                    AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                    EmailSubject = CampaignReportScheduling.EmailSubject,
                    PreferedName = CampaignReportScheduling.PreferedName,
                    IsActive = CampaignReportScheduling.IsActive,
                    ID = CampaignReportScheduling.Id,
                    IsSunday = CampaignReportScheduling.IsSunday,
                    IsMonday = CampaignReportScheduling.IsMonday,
                    IsTuesday = CampaignReportScheduling.IsTuesday,
                    IsWednesday = CampaignReportScheduling.IsWednesday,
                    IsThursday = CampaignReportScheduling.IsThursday,
                    IsFriday = CampaignReportScheduling.IsFriday,

                    //ReportCriteriaDto
                    ReportDto = new ReportCriteriaDto
                    {
                        QueryJsonData = CampaignReportScheduling.QueryJsonData,
                        ColumnsIdsString = CampaignReportScheduling.ColumnsIdsString,
                        MeasuresIdsString = CampaignReportScheduling.MeasuresIdsString,
                        SummaryBy = CampaignReportScheduling.SummaryBy,
                        ColumnsIds = string.IsNullOrEmpty(CampaignReportScheduling.ColumnsIdsString) ? new List<int>() :
                        CampaignReportScheduling.ColumnsIdsString.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList(),
                        MeasuresIds = string.IsNullOrEmpty(CampaignReportScheduling.MeasuresIdsString) ? new List<int>() : CampaignReportScheduling.MeasuresIdsString.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList(),
                        fact = CampaignReportScheduling.fact,
                        IncludeId = CampaignReportScheduling.IncludeId,
                        ToDate = new DateTime(CampaignReportScheduling.ToDate.Year, CampaignReportScheduling.ToDate.Month, CampaignReportScheduling.ToDate.Day, 23, 59, 0),
                        FromDate = new DateTime(CampaignReportScheduling.FromDate.Year, CampaignReportScheduling.FromDate.Month, CampaignReportScheduling.FromDate.Day, 0, 0, 0),
                        AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value
                    }
                };
                reportSchedulerDto.IsForQueryBuilder = true;
                id = _reportService.SaveSchadulingReport(reportSchedulerDto).Value;
                AddSuccessfullyMsg("savedSuccessfully", "Global", string.Empty);
            }
            catch (Exception ex)
            {
                status = ResponseStatus.businessException;
                AddErrorMsgs(ResourcesUtilities.GetResource("Exception", "Global"));
            }

            return Json(id, "Report Scheduling", status);
        }

        private CampaignReportSchedulingViewModel GetSchadulingReportModel(int? id, string reportType = "ad")
        {

            CampaignReportSchedulingViewModel model = new CampaignReportSchedulingViewModel();
            var RecurrenceTypeSelection = RecurrenceType.Month;
            int MonthSelection = 1, DaysSelection = 1;
            IList<int> ColumnsToBeSelected = new List<int>();

            if (id.HasValue && id > 0)
            {
                model.ReportSchedulerDto = _reportService.GetSchadulingReport(new ValueMessageWrapper<int> { Value = (int)id });
                model.RecipientEmail = model.ReportSchedulerDto.AllReportRecipient.Select(x => x.Email).ToList();
                RecurrenceTypeSelection = model.ReportSchedulerDto.RecurrenceType;
                MonthSelection = model.ReportSchedulerDto.MonthDay;
                model.ReportSchedulerDto.Status = (model.ReportSchedulerDto.EndDate != null && model.ReportSchedulerDto.EndDate < Framework.Utilities.Environment.GetServerTime()) || model.ReportSchedulerDto.NextFireTime == null ? ResourcesUtilities.GetResource("Active", "JobGrid") : ResourcesUtilities.GetResource("NotActive", "JobGrid");
                model.ReportSchedulerDto.ReportDto.FromDateString = model.ReportSchedulerDto.ReportDto.RFromDate.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = model.ReportSchedulerDto.ReportDto.RToDate.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                DaysSelection = (int)model.ReportSchedulerDto.WeekDay;
            }
            else
            {
                model.ReportSchedulerDto = new ReportSchedulerDto();
                model.RecipientEmail = new List<string>();
                model.ReportSchedulerDto.ReportDto = new ReportCriteriaDto();
                model.ReportSchedulerDto.Name = ResourcesUtilities.GetResource("QueryScheduleNameDefault", "Report");
                model.ReportSchedulerDto.ReportDto.FromDate = Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.ReportDto.ToDate = Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.DateRecurrenceType = DateRecurrenceType.Today;
                model.ReportSchedulerDto.ReportDto.FromDateString = Framework.Utilities.Environment.GetServerTime().ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = Framework.Utilities.Environment.GetServerTime().ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.TabId = reportType == "ad" ? "campaign" : "App";
                model.ReportSchedulerDto.ReportDto.SummaryBy = 4;
                model.ReportSchedulerDto.ReportDto.CriteriaOpt = "all";
                model.ReportSchedulerDto.ReportDto.Layout = "summary";
                model.ReportSchedulerDto.IsSunday = true;
                model.ReportSchedulerDto.EmailIntroduction = "";
                model.ReportSchedulerDto.IsActive = true;
                model.ReportSchedulerDto.ReportDto.GroupByName = false;
                model.ReportSchedulerDto.ReportDto.DeviceCategory = "platform";
                model.ReportSchedulerDto.StartDate = ArabyAds.Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.EmailSubject = ResourcesUtilities.GetResource("DefaultSubject", "Report");
            }

            model.Time = new List<SelectListItem> {
                 new SelectListItem{Text = ResourcesUtilities.GetResource("Monthly", "Time") ,Value = ((int)RecurrenceType.Month).ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Month }
                 ,new SelectListItem {Text =ResourcesUtilities.GetResource("Weekly", "Time") ,Value=((int)RecurrenceType.Week).ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Week}
                 , new SelectListItem{Text =ResourcesUtilities.GetResource("Daily", "Time"),Value=((int)RecurrenceType.Day).ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Day}
                };

            return model;
        }





      //  [PermissionsAuthorize(Permission = PortalPermissionsCode.QueryBuilder, Roles = "Administrator,adops,AccountManager")]
        [HttpGet]
        public ActionResult GetFilter(int? id, int factid = 1, string reportType="ad")
        {
            var model = GetModel(factid, reportType);

            model.SchedulingViewModel = GetSchadulingReportModel(id);
            if (Config.IsAdOpsAdminInAdminApp || Config.IsAppOpsAdminInAdminApp)
                ViewBag.SchadulingReportAllowed = true;
            else
                ViewBag.SchadulingReportAllowed = _accountService.checkAdPermissions(new ValueMessageWrapper<PortalPermissionsCode> { Value = PortalPermissionsCode.ReportSchedule }).Value || Config.IsAppOps;

            if (model.SchedulingViewModel.ReportSchedulerDto.ReportDto.fact > 0)
            {
                model.FactId = model.SchedulingViewModel.ReportSchedulerDto.ReportDto.fact;
                model.IncludeId = model.SchedulingViewModel.ReportSchedulerDto.ReportDto.IncludeId;

            }
            if (model?.SchedulingViewModel?.ReportSchedulerDto?.ReportDto?.QueryJsonData != null)
            {
               var filters = System.Text.Json.JsonSerializer.Deserialize<IDictionary<int, int[]>>(model.SchedulingViewModel.ReportSchedulerDto.ReportDto.QueryJsonData, _jsonOptions);
                if(filters != null)
                {
                    foreach (var filter in filters)
                    {
                        model.SchedulingViewModel.ReportSchedulerDto.ReportDto.DimensionValues[filter.Key] = GetSelect2ElementsInternal(filter.Key.ToString(), null, null, 1, 1, string.Join(",", filter.Value)).Select(m => (object)new { label = m.text, value = m.id }).ToList() ;
                    }
                }

            }
               
            return Json(model);
        }

        [PermissionsAuthorize(Permission = PortalPermissionsCode.QueryBuilder, Roles = "Administrator,adops,AccountManager,AppOps")]

        [HttpGet]

        public ActionResult Filter(int? id, int factid = 1)
        {
            return View();
            var model = GetModel(factid);

            model.SchedulingViewModel = GetSchadulingReportModel(id);
            if (Config.IsAdOpsAdminInAdminApp || Config.IsAppOpsAdminInAdminApp)
                ViewBag.SchadulingReportAllowed = true;
            else
                ViewBag.SchadulingReportAllowed = _accountService.checkAdPermissions(new ValueMessageWrapper<PortalPermissionsCode> { Value = PortalPermissionsCode.ReportSchedule }).Value || Config.IsAppOps;

            if (model.SchedulingViewModel.ReportSchedulerDto.ReportDto.fact > 0)
            {
                model.FactId = model.SchedulingViewModel.ReportSchedulerDto.ReportDto.fact;
                model.IncludeId = model.SchedulingViewModel.ReportSchedulerDto.ReportDto.IncludeId;

            }
            return View(model);
        }
        public string Validate(DataModel data)
        {
            StringBuilder Massages = new StringBuilder();
            int DimensionsMaxSelect = Convert.ToInt32(JsonConfigurationManager.AppSettings["DimensionsMaxSelect"]);

            if (FixTreeIds(data.ColumnsIds, "1").Count() > DimensionsMaxSelect)
            {
                Massages.Append(string.Format("<div>you can\'t select more than {0} Dimensions</div >", DimensionsMaxSelect));

            }

            return Massages.ToString();
        }
        private List<int> FixTreeIds(List<int> ids, string type)
        {

            List<int> FixedIds = new List<int>();
            if (ids != null)
            {

                switch (type)
                {
                    case "1":
                        var CRoot = this._reportService.GetColumnByName("root");
                        foreach (var id in ids)
                        {
                            var node = this._reportService.GetColumnById(new ValueMessageWrapper<int> { Value = id });
                            int count = this._reportService.GetColumnCount(new ValueMessageWrapper<int> { Value = node.Id }).Value;

                            if (node.ParentId > 0 && (node.ParentId != CRoot.Id || count == 0) && count == 0)
                            {
                                FixedIds.Add(id);
                            }
                        }

                        break;
                    case "2":
                        var MRoot = this._reportService.GetMeasureByName("Root");

                        foreach (var id in ids)
                        {
                            var node = this._reportService.GetMeasureById(new ValueMessageWrapper<int> { Value = id });
                            int childerCount = this._reportService.GetMeasureCount(new ValueMessageWrapper<int> { Value = node.Id }).Value;
                            if (node.ParentId > 0 && (node.ParentId != MRoot.Id || childerCount == 0) && childerCount == 0)
                            {
                                FixedIds.Add(id);
                            }
                        }
                        break;
                    default:
                        break;

                }
            }
            return FixedIds;

        }

        private void FixData(DataModel data)
        {
            data.Querydata = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.QueryJsonData);
            data.ColumnsIds = string.IsNullOrEmpty(data.ColumnsIdsString) ? new List<int>()
                : data.ColumnsIdsString.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList();
            data.MeasuresIds = string.IsNullOrEmpty(data.MeasuresIdsString) ? new List<int>()
                : data.MeasuresIdsString.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList();
            function = data.function;
            selectedFactId = data.fact;
        }
        public void FilterResult(DataModel data)
        {

            ResultDataModelDto resultModel = new ResultDataModelDto();
            DataModelDto filterModel = new DataModelDto();
            filterModel.ColumnsIds = data.ColumnsIds;
            filterModel.ColumnsIdsString = data.ColumnsIdsString;
            filterModel.fact = data.fact;
            filterModel.ForPublisher = data.ForPublisher;
            filterModel.from = data.from;

            filterModel.MeasuresIds = data.MeasuresIds;
            filterModel.MeasuresIdsString = data.MeasuresIdsString;
            filterModel.pageSize = data.size;
            filterModel.pageNumber = data.page;
            filterModel.Querydata = data.Querydata;
            filterModel.QueryJsonData = data.QueryJsonData;
            filterModel.to = data.to;
            filterModel.IncludeId = data.IncludeId;
            filterModel.SummaryBy = data.SummaryBy;
            filterModel.accountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;


            resultModel = this._reportService.FilterResult(filterModel);
            warnings = new StringBuilder(resultModel.Warnings);

            if (!string.IsNullOrEmpty(resultModel.Query) && data.function != Function.Query)
            {
                Execute(resultModel, filterModel.pageNumber, filterModel.pageSize);
            }

        }

        public ActionResult GetQuery([FromBody] DataModel data)
        {
            FixData(data);
            FilterResult(data);
            return Json(new { Query = Query, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" });

        }


        public void Execute(ResultDataModelDto datamOdelDto, int pageno, int pagesize)
        {
            try
            {


                if (function == Function.CSV)
                {
                    var results = this._reportService.Execute(new ExecuteQueryRequest { Query = datamOdelDto.Query, DataModelDto = datamOdelDto });


                    foreach (var row in results)
                    {

                        PreparePacket(row);
                    }

                }
                else
                {
                    dataSet = this._reportService.ExecuteWithPagination(new ExecuteWithPaginationRequest { CountQuery = datamOdelDto.CountQuery, Query = datamOdelDto.Query, PageNumber = pageno, DataModelDto = datamOdelDto, PageSize = pagesize });

                }
            }
            catch (Exception e)
            {

                throw e;

            }

        }
        public void PreparePacket(string row)
        {
            if (function == Function.CSV)
            {
                PrepareCSVPacket(row);
            }
            else
            {
                BuildTable(row);
            }
        }
        public void BuildTable(string row)
        {
            if (string.IsNullOrEmpty(tableBody.ToString()))
            {
                tableBody.Append("<tr>");
                foreach (string item in row.ToString().Split('^'))
                {
                    tableBody.Append("<th>" + item + "</th>");
                }
                tableBody.Append("</tr>");
            }
            else
            {
                tableBody.Append("<tr>");
                foreach (string item in row.ToString().Split('^'))
                {
                    tableBody.Append("<td>" + item + "</td>");
                }
                tableBody.Append("</tr>");
            }
        }

        /*

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult CampaignReport(FormCollection collection, string fromdate, string toDate, string summaryBy, string layout, string criteriaOpt, string AdsList, string advancedCriteria, string deviceCategory, string tabId, string page, string orderBy, string groupByName, string AdvertiserId, string AccountAdvertiserId)
        {
            List<CampaignCommonReportDto> reportingList;
            int counter;
            bool result = false;
            try
            {

                result = GetCampaignReportData(collection, fromdate, toDate, summaryBy, layout, criteriaOpt, AdsList, advancedCriteria, deviceCategory, tabId, page, 10, orderBy, groupByName, AdvertiserId, AccountAdvertiserId, out reportingList, out counter);

                dynamic expando = new ExpandoObject();

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ResourcesUtilities.GetResource("Exception", "Global"));
            }

            if (result)
            {
                return View(new GridModel
                {
                    Data = reportingList,
                    Total = counter
                });
            }
            else
            {
                Response.StatusCode = 500;
                return Content(ResourcesUtilities.GetResource("FromDateandToDate", "Errors"));
            }
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
                public ActionResult QBGrid(DataModel data)
        {
            GridReportModel Model = new GridReportModel();
            FixData(data);
            string Massage = Validate(data);
            var ColumnsData = _ReportService.GetmetriceColumnsForAdvertiser();
          
            Model.GridColumnSettings = new List<GridColumnSettings>();

            table = "<table style=\"max-width: 10000px;\" id='resultTable'>{0}</table>";
            if (string.IsNullOrEmpty(Massage))
            {
                FilterResult(data);
                table = string.Format(table, tableBody);
                return Json(new { finalTable = table, Massage = Massage, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" });
            }

            foreach (var ColumnData in ColumnsData)
            {

                Model.GridColumnSettings.Add(new GridColumnSettings
                {
                    Member = ColumnData.AppFieldName,
                    Title = ResourcesUtilities.GetResource(ColumnData.HeaderResourceKey, ColumnData.HeaderResourceSet),
                    Format = ColumnData.Format
                });
            }

            return PartialView(Model);
        }
             
             */
        public ActionResult GetTable([FromBody] DataModel data)
        {
            FixData(data);
            string Massage = Validate(data);
            tableBody = new StringBuilder();
            table = "<table style=\"max-width: 10000px;\" id='resultTable'>{0}</table>";
            if (string.IsNullOrEmpty(Massage))
            {
                FilterResult(data);
                table = string.Format(table, tableBody);
                return Json(new { finalTable = table, Massage = Massage, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" });
            }
            return Json(new { finalTable = "", Massage = Massage, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" });
        }
        public ActionResult GetPaginationGrid([FromBody] DataModel data)
        {
            FixData(data);
            string Massage = Validate(data);
            tableBody = new StringBuilder();
            table = "<table style=\"max-width: 10000px;\" id='resultTable'>{0}</table>";
            if (string.IsNullOrEmpty(Massage))
            {
                pagenumber = data.pageNumber;

                try
                {
                    FilterResult(data);
                    if (dataSet.Rows == null)
                    {

                        dataSet.Rows = new List<ValueMessageWrapper<IDictionary<string, string>>>();
                    }
                    table = string.Format(table, tableBody);
                    return Json(new { data = dataSet, finalTable = table, Massage = Massage, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" });
                }
                catch (Exception ex)
                {
                    return Json(new { finalTable = "", Massage = ResourcesUtilities.GetResource("ErrorMsg", "ReportBuilder"), warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" });
                }

            }
            return Json(new { finalTable = "", Massage = Massage, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" });
        }


        [HttpPost]
        public void DownloadCSV([FromForm] DataModel data)
        {
            try
            {
                FixData(data);
                // string massage = Validate(data);
                // if (string.IsNullOrEmpty(massage))
                // {
                string filename = "Result_" + ArabyAds.Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMddHHmmss") + ".csv";
                Response.Clear();
                Response.Headers.Clear();


                // Response.Headers.e = Encoding.UTF8;
                Response.AddHeader("Pragma", "public");
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AddHeader("Connection-Timeout", "1000");
                // Response.AppendHeader("ETag", "bo\"" + _EncodedData + "\"");
                Response.ContentType = "application/octet-stream; charset=utf-8";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                //Response.AddHeader("Content-Length", (bytes.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.AddHeader("Keep-Alive", "timeout=1000, max=1000");

                //Response.ContentType = Encoding.UTF8.ToString();
                // Start the feed with BOM
                Response.Body.Write(Encoding.UTF8.GetPreamble());
                FilterResult(data);
                //}
            }
            catch (Exception e)
            {
                throw;

            }
            finally
            {
                Response.Flush();
            }
            // return Content("");
        }

        public void PrepareCSVPacket(string row)
        {
            List<string> cleanRow = new List<string>();
            foreach (string cell in row.ToString().Split('^'))
            {
                string temp = cell;
                if (cell.Contains("\""))
                {
                    temp = cell.Replace("\"", "\"\"");
                }
                if (cell.Contains(","))
                {
                    temp = string.Format("\"{0}\"", cell);
                }
                if (cell.Contains(System.Environment.NewLine))
                {
                    temp = string.Format("\"{0}\"", cell);
                }
                cleanRow.Add(temp);
            }
            row = string.Join(",", cleanRow);
            row = row + System.Environment.NewLine;
            Download(row.ToString());
        }
        public void Download(string packet)
        {

            byte[] bytes = Encoding.UTF8.GetBytes(packet);
            //Stream stream = new StreamReader(path).BaseStream;
            using (BinaryReader sr = new BinaryReader(new MemoryStream(bytes)))
            {
                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((bytes.Length + 0.0) / 1024);
                //Download in block of 1024 bytes
                for (int i = 0; i < maxCount && !HttpContext.RequestAborted.IsCancellationRequested; i++)
                {
                    Response.Body.Write(sr.ReadBytes(1024));
                    Response.Flush();
                }
            }


        }

        private CampaignReportSchedulingViewModel GetCampaignSchadulingReportModel(int? id, string reportType = "ad")
        {

            CampaignReportSchedulingViewModel model = new CampaignReportSchedulingViewModel();
            var RecurrenceTypeSelection = RecurrenceType.Month;
            int MonthSelection = 1, DaysSelection = 1;
            IList<int> ColumnsToBeSelected = new List<int>();

            if (id.HasValue && id > 0)
            {
                model.ReportSchedulerDto = _ReportService.GetSchadulingReport(new ValueMessageWrapper<int> { Value = (int)id });
                model.RecipientEmail = model.ReportSchedulerDto.AllReportRecipient.Select(x => x.Email).ToList();
                RecurrenceTypeSelection = model.ReportSchedulerDto.RecurrenceType;
                MonthSelection = model.ReportSchedulerDto.MonthDay;
                //model.ReportSchedulerDto.ReportDto.SummaryBy=
                model.ReportSchedulerDto.Status = (model.ReportSchedulerDto.EndDate != null && model.ReportSchedulerDto.EndDate < Framework.Utilities.Environment.GetServerTime()) || model.ReportSchedulerDto.NextFireTime == null ? ResourcesUtilities.GetResource("Active", "JobGrid") : ResourcesUtilities.GetResource("NotActive", "JobGrid");
                model.ReportSchedulerDto.ReportDto.FromDateString = model.ReportSchedulerDto.ReportDto.FromDate.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = model.ReportSchedulerDto.ReportDto.ToDate.ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                DaysSelection = (int)model.ReportSchedulerDto.WeekDay;
            }
            else
            {
                model.ReportSchedulerDto = new ReportSchedulerDto();
                model.RecipientEmail = new List<string>();
                model.ReportSchedulerDto.ReportDto = new ReportCriteriaDto();
                model.ReportSchedulerDto.Name = ResourcesUtilities.GetResource("ScheduleNameDefault", "Report");
                model.ReportSchedulerDto.ReportDto.FromDate = Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.ReportDto.ToDate = Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.DateRecurrenceType = DateRecurrenceType.Today;
                model.ReportSchedulerDto.ReportDto.FromDateString = Framework.Utilities.Environment.GetServerTime().ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = Framework.Utilities.Environment.GetServerTime().ToString(ArabyAds.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.TabId = reportType == "ad" ? "campaign" : "App";
                model.ReportSchedulerDto.ReportDto.SummaryBy = 1;
                model.ReportSchedulerDto.ReportDto.CriteriaOpt = "all";
                model.ReportSchedulerDto.ReportDto.Layout = "summary";
                model.ReportSchedulerDto.IsSunday = true;
                model.ReportSchedulerDto.EmailIntroduction = "";
                model.ReportSchedulerDto.IsActive = true;
                model.ReportSchedulerDto.ReportDto.GroupByName = false;
                model.ReportSchedulerDto.ReportDto.DeviceCategory = "platform";
                model.ReportSchedulerDto.StartDate = ArabyAds.Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.EmailSubject = ResourcesUtilities.GetResource("DefaultSubject", "Report");
            }

            model.Time = new List<SelectListItem> {
                 new SelectListItem{Text = ResourcesUtilities.GetResource("Monthly", "Time") ,Value = ((int)RecurrenceType.Month).ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Month }
                 ,new SelectListItem {Text =ResourcesUtilities.GetResource("Weekly", "Time") ,Value=((int)RecurrenceType.Week).ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Week}
                 , new SelectListItem{Text =ResourcesUtilities.GetResource("Daily", "Time"),Value=((int)RecurrenceType.Day).ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Day}
                };

            return model;
        }

    }
}











