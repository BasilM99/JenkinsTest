using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.Script.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Web.Controllers.Model.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using Noqoush.Framework;
using Noqoush.Framework.Utilities;
using Telerik.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Telerik.Web.Mvc.Extensions;
using Action = Noqoush.AdFalcon.Web.Controllers.Model.Action;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using System.Globalization;

using System.Text.RegularExpressions;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Handler;
using Telerik.Web.Mvc.UI;
using System.Web;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using Noqoush.AdFalcon.Web.Controllers.Model.User;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal;
using Noqoush.AdFalcon.Domain.Common.Repositories;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Newtonsoft.Json;
using Noqoush.AdFalcon.Web.Controllers.Model.Advertiser;
using Noqoush.AdFalcon.Domain.Common.Repositories.Account;
using Noqoush.AdFalcon.Exceptions;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite.Performance;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;


using Noqoush.AdFalcon.Web.Controllers.Model.QueryBuilder;
using Noqoush.AdFalcon.Domain.Common.Model.QueryBuilder;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QB;
using System.Dynamic;
using Noqoush.AdFalcon.Web.Controllers.Model.Report;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider }, Roles = "AppOps", AuthorizeRoles = "Administrator,AccountManager,AdOps", DenyImpersonationOnly = true)]
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
        private  int selectedFactId;
        private  Function function;
        private  string Query = "";
        private string warningDiv = "<div><strong>{0}</strong> were excluded, invalid input</div>";
        private  string limit = " limit 100 ;";
        private  StringBuilder warnings = new StringBuilder();
        private  string table;
        private int pagenumber = 0;
        private  StringBuilder tableBody = new StringBuilder();
        public FilterController(
            IAudienceSegmentService AudienceSegmentService,
            IAccountService accountService,
        ICampaignService campaignService,
                                IKeywordService keywordService,
                                IObjectiveTypeService objectiveTypeService,
                                ICreativeUnitService creativeUnitService,
                                IAgeGroupService ageGroupService,
                                ITileImageService tileImageService,
                                IAdCreativeStatusService adCreativeStatusService,
                                IDeviceCapabilityService deviceCapabilityService,
                                IRichMediaRequiredProtocolService richMediaRequiredProtocolService,
                                ILocationService locationService,
                                IDeviceTypeService deviceTypeService,
                                ITrackingEventService trackingEventService,
                                IPlatformService platformService,
                                ICostModelWrapperService costModelWrapperService,
                               IVideoTypeService videoTypeService,

           IVideoDeliveryMethodsService videoDeliveryMethodsService, IAppSiteTypeService appSiteTypeService, IAppMarketingPartnerService appMarketingPartnerService, IUserService userService, IAppSiteService appSiteService, IAdvertiserService AdvertiserService

, ICreativeVendorService creativeVendorService, ILanguageService LanguageService, IReportService reportService
            , IPartyService partService, ILookupService lookupService
)
        {
            _lookupService = lookupService;
            _audienceSegmentService = AudienceSegmentService;
            _keywordService = keywordService;
            _campaignService = campaignService;
            _objectiveTypeService = objectiveTypeService;
            _creativeUnitService = creativeUnitService;
            _ageGroupService = ageGroupService;
            _tileImageService = tileImageService;
            _adCreativeStatusService = adCreativeStatusService;
            _deviceCapabilityService = deviceCapabilityService;
            _richMediaRequiredProtocolService = richMediaRequiredProtocolService;
            _locationService = locationService;
            _deviceTypeService = deviceTypeService;
            _trackingEventService = trackingEventService;
            _platformService = platformService;
            _CostModelWrapperService = costModelWrapperService;
            _videoTypeService = videoTypeService;
            _videoDeliveryMethodsService = videoDeliveryMethodsService;
            _appSiteTypeService = appSiteTypeService;
            _appMarketingPartnerServic = appMarketingPartnerService;
            _accountService = accountService;
            this._appSiteService = appSiteService;
            _AdvertiserService = AdvertiserService;
            _userService = userService;
            _creativeVendorService = creativeVendorService;
            _languageService = LanguageService;
            _reportService = reportService;
            this._partyService = partService;
        }


        private bool checkAdPermissions(PortalPermissionsCode Code)
        {

            bool result = _accountService.checkAdPermissions(Code);

            return result;
        }

        //Fill Dims Types
        private FilterViewModel GetModel(int factid)
        {
            FilterViewModel model = new FilterViewModel();

            #region dropDwonlists
            model.Facts = new List<SelectListItem>();
            model.Dimensions = new List<SelectListItem>();
            model.Dimensions.Add(new SelectListItem { Value = "-1", Text = "Please Select", Selected = true });


            List<FactDto> Facts = this._reportService.GetAllFacts().ToList();
            if (Config.IsAdministrationApp)
            {
                model.Facts.AddRange(Facts.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.DisplayName }).OrderBy(x => x.Value).ToList());
            }
            else
            {
                model.Facts.AddRange(Facts.Where(x=>x.IsForWeb==true).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.WebDisplayName }).OrderBy(x => x.Value).ToList());


            }
            if (Domain.Configuration.IsAdminOrAdOps)
            {
                model.Dimensions.AddRange(Facts.Where(x => x.Id == factid).SingleOrDefault().Dimensions.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).OrderBy(x => x.Text).ToList());
            }
            else
            {
                model.Dimensions.AddRange(Facts.Where(x => x.Id == factid).SingleOrDefault().Dimensions.Where(M=>M.Id!= 25).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).OrderBy(x => x.Text).ToList());

            }
            selectedFactId = factid;
            #endregion
            DateTime today = Noqoush.Framework.Utilities.Environment.GetServerTime();
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

            model.DimensionsMaxSelect = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DimensionsMaxSelect"]);

            model.SchedulingViewModel = GetCampaignSchadulingReportModel(0);

            return model;

        }
        public ActionResult SubFilter(int id, int factid = 1)
        {
            var model = GetModel(factid);
            model.Id = ++id;
            return PartialView(model);
        }
        public ActionResult MultiSelectQB(int id, string selectedId, string ListOfIds)
        {
         
            Model.QueryBuilder.Select2ViewModel model = new Model.QueryBuilder.Select2ViewModel
            {
                Code = id.ToString(),
                ActionName = "GetSelect2Elements",
                Id = selectedId,
                ListOfIds= ListOfIds
            };
            return PartialView(model);
        }
        //fill dim values 
        public ActionResult GetSelect2Elements(string id, string q,string t,  int selectedFactIdVar , int page, string Ids)
        {
            List<Select2> list = new List<Select2>();
            string subId = string.Empty;
            int dimID = Convert.ToInt32(id);
            DimensionDto dim = this._reportService.GetDimensionById(dimID);
            FactDto fact = this._reportService.GetFactById(selectedFactIdVar);
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
               int lastindexSimi=  t.LastIndexOf(":");
              firstPart =  t.Substring(0, lastindexSimi);
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

            if (q==null)
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

                        script= script +" AND " + idsSting;
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

                    if (!string.IsNullOrEmpty(Ids))
                    {

                        script = script + " AND " + idsSting;
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


                    script = script.Replace("search", "  " );
                }

                List<DataQBDto> Data = new List<DataQBDto>();

         
                    if(!dim.IsScoped)
                      Data = this._reportService.GetResultofDataQBDto(script,string.Empty, dim.TableName, page);
                    else
                    Data = this._reportService.GetResultofDataQBDtoWithScoping(script, string.Empty, dim.ScopeTableName, page);


                if (Data != null && Data.Count > 0 && !string.IsNullOrEmpty(Data.First().ParentName)&& string.IsNullOrEmpty(Ids))
                {
                    if (!string.IsNullOrEmpty(Data.First().SuperParentName))
                    {
                        var DataSuperParentName = Data.GroupBy(user => user.SuperParentName);
                        foreach (var groupRoot in DataSuperParentName)
                        {
                            var DataParentName = groupRoot.GroupBy(m => m.ParentName);
                            Select2 rootvar = new Select2 { text = groupRoot.Key ,TotalCount= Data.First().TotalCount };
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
                if (TagIds.Count==0)
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

            return Json(list, JsonRequestBehavior.AllowGet);
        }


       

        public ActionResult SaveReportSchedulingReport(CampaignReportSchedulingSaveModel CampaignReportScheduling)
        {
            string message = "";
            bool result = false;
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
                        ColumnsIds = string.IsNullOrEmpty(CampaignReportScheduling.ColumnsIdsString) ? new List<int>():
                        CampaignReportScheduling.ColumnsIdsString.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList(),
                         MeasuresIds = string.IsNullOrEmpty(CampaignReportScheduling.MeasuresIdsString) ? new List<int>(): CampaignReportScheduling.MeasuresIdsString.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList(),
                        fact = CampaignReportScheduling.fact,
                        IncludeId = CampaignReportScheduling.IncludeId,
                        ToDate = new DateTime(CampaignReportScheduling.ToDate.Year, CampaignReportScheduling.ToDate.Month, CampaignReportScheduling.ToDate.Day, 0, 0, 0),
                        FromDate = new DateTime(CampaignReportScheduling.FromDate.Year, CampaignReportScheduling.FromDate.Month, CampaignReportScheduling.FromDate.Day, 23, 59, 0),
                        AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value
                    }
                };
                reportSchedulerDto.IsForQueryBuilder = true;
                id = _reportService.SaveSchadulingReport(reportSchedulerDto);
                message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), string.Empty);
                result = true;
            }
            catch (Exception ex)
            {
                message = ResourcesUtilities.GetResource("Exception", "Global");
            }

            return Json(new { Result = result, Message = message, id = id });
        }

        private CampaignReportSchedulingViewModel GetSchadulingReportModel(int? id, string reportType = "ad")
        {

            CampaignReportSchedulingViewModel model = new CampaignReportSchedulingViewModel();
            var RecurrenceTypeSelection = RecurrenceType.Month;
            int MonthSelection = 1, DaysSelection = 1;
            IList<int> ColumnsToBeSelected = new List<int>();

            if (id.HasValue && id > 0)
            {
                model.ReportSchedulerDto = _reportService.GetSchadulingReport((int)id);
                model.RecipientEmail = model.ReportSchedulerDto.AllReportRecipient.Select(x => x.Email).ToList();
                RecurrenceTypeSelection = model.ReportSchedulerDto.RecurrenceType;
                MonthSelection = model.ReportSchedulerDto.MonthDay;
                model.ReportSchedulerDto.Status = (model.ReportSchedulerDto.EndDate != null && model.ReportSchedulerDto.EndDate < Framework.Utilities.Environment.GetServerTime()) || model.ReportSchedulerDto.NextFireTime == null ? ResourcesUtilities.GetResource("Active", "JobGrid") : ResourcesUtilities.GetResource("NotActive", "JobGrid");
                model.ReportSchedulerDto.ReportDto.FromDateString = model.ReportSchedulerDto.ReportDto.FromDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = model.ReportSchedulerDto.ReportDto.ToDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
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
                model.ReportSchedulerDto.ReportDto.FromDateString = Framework.Utilities.Environment.GetServerTime().ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = Framework.Utilities.Environment.GetServerTime().ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.TabId = reportType == "ad" ? "campaign" : "App";
                model.ReportSchedulerDto.ReportDto.SummaryBy = 4;
                model.ReportSchedulerDto.ReportDto.CriteriaOpt = "all";
                model.ReportSchedulerDto.ReportDto.Layout = "summary";
                model.ReportSchedulerDto.IsSunday = true;
                model.ReportSchedulerDto.EmailIntroduction = "";
                model.ReportSchedulerDto.IsActive = true;
                model.ReportSchedulerDto.ReportDto.GroupByName = false;
                model.ReportSchedulerDto.ReportDto.DeviceCategory = "platform";
                model.ReportSchedulerDto.StartDate = Noqoush.Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.EmailSubject = ResourcesUtilities.GetResource("DefaultSubject", "Report");
            }

            model.Time = new List<SelectListItem> {
                 new SelectListItem{Text = ResourcesUtilities.GetResource("Monthly", "Time") ,Value = RecurrenceType.Month.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Month },new SelectListItem {Text =ResourcesUtilities.GetResource("Weekly", "Time") ,Value=RecurrenceType.Week.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Week}, new SelectListItem{Text =ResourcesUtilities.GetResource("Daily", "Time"),Value=RecurrenceType.Day.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Day}
                };

            return model;
        }





        [PermissionsAuthorize(Permission = PortalPermissionsCode.QueryBuilder, Roles = "Administrator,adops,AccountManager")]

        [HttpGet]

        public ActionResult Filter(int? id, int factid = 1)
        {
            var model = GetModel(factid);

            model.SchedulingViewModel = GetSchadulingReportModel(id);
            if (Config.IsAdOpsAdminInAdminApp || Config.IsAppOpsAdminInAdminApp)
                ViewBag.SchadulingReportAllowed = true;
            else
                ViewBag.SchadulingReportAllowed = _accountService.checkAdPermissions(PortalPermissionsCode.ReportSchedule) || Config.IsAppOps;

         if (model.SchedulingViewModel.ReportSchedulerDto.ReportDto.fact>0)
            {
                model.FactId = model.SchedulingViewModel.ReportSchedulerDto.ReportDto.fact;
                model.IncludeId = model.SchedulingViewModel.ReportSchedulerDto.ReportDto.IncludeId;

            }
            return View(model);
        }
        public string Validate(DataModel data)
        {
            StringBuilder Massages = new StringBuilder();
            int DimensionsMaxSelect = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DimensionsMaxSelect"]);

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
                            var node = this._reportService.GetColumnById(id);
                            int count = this._reportService.GetColumnCount(node.Id);

                            if (node.ParentId>0 && (node.ParentId != CRoot.Id || count == 0) && count == 0)
                            {
                                FixedIds.Add(id);
                            }
                        }

                        break;
                    case "2":
                        var MRoot = this._reportService.GetMeasureByName("Root");

                        foreach (var id in ids)
                        {
                            var node = this._reportService.GetMeasureById(id);
                            int childerCount = this._reportService.GetMeasureCount(node.Id);
                            if (node.ParentId>0 && (node.ParentId != MRoot.Id || childerCount == 0) && childerCount == 0)
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
                : data.ColumnsIdsString.Split(',').Where(x=>!string.IsNullOrEmpty(x)).Select(x =>  Convert.ToInt32(x)).ToList();
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
            warnings= resultModel.Warnings;

            if (!string.IsNullOrEmpty(resultModel.Query)&& data.function!=Function.Query)
            {
                Execute(resultModel, filterModel.pageNumber, filterModel.pageSize);
            }

        }

        public ActionResult GetQuery(DataModel data)
        {
            FixData(data);
            FilterResult(data);
            return Json(new { Query = Query, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" }, JsonRequestBehavior.AllowGet);

        }
        

        public void Execute(ResultDataModelDto datamOdelDto,int pageno,int pagesize)
        {
            try
            {
              

                if (function == Function.CSV)
                {
                    var results = this._reportService.Execute(datamOdelDto.Query, datamOdelDto);


                    foreach (var row in results)
                    {

                        PreparePacket(row);
                    }
                }
                else
                {
                    dataSet = this._reportService.ExecuteWithPagination(datamOdelDto.Query, pageno, datamOdelDto, pagesize);

                }
            }
            catch (Exception e)
            {

                throw e;
                    
            }

        }
        public void PreparePacket(StringBuilder row)
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
        public void BuildTable(StringBuilder row)
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
                return Json(new { finalTable = table, Massage = Massage, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetTable(DataModel data)
        {
            FixData(data);
            string Massage = Validate(data);
            tableBody = new StringBuilder();
            table = "<table style=\"max-width: 10000px;\" id='resultTable'>{0}</table>";
            if (string.IsNullOrEmpty(Massage))
            {
                FilterResult(data);
                table = string.Format(table, tableBody);
                return Json(new { finalTable = table, Massage = Massage, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { finalTable = "", Massage = Massage, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPaginationGrid(DataModel data)
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
                    table = string.Format(table, tableBody);
                    return Json(new { data = dataSet, finalTable = table, Massage = Massage, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { finalTable = "", Massage = ResourcesUtilities.GetResource("ErrorMsg", "ReportBuilder")   , warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" }, JsonRequestBehavior.AllowGet);
                }
                
            }
            return Json(new { finalTable = "", Massage = Massage, warnings = !string.IsNullOrEmpty(warnings.ToString()) ? string.Format(warningDiv, warnings.ToString()) : "" }, JsonRequestBehavior.AllowGet);
        }



        public void DownloadCSV(DataModel data)
        {
            try
            {
                FixData(data);
                // string massage = Validate(data);
                // if (string.IsNullOrEmpty(massage))
                // {
                string filename = "Result_" + Noqoush.Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMddHHmmss") + ".csv";
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();

                Response.HeaderEncoding = Encoding.UTF8;
               Response.AddHeader("Pragma", "public");
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AddHeader("Connection-Timeout", "1000");
                // Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                //Response.AddHeader("Content-Length", (bytes.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.AddHeader("Keep-Alive", "timeout=1000, max=1000");

                Response.ContentEncoding = Encoding.UTF8;
                // Start the feed with BOM
               Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                FilterResult(data);
                //}
            }
            catch (Exception e)
            {
                throw;

            }
            finally
            {
                Response.End();
            }
        }

        public void PrepareCSVPacket(StringBuilder row)
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
            row = new StringBuilder(string.Join(",", cleanRow));
            row.AppendLine();
            Download(row.ToString());
        }
        public void Download(string packet)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(packet);
                //Stream stream = new StreamReader(path).BaseStream;
                using (BinaryReader sr = new BinaryReader(new MemoryStream(bytes)))
                {
                    //Dividing the data in 1024 bytes package
                    int maxCount = (int)Math.Ceiling((bytes.Length + 0.0) / 1024);
                    //Download in block of 1024 bytes
                    for (int i = 0; i < maxCount && Response.IsClientConnected; i++)
                    {
                        Response.BinaryWrite(sr.ReadBytes(1024));
                        Response.Flush();
                    }
                }
            }
            catch (Exception e)
            {

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
                model.ReportSchedulerDto = _ReportService.GetSchadulingReport((int)id);
                model.RecipientEmail = model.ReportSchedulerDto.AllReportRecipient.Select(x => x.Email).ToList();
                RecurrenceTypeSelection = model.ReportSchedulerDto.RecurrenceType;
                MonthSelection = model.ReportSchedulerDto.MonthDay;
                //model.ReportSchedulerDto.ReportDto.SummaryBy=
                model.ReportSchedulerDto.Status = (model.ReportSchedulerDto.EndDate != null && model.ReportSchedulerDto.EndDate < Framework.Utilities.Environment.GetServerTime()) || model.ReportSchedulerDto.NextFireTime == null ? ResourcesUtilities.GetResource("Active", "JobGrid") : ResourcesUtilities.GetResource("NotActive", "JobGrid");
                model.ReportSchedulerDto.ReportDto.FromDateString = model.ReportSchedulerDto.ReportDto.FromDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = model.ReportSchedulerDto.ReportDto.ToDate.ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
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
                model.ReportSchedulerDto.ReportDto.FromDateString = Framework.Utilities.Environment.GetServerTime().ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.ToDateString = Framework.Utilities.Environment.GetServerTime().ToString(Noqoush.AdFalcon.Web.Controllers.Utilities.Config.ShortDateFormat);
                model.ReportSchedulerDto.ReportDto.TabId = reportType == "ad" ? "campaign" : "App";
                model.ReportSchedulerDto.ReportDto.SummaryBy = 1;
                model.ReportSchedulerDto.ReportDto.CriteriaOpt = "all";
                model.ReportSchedulerDto.ReportDto.Layout = "summary";
                model.ReportSchedulerDto.IsSunday = true;
                model.ReportSchedulerDto.EmailIntroduction = "";
                model.ReportSchedulerDto.IsActive = true;
                model.ReportSchedulerDto.ReportDto.GroupByName = false;
                model.ReportSchedulerDto.ReportDto.DeviceCategory = "platform";
                model.ReportSchedulerDto.StartDate = Noqoush.Framework.Utilities.Environment.GetServerTime();
                model.ReportSchedulerDto.EmailSubject = ResourcesUtilities.GetResource("DefaultSubject", "Report");
            }

            model.Time = new List<SelectListItem> {
                 new SelectListItem{Text = ResourcesUtilities.GetResource("Monthly", "Time") ,Value = RecurrenceType.Month.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Month },new SelectListItem {Text =ResourcesUtilities.GetResource("Weekly", "Time") ,Value=RecurrenceType.Week.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Week}, new SelectListItem{Text =ResourcesUtilities.GetResource("Daily", "Time"),Value=RecurrenceType.Day.ToString(),Selected=RecurrenceTypeSelection ==RecurrenceType.Day}
                };

            return model;
        }

    }
}










