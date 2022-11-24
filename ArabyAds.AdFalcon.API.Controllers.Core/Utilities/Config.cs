using ArabyAds.Framework;
using ArabyAds.Framework.ConfigurationSetting;
using ArabyAds.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.API.Controllers.Utilities
{
    public static class Config
    {
        private static int? _applicationId;
        private static string _supportedAPIVersionNumbers;
        private static int? _maxRequestPerMinute;
        private static int? _maxRequestPerDay;
        private static int? _defaultAPIRequestResultNumber;
        private static int? _maxAPIRequestResultNumber;
        private static string _groupByOptions;
        private static string _apiDateTimeFormat;
        private static string _apiRequestDateTimeFormat;
        private static string _apiDomainFormat;

        private static Dictionary<string, string> _groupByOptionsList;

        private static IConfigurationManager _configurationManager = IoC.Instance.Resolve<IConfigurationManager>();

        public static int ApplicationId
        {
            get
            {
                if (!_applicationId.HasValue)
                {
                    _applicationId = int.Parse(JsonConfigurationManager.AppSettings["ApplicationId"].ToString());
                }

                return _applicationId.Value;
            }
        }

        public static string[] SupportedAPIVersionNumbers
        {
            get
            {
                if (_supportedAPIVersionNumbers == null)
                {
                    _supportedAPIVersionNumbers = _configurationManager.GetConfigurationSetting(ApplicationId, null, "SupportedVersions");
                }

                return _supportedAPIVersionNumbers.Split(',');
            }
        }

        public static int MaxRequestPerMinute
        {
            get
            {
                if (!_maxRequestPerMinute.HasValue)
                {
                    _maxRequestPerMinute = int.Parse(_configurationManager.GetConfigurationSetting(ApplicationId, null, "MaxRequestPerMinute"));
                }

                return _maxRequestPerMinute.Value;
            }
        }

        public static int MaxRequestPerDay
        {
            get
            {
                if (!_maxRequestPerDay.HasValue)
                {
                    _maxRequestPerDay = int.Parse(_configurationManager.GetConfigurationSetting(ApplicationId, null, "MaxRequestPerDay"));
                }

                return _maxRequestPerDay.Value;
            }
        }

        public static int DefaultAPIRequestResultNumber
        {
            get
            {
                if (!_defaultAPIRequestResultNumber.HasValue)
                {
                    _defaultAPIRequestResultNumber = int.Parse(_configurationManager.GetConfigurationSetting(ApplicationId, null, "DefaultAPIRequestResultNumber"));
                }

                return _defaultAPIRequestResultNumber.Value;
            }

        }

        public static int MaxAPIRequestResultNumber 
        {
            get
            {
                if (!_maxAPIRequestResultNumber.HasValue)
                {
                    _maxAPIRequestResultNumber = int.Parse(_configurationManager.GetConfigurationSetting(ApplicationId, null, "MaxAPIRequestResultNumber"));
                }

                return _maxAPIRequestResultNumber.Value;
            }
        }

        public static string GroupByOptions
        {
            get
            {
                if (string.IsNullOrEmpty(_groupByOptions))
                {
                    _groupByOptions = _configurationManager.GetConfigurationSetting(ApplicationId, null, "GroupByOptions");
                }

                return _groupByOptions;
            }
        }

        public static Dictionary<string,string> GroupByOptionsList
        {
            get
            {
                if (_groupByOptionsList == null)
                {
                    _groupByOptionsList = (from groupOption in GroupByOptions.Split(',')
                                              select new KeyValuePair<string, string>(groupOption.Split(':')[0], groupOption.Split(':')[1]))
                                              .ToDictionary(p => p.Key.ToLower(), p => p.Value);
                                                                                                  
                }

                return _groupByOptionsList;
            }
        }

        public static string APIDateTimeFormat
        {
            get
            {
                if (string.IsNullOrEmpty(_apiDateTimeFormat))
                {
                    _apiDateTimeFormat = _configurationManager.GetConfigurationSetting(ApplicationId, null, "APIDateTimeFormat");
                }

                return _apiDateTimeFormat;
            }
        }

        public static string APIRequestDateTimeFormat
        {
            get
            {
                if (string.IsNullOrEmpty(_apiRequestDateTimeFormat))
                {
                    _apiRequestDateTimeFormat = _configurationManager.GetConfigurationSetting(ApplicationId, null, "APIRequestDateTimeFormat");
                }

                return _apiRequestDateTimeFormat;
            }
        }

        public static string APIDomainFormat
        {
            get
            {
                if (string.IsNullOrEmpty(_apiDomainFormat))
                {
                    _apiDomainFormat = _configurationManager.GetConfigurationSetting(ApplicationId, null, "APIDomainFormat");
                }

                return _apiDomainFormat;
            }
        }

    }
}
