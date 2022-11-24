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

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider }, Roles = "AppOps", AuthorizeRoles = "Administrator,AccountManager,AdOps", DenyImpersonationOnly = true)]
    public class TreeController : AuthorizedControllerBase
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
        public TreeController(
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
        public JsonResult Get(string q, string type, int factId,bool IncludeId)
        {
            List<TreeQBDto> records = new List<TreeQBDto>();

            switch (type)
            {
                case "1":
                    records = GetColumns(q, type, factId, IncludeId);
                    break;
                case "2":
                    records = GetMeasures(q, type, factId);
                    break;
                default:
                    break;

            }

           var trees= TreeModel.GetTreeQBNodes(records);
            var result = new JsonResult { Data = trees, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
           // return Json(records, JsonRequestBehavior.AllowGet);

        }
        private List<TreeQBDto> GetColumns(string q, string type, int factId, bool IncludeId)
        {
            List<ColumnQBDto> columns = this._reportService.GetColumnsByFactId(factId, IncludeId).ToList();

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

        private List<TreeQBDto> GetMeasures(string q, string type, int factId)
        {
            List<MeasureDto> measures = this._reportService.GetMeasuresByFactId(factId);

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


        //    return this.Json(records, JsonRequestBehavior.AllowGet);
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


        //    return this.Json(records, JsonRequestBehavior.AllowGet);
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










