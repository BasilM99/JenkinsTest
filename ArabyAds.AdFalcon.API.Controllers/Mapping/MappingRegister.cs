using AutoMapper;
using ArabyAds.AdFalcon.API.Controllers.Core.ExceptionHandling;
using ArabyAds.AdFalcon.API.Controllers.Model.Reports;
using ArabyAds.AdFalcon.API.Controllers.Utilities;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.AppSite;
using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace ArabyAds.AdFalcon.API.Controllers.Mapping
{
    public static class MappingRegister
    {
        private static ICountryService _countrySerivce;
        private static IAppSiteService _appsiteService;
 
        static MappingRegister()
        {
            _countrySerivce = IoC.Instance.Resolve<ICountryService>();
            _appsiteService = IoC.Instance.Resolve<IAppSiteService>();

        }

        public static void RegisterMapping()
        {
            RegisterAppSiteStatsCriteriaToReportCtiteriaDto();
        }

        private static void RegisterAppSiteStatsCriteriaToReportCtiteriaDto()
        {
            string requestDteTimeFormat = Config.APIRequestDateTimeFormat;

            Mapper.CreateMap<AppSiteStatsCriteria, AppSiteStatisticsCriteriaDto>()
                .ForMember(p => p.FromDate, x => x.MapFrom(p =>
                    {
                        return DateTime.ParseExact(p.FDate, requestDteTimeFormat, new CultureInfo("en-US"));
                    }))
                .ForMember(p => p.ToDate, x => x.MapFrom(p =>
                {
                    return DateTime.ParseExact(p.TDate, requestDteTimeFormat, new CultureInfo("en-US"));
                }))
                .ForMember(p => p.PageNumber, x => x.MapFrom(p =>
                {
                    return p.OS;
                }))
            .ForMember(p => p.ItemsPerPage, x => x.MapFrom(p =>
                {
                    return p.L;
                }))
            .ForMember(p => p.Layout, x => x.MapFrom(p =>
            {
                return "detailed";
            }))
            .ForMember(p => p.SummaryBy, x => x.MapFrom(p =>
            {
                DateTime fromDate = DateTime.ParseExact(p.FDate, requestDteTimeFormat, new CultureInfo("en-US"));
                DateTime toDate = DateTime.ParseExact(p.TDate, requestDteTimeFormat, new CultureInfo("en-US"));

                toDate = toDate.AddDays(1).AddMinutes(-1);
                TimeSpan difference = toDate - fromDate;

                if (difference.TotalDays < 0)
                    throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.InvalidDateRange);

                if (difference.TotalDays < 1)
                    return SummaryBy.Hour;

                if (difference.TotalDays >= 1 && difference.TotalDays <= 180)
                    return SummaryBy.Day;

                return SummaryBy.Month;
            }))
            .ForMember(p => p.ItemsList, x => x.MapFrom(p =>
            {
                if (string.IsNullOrEmpty(p.AId))
                    return string.Empty;

                AppSiteBasicDto appSite = _appsiteService.GetAppSiteByPublisherId(p.AId);

                if (appSite != null)
                {
                    return appSite.ID.ToString();
                }
                
                return string.Empty;
            }))
            .ForMember(p => p.AdvancedCriteria, x => x.MapFrom(p =>
            {
                if (string.IsNullOrEmpty(p.CC))
                    return string.Empty;

                CountryDto country = _countrySerivce.GetAll().Where(z => string.Compare(z.Code, p.CC, true) == 0).SingleOrDefault();

                if (country != null)
                {
                    return country.ID.ToString();
                }
                else
                {
                    throw new APIException(ErrorCode.BadRequest, APIExceptionMessages.InvalidCountryCode);
                }
            }))
            .ForMember(p => p.GroupBy, x => x.MapFrom(p =>
            {
                return !string.IsNullOrEmpty(p.GB) ? Config.GroupByOptionsList[p.GB] : string.Empty;
            }));
        }
    }
}
