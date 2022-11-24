using System;
using System.Collections;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Core.ViewComponents.Campaign
{

    public class AdRequests : ViewComponent
    {
        protected static ILookupService _lookupService;

        private static IAppSiteTypeService _appSiteTypeService;
        protected static IKeywordService _keywordService;
        protected static ICampaignService _campaignService;
        protected static IObjectiveTypeService _objectiveTypeService;
        protected static ICreativeUnitService _creativeUnitService;
        protected static IAgeGroupService _ageGroupService;
        protected static IAudienceSegmentService _audienceSegmentService;
        private static IAppSiteService _appSiteService;
        protected static IAdvertiserService _AdvertiserService;

        protected static ITileImageService _tileImageService;
        protected static IAdCreativeStatusService _adCreativeStatusService;
        protected static IDeviceCapabilityService _deviceCapabilityService;
        protected static IRichMediaRequiredProtocolService _richMediaRequiredProtocolService;
        protected static ILocationService _locationService;
        protected static IDeviceTypeService _deviceTypeService;
        protected static IPlatformService _platformService;
        protected static ICostModelWrapperService _CostModelWrapperService;
        protected static IVideoTypeService _videoTypeService;
        private static ITrackingEventService _trackingEventService;
        protected static IAccountService _accountService;
        protected static IVideoDeliveryMethodsService _videoDeliveryMethodsService;
        protected static IAppMarketingPartnerService _appMarketingPartnerServic;

        protected static ICreativeVendorService _creativeVendorService;
        protected static ILanguageService _languageService;
        protected static IUserService _userService;
        protected static IReportService _reportService;
        //  private readonly IWebHostEnvironment _hostingEnvironment;
        //protected readonly JsonSerializerOptions _jsonOptions;
        protected static IAdCreativeAttributeService adCreativeAttributeService;
        protected static IPartyService _partyService;

        static AdRequests()
        {


            adCreativeAttributeService = IoC.Instance.Resolve<IAdCreativeAttributeService>();
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
            _appSiteService = IoC.Instance.Resolve<IAppSiteService>();
            _AdvertiserService = IoC.Instance.Resolve<IAdvertiserService>();
            _userService = IoC.Instance.Resolve<IUserService>();
            _creativeVendorService = IoC.Instance.Resolve<ICreativeVendorService>();
            _languageService = IoC.Instance.Resolve<ILanguageService>();
            _reportService = IoC.Instance.Resolve<IReportService>();
            _partyService = IoC.Instance.Resolve<IPartyService>();

        }
        public AdRequests()
        {
        }

        private  ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Campaign.AdRequestViewModel getAdrequestViewModel(int adGroupId)
        {
            //var AllItems = GetAdRequestQueryResult(null, adGroupId);
            var Types_Platforms_Versions = _campaignService.GetAdRequestTypes_Platforms_Versions();
            IList<SelectListItem> Types = new List<SelectListItem>();
            bool SelectedVar = true;
            var typeId = 0;
            foreach (AdRequestTypeDto Type in Types_Platforms_Versions.Types)
            {
                Types.Add(new SelectListItem { Value = Type.ID.ToString(), Text = Type.Name, Selected = SelectedVar });
                if (SelectedVar == true)
                {

                    typeId = Type.ID;
                }
                SelectedVar = false;

            }

            IList<SelectListItem> Platforms = new List<SelectListItem>();

            IList<SelectListItem> Versions = new List<SelectListItem>();
            var filteredByTypes = Types_Platforms_Versions.All.Where(M => M.AdRequestType.ID == typeId).ToList();
            SelectedVar = true;
            var SelectedVar2 = true;
            int PlatformId = 0;
            foreach (var Item in filteredByTypes)
            {
                if (Platforms.Where(M => M.Value == Item.AdRequestPlatform.ID.ToString()).SingleOrDefault() == null & Item.AdRequestType.ID == typeId)
                {
                    Platforms.Add(new SelectListItem { Value = Item.AdRequestPlatform.ID.ToString(), Text = Item.AdRequestPlatform.Name, Selected = SelectedVar });
                    PlatformId = Item.AdRequestPlatform.ID;
                    SelectedVar = false;
                }

                if (PlatformId > 0 & Versions.Where(M => M.Value == Item.Version).SingleOrDefault() == null & Item.AdRequestPlatform.ID == PlatformId)
                {
                    Versions.Add(new SelectListItem { Value = Item.Version, Text = Item.Version, Selected = SelectedVar2 });
                    SelectedVar2 = false;
                }
            }


            return new ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Campaign.AdRequestViewModel
            {
                adGroupId = adGroupId,
                AdRequestDialog = new ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Campaign.AdRequestDialogViewModel
                {
                    Types = Types,
                    Platforms = Platforms,
                    Versions = Versions
                },
              
            };
        }

        public async Task<IViewComponentResult> InvokeAsync(
       int adGroupId)
        {

            ArabyAds.AdFalcon.Web.Controllers.Core.Utilities.Security.AuthorizeRole(new string[] { "Administrator", "AccountManager", "AdOps" });

            return View("AdRequests", getAdrequestViewModel(adGroupId));
        }

    }
}
