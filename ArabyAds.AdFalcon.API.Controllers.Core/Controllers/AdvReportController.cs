using ArabyAds.AdFalcon.API.Controllers.Core.ExceptionHandling;
using ArabyAds.AdFalcon.API.Controllers.Core.Response;
using ArabyAds.AdFalcon.API.Controllers.Core.Response.ResponseData;
using ArabyAds.AdFalcon.API.Controllers.Model.Reports;
using ArabyAds.AdFalcon.API.Controllers.Utilities;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ArabyAds.AdFalcon.API.Controllers
{
    public class AdvReportController : PubReportController
    {
        public AdvReportController()
            : base()
        {
        }

    }
}
