using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Model.Tree;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using Telerik.Web.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System.Drawing;
using ArabyAds.AdFalcon.Web.Core.Helper;
using ArabyAds.AdFalcon.Web.Controllers.Model;
using ArabyAds.AdFalcon.Web.Controllers.Core.Security;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Web.Controllers.Model.Campaign;

using ArabyAds.AdFalcon.Web.Controllers.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.AdFalcon.Web.Controllers.Model.Report;
using Telerik.Web.Mvc.UI;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using ArabyAds.AdFalcon.Services.Interfaces.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Telerik.Web.Mvc.Extensions;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.ViewComponents.Reports
{
    public class AdChart : ViewComponent
    {
        private static IAppSiteService _appsiteService;
        private static IMetricService _MetricService;
        private static IReportService _ReportService;
        private static ICampaignService _CampaignsService;
        protected static IAccountService _accountService;
        protected static IAudienceSegmentService _AudienceSegmentService;
        private static WriteReportDocumentsHelper _WriteReportHelper;
        static AdChart()
        {
            _AudienceSegmentService = IoC.Instance.Resolve<IAudienceSegmentService>();
            _appsiteService = IoC.Instance.Resolve<IAppSiteService>();
            _MetricService = IoC.Instance.Resolve<IMetricService>();
            _ReportService = IoC.Instance.Resolve<IReportService>();
            _CampaignsService = IoC.Instance.Resolve<ICampaignService>();
            _accountService = IoC.Instance.Resolve<IAccountService>();

            _WriteReportHelper = new WriteReportDocumentsHelper();
        }
        public AdChart()
        {
            
        }

        public async Task<IViewComponentResult> InvokeAsync(
       )
        {
            List<MetricDto> metricDtoList = _MetricService.GetAll().Where(p => p.MetricTarget.ToLower() == "appsite").ToList();
            ViewData["Metrics"] = metricDtoList;
          

            return View("GAdChart");
        }

    }
}
