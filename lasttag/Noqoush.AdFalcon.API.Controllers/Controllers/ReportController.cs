using Noqoush.AdFalcon.API.Controllers.Core.ExceptionHandling;
using Noqoush.AdFalcon.API.Controllers.Core.Response;
using Noqoush.AdFalcon.API.Controllers.Core.Response.ResponseData;
using Noqoush.AdFalcon.API.Controllers.Model.Reports;
using Noqoush.AdFalcon.API.Controllers.Utilities;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.API.Controllers
{
    public class ReportController : PubReportController
    {
        public ReportController(IReportService reportService)
            : base(reportService)
        {
        }

    }
}
