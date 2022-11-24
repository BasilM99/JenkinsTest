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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Noqoush.AdFalcon.Services.Interfaces.Messages;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Web.Controllers.Core.ViewComponents.Campaign
{

    public class CostElements : ViewComponent
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
        protected static  IAdCreativeStatusService _adCreativeStatusService;
        protected static IDeviceCapabilityService _deviceCapabilityService;
        protected static IRichMediaRequiredProtocolService _richMediaRequiredProtocolService;
        protected static ILocationService _locationService;
        protected static IDeviceTypeService _deviceTypeService;
        protected static IPlatformService _platformService;
        protected static ICostModelWrapperService _CostModelWrapperService;
        protected static IVideoTypeService _videoTypeService;
        private static ITrackingEventService _trackingEventService;
        protected static  IAccountService _accountService;
        protected static IVideoDeliveryMethodsService _videoDeliveryMethodsService;
        protected static IAppMarketingPartnerService _appMarketingPartnerServic;

        protected static ICreativeVendorService _creativeVendorService;
        protected static ILanguageService _languageService;
        protected static  IUserService _userService;
        protected static IReportService _reportService;
      //  private readonly IWebHostEnvironment _hostingEnvironment;
        //protected readonly JsonSerializerOptions _jsonOptions;

        protected static IPartyService _partyService;

        static CostElements()
        {


           
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
        public CostElements()
        {
        }


        public async Task<IViewComponentResult> InvokeAsync(
       )
        {

            return View("CostElements");
        }

    }
}
