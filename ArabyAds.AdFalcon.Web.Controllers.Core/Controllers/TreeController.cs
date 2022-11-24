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
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework.ConfigurationSetting;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider }, Roles = "AppOps", AuthorizeRoles = "Administrator,AccountManager,AdOps", DenyImpersonationOnly = true)]
    [RequireHttps(Order = 1)]
  
    public class TreeController : ControllerBase
    {
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
        protected string colorGray = "color:#969696;";
        protected IPartyService _partyService;
        private static IConfigurationManager configurationManager = null;
        public TreeController()
        {
            _lookupService = IoC.Instance.Resolve<ILookupService>();
            _audienceSegmentService = IoC.Instance.Resolve<IAudienceSegmentService>(); ;
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

        public static IConfigurationManager ConfigurationManager
        {
            get
            {
                return configurationManager ?? (configurationManager = Framework.IoC.Instance.Resolve<IConfigurationManager>());
            }
        }

        [OutputCache(Duration = 21600, VaryByQueryKeys = new string[] { "type", "factId", "IncludeId",  "reportType" })]

        public JsonResult Get( string type, int factId,bool IncludeId, string reportType = "ad")
        {
            string q = null;
            string DashBoardType = null;
            bool forPublisher = reportType == "app";
            List<TreeQBDto> records = new List<TreeQBDto>();
            if (DashBoardType=="campaign")
            {
                DashBoardType = "ad";


            }

            if (DashBoardType == "appsite")
            {
                DashBoardType = "app";


            }
            switch (type)
            {
                case "1":
                    records = GetColumns(q, type, factId, IncludeId, forPublisher);
                    if(!string.IsNullOrWhiteSpace(DashBoardType))
                    {
                        List<int> ColumnsIds = null;
                           string allowedColumns =  ConfigurationManager.GetConfigurationSetting(null, null, DashBoardType.ToLower() + "Columns");
                      if(!string.IsNullOrEmpty(allowedColumns))
                         ColumnsIds =  allowedColumns.Split(',').Select(int.Parse).ToList();
                        IList<TreeQBDto> newRecords = new List<TreeQBDto>();
                        if (ColumnsIds!=null && ColumnsIds.Count>0 )
                        {
                            foreach (var record in records)
                            {
                                if (ColumnsIds.Contains(record.Id))
                                {
                                    newRecords.Add(record);


                                }
                            }
                        }

                       records = newRecords.ToList();
                    }

                    break;
                case "2":
                    records = GetMeasures(q, type, factId, forPublisher);

                    if (!string.IsNullOrWhiteSpace(DashBoardType))
                    {
                      string allowedMeasures=  ConfigurationManager.GetConfigurationSetting(null, null, DashBoardType.ToLower() + "Measures");

                        List<int> MeasuresIds = null;
  
                        if (!string.IsNullOrEmpty(allowedMeasures))
                            MeasuresIds = allowedMeasures.Split(',').Select(int.Parse).ToList();

                        IList<TreeQBDto> newRecords = new List<TreeQBDto>();
                        if (MeasuresIds != null && MeasuresIds.Count > 0)
                        {
                            foreach (var record in records)
                            {
                                if (MeasuresIds.Contains(record.Id))
                                {
                                    newRecords.Add(record);


                                }
                            }
                        }

                        records = newRecords.ToList();

                    }
                    break;
                default:
                    break;

            }

           var trees= TreeModel.GetTreeQBNodes(records);
            var result = new JsonResult (  trees);
            return result;
           // return Json(records);

        }
        private List<TreeQBDto> GetColumns(string q, string type, int factId, bool IncludeId, bool forPublisher = false)
        {
            List<ColumnQBDto> columns = this._reportService.GetColumnsByFactId( new GetColumnsByFactIdRequest {  Id= factId, IncludeId=   IncludeId } ).ToList();
            if (forPublisher)
            {
                columns = columns.Where(x => x.SupportedByPublisher).ToList();
            }
            else
            {
                columns = columns.Where(x => x.SupportedByAdvertiser).ToList();


            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                columns = columns.Where(z => z.Name.Contains(q)).ToList();
            }
            ColumnQBDto Root = this._reportService.GetColumnByName("root"); 

            return columns.Where(l => l.ParentId == Root.Id).OrderBy(l => l.OrderNumber)
                .Select(l => new TreeQBDto
                {
                    Id = l.Id,
                    id = l.Id.ToString(),
                    text = l.DisplayName,
                    data =l.DisplayName,
                    Key = "columns",
                    style = colorGray ,
                    Childs = GetChildren(columns, l.Id, "columns")
                }).ToList();
        }

        private List<TreeQBDto> GetMeasures(string q, string type, int factId, bool forPublisher = false)
        {
            List<MeasureDto> measures = this._reportService.GetMeasuresByFactId(new ValueMessageWrapper<int> { Value = factId });

            if (forPublisher)
            {
                measures = measures.Where(x => x.SupportedByPublisher).ToList();
            }
            else
            {
                measures = measures.Where(x => x.SupportedByAdvertiser).ToList();
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                measures = measures.Where(z => z.Name.Contains(q)).ToList();
            }

            measures = measures.OrderBy(o => o.Name).ToList();

            MeasureDto Root = this._reportService.GetMeasureByName("Root");
            return measures.Where(l => l.ParentId == Root.Id).OrderBy(l => l.OrderNumber)
                .Select(l => new TreeQBDto
                {
                    Id = l.Id,
                    id = l.Id.ToString(),
                   
                    text = l.Name,
                    style = colorGray ,
                    data = l.Name,
                    Key = "measures",
                    Childs = GetChildren(measures, l.Id, "measures")
                }).ToList();

        }

        private List<TreeQBDto> GetChildren<T>(List<T> List, int parentId, string key) where T : TreeQBDto
        {
            var test = List.Where(l => l.ParentId == parentId).OrderBy(l => l.OrderNumber)
                .Select(l => new TreeQBDto
                {
                    Id = l.Id,
                    id = l.Id.ToString(),
                    text = l.Name,
                    data = l.Name,
                    Key =key,
                    style = List.Where(x=>x.ParentId == l.Id).Count()>0   ? colorGray : "",
                    Childs = GetChildren(List, l.Id,key)
                }).ToList();

            return test;
        }



        //public JsonResult LazyGet(int? parentId)
        //{
        //    List<Column> columns;
        //    List<ColumnDto> records;

        //    columns = _colRepository.GetAll().ToList();

        //    records = columns.Where(l => l.ParentId == parentId).OrderBy(l => l.OrderNumber)
        //            .Select(l => new ColumnDto
        //            {
        //                id = l.Id,
        //                text = l.Attribute,
        //                hasChildren = columns.Any(l2 => l2.ParentId == l.Id)
        //            }).ToList();


        //    return this.Json(records);
        //}


        //public JsonResult GetColumns(string query)
        //{
        //    List<ColumnDto> records;

        //    records = _colRepository.Query(l => !l.ParentId.HasValue)
        //        .Select(l => new ColumnDto
        //        {
        //            id = l.Id,
        //            text = l.Name,

        //        }).ToList();


        //    return this.Json(records);
        //}

        //[HttpPost]
        //public JsonResult SaveCheckedNodes(List<int> checkedIds)
        //{
        //    if (checkedIds == null)
        //    {
        //        checkedIds = new List<int>();
        //    }
        //    using (ApplicationDbContext context = new ApplicationDbContext())
        //    {
        //        var locations = context.Locations.ToList();
        //        foreach (var location in locations)
        //        {
        //            location.Checked = checkedIds.Contains(location.ID);
        //        }
        //        context.SaveChanges();
        //    }

        //    return this.Json(true);
        //}

        //[HttpPost]
        //public JsonResult ChangeNodePosition(int id, int parentId, int orderNumber)
        //{

        //        var location = context.Locations.First(l => l.ID == id);

        //        var newSiblingsBelow = context.Locations.Where(l => l.ParentID == parentId && l.OrderNumber >= orderNumber);
        //        foreach (var sibling in newSiblingsBelow)
        //        {
        //            sibling.OrderNumber = sibling.OrderNumber + 1;
        //        }

        //        var oldSiblingsBelow = context.Locations.Where(l => l.ParentID == location.ParentID && l.OrderNumber > location.OrderNumber);
        //        foreach (var sibling in oldSiblingsBelow)
        //        {
        //            sibling.OrderNumber = sibling.OrderNumber - 1;
        //        }


        //        location.ParentID = parentId;
        //        location.OrderNumber = orderNumber;

        //        context.SaveChanges();


        //    return this.Json(true);
        //}
    }
}










