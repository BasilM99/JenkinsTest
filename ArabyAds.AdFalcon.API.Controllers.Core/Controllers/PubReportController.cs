using AutoMapper;
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
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.Configuration;
using ArabyAds.AdFalcon.API.Controllers.Mapping;
using Microsoft.AspNetCore.Routing;

namespace ArabyAds.AdFalcon.API.Controllers
{
    public class PubReportController : ArabyAds.AdFalcon.API.Controllers.Core.Controllers.ControllerBase
    {
        private IReportService _reportService;



  
        public PubReportController()
        {
            _reportService = IoC.Instance.Resolve<IReportService>() ;
        }

        public async Task<ActionResult> Stats([FromBody] AppSiteStatsCriteriaContainer container, bool isTest)
        {
           var criteria= container.criteria;
            criteria.IsTest = isTest;
          //  BuildCriteria(ref criteria);
            ValidateCriteria(ref criteria);
            return await Task.FromResult(GetAppSiteStatisticsReport(criteria));
        }

        private void BuildCriteria(ref AppSiteStatsCriteria criteria)
        {
            PropertyInfo[] properties = typeof(AppSiteStatsCriteria).GetProperties();

            NameValueCollection jsonParameters = ConvertJSONParametersToNameValueCollection(Request.Body);

            foreach (var item in jsonParameters)
            {
                string key = item.ToString();
                PropertyInfo property = properties.Where(p => String.Compare(p.Name, key, true) == 0).SingleOrDefault();
                if (property != null)
                {
                    // This to handle nullable types like int?
                    Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    string value = jsonParameters[key];

                    if (!string.IsNullOrEmpty(value))
                    {
                        object safeValue = (value == null) ? null
                                       : Convert.ChangeType(value, underlyingType);

                        property.SetValue(criteria, safeValue, null);
                    }
                }
            }
        }

        private NameValueCollection ConvertJSONParametersToNameValueCollection(Stream stream)
        {
            string jsonParametersString = GetJSONString(stream);
            NameValueCollection jsonParametersNameValue = new NameValueCollection();

            if (!string.IsNullOrEmpty(jsonParametersString))
            {
                JObject jsonParameters = JObject.Parse(System.Net.WebUtility.UrlDecode(jsonParametersString));

                if (jsonParameters["criteria"] != null)
                {
                    foreach (var result in jsonParameters["criteria"])
                    {
                        jsonParametersNameValue.Add(((JProperty)result).Name, ((JProperty)result).Value.ToString());
                    }
                }
            }

            return jsonParametersNameValue;
        }

        private string GetJSONString(Stream stream)
        {
            string jsonParametersString = null;
            if (stream != null)
            {
                if (stream.Position != 0)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                NameValueCollection jsonParametersNameValue = new NameValueCollection();
                using (StreamReader requestStreamReader = new StreamReader(stream))
                {
                    jsonParametersString = requestStreamReader.ReadToEnd();
                }
            }

            return jsonParametersString;
        }


        private ActionResult GetAppSiteStatisticsReport(AppSiteStatsCriteria criteria)
        {
            AppSiteStatisticsCriteriaDto reportCriteria = null;

            try

            {
                

             
             

                reportCriteria = MappingRegister.Mapper.Map<AppSiteStatisticsCriteriaDto>(criteria);
            }
            catch (AutoMapperMappingException x)
            {
                if (x.InnerException != null && x.InnerException.InnerException != null)
                {
                    APIException internalException = x.InnerException.InnerException as APIException;
                    if (internalException != null)
                    {
                        throw internalException;
                    }
                }

                throw x;
            }

            reportCriteria.AccountId = CallContext.Current.Items.TryGetValue("AccountId", out var accountId) ? int.Parse(accountId.ToString()) : 0;

            List<string> execludedProperties = new List<string>();
            execludedProperties.Add("Date");
            execludedProperties.Add("TimeId");

            List<AppSiteStatisticsReport> appStatsReport;

            if (criteria.IsTest)
            {
                if (string.IsNullOrEmpty(reportCriteria.GroupBy))
                {
                    appStatsReport = TestingDataUtility.GetAppSiteStatistcsReport();
                }
                else
                {
                    appStatsReport = TestingDataUtility.GetAppSiteStatistcsGeoReport().Select(p => p as AppSiteStatisticsReport).ToList();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(reportCriteria.GroupBy) && string.IsNullOrEmpty(reportCriteria.AdvancedCriteria))
                {
                    appStatsReport = _reportService.GetAppSiteStatisticsReport(reportCriteria);
                }
                else
                {
                    appStatsReport = _reportService.GetAppSiteStatisticsGeoReport(reportCriteria).Select(p => p as AppSiteStatisticsReport).ToList();
                }

                foreach (var item in appStatsReport)
                {
                    item.d = DateTimeUtility.FixDate(DateTime.ParseExact(item.Date.ToString(), "yyyyMMdd", null), item.TimeId, reportCriteria.FromDate, (int)reportCriteria.SummaryBy);
                }
            }

            return new APIReponseResult<List<AppSiteStatisticsReport>>(new JSONResponseData<List<AppSiteStatisticsReport>>(appStatsReport, execludedProperties));
        }

        private void ValidateCriteria(ref AppSiteStatsCriteria criteria)
        {
            DateTime currentDateTime = DateTime.Now;

            if (string.IsNullOrEmpty(criteria.F))
            {
                throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.MissingParameters);
            }

            // If (tdate) parameter is null or empty, assign the default value which is the first day of current month
            if (string.IsNullOrEmpty(criteria.FDate))
            {
                criteria.FDate = (new DateTime(currentDateTime.Year, currentDateTime.Month, 1)).ToString(Config.APIRequestDateTimeFormat);
            }
            else
            {
                // Check that (tdate) parameter matches (yyyy-MM-dd) format
                DateTime fDate;
                bool parseResult = DateTime.TryParseExact(criteria.FDate, Config.APIRequestDateTimeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out fDate);

                if (!parseResult)
                {
                    throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.InvalidFromDateFormat);
                }
            }


            // If (tdate) parameter is null or empty, assign the default value which is the last day of current month
            if (string.IsNullOrEmpty(criteria.TDate))
            {
                DateTime nextMonthDate = currentDateTime.AddMonths(1);
                DateTime nextMonthFirstDayDate = new DateTime(nextMonthDate.Year, nextMonthDate.Month, 1);

                criteria.TDate = nextMonthFirstDayDate.AddDays(-1).ToString(Config.APIRequestDateTimeFormat);
            }
            else
            {
                // Check that (tdate) parameter matches (yyyy-MM-dd) format
                DateTime tDate;
                bool parseResult = DateTime.TryParseExact(criteria.TDate, Config.APIRequestDateTimeFormat, new CultureInfo("en-US"), DateTimeStyles.None, out tDate);

                if (!parseResult)
                {
                    throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.InvalidToDateFormat);
                }
            }

            // Check (l) parameter
            if (!criteria.L.HasValue)
            {
                // If (l) parameter is null, assign default value
                criteria.L = Config.DefaultAPIRequestResultNumber;
            }
            else
            {
                // Check that (l) doesnt exceed the maximum limit
                int maxAPIRequestResultNumber = Config.MaxAPIRequestResultNumber;
                if (criteria.L > maxAPIRequestResultNumber)
                {
                    throw new APIException(ErrorCode.BadRequest, string.Format(APIExceptionMessages.LengthExceedMaxNumber, maxAPIRequestResultNumber));
                }

            }

            // Check (gb) parameter 
            if (!string.IsNullOrEmpty(criteria.GB))
            {
                if (!Config.GroupByOptionsList.ContainsKey(criteria.GB.ToLower()))
                {
                    throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.InvalidGroupByOption);
                }
            }

            // Check (aid) parameter
            if (!string.IsNullOrEmpty(criteria.AId))
            {
                Guid newGUid;
                // Check that (aid) parameter matches the format (00000000000000000000000000000000)
                if (!(Guid.TryParseExact(criteria.AId,"N",out newGUid)))
                {
                    throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.InvalidAppIdFormat);
                }
            }
        }
    }
}
