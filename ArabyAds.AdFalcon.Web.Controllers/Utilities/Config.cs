using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Noqoush.Framework;
using Noqoush.Framework.ConfigurationSetting;

namespace Noqoush.AdFalcon.Web.Controllers.Utilities
{
    public static class Config
    {

        private static int? pageSize;
        private static int? currentHourMinMinute = null;
        private static double? defaultRevenuePercentage = null;
        private static string shortDateFormat = string.Empty;
        private static string timeFormat = string.Empty;
        private static string longDateFormat = string.Empty;
        private static string clientShortDateFormat = string.Empty;
        private static string clientLongDateFormat = string.Empty;
        private static string defaultController = string.Empty;
        private static string defaultAction = string.Empty;
        private static string cookieDomain = string.Empty;
        private static IConfigurationManager configurationManager = null;
        private static int? applicationId;
        private static int? WifiOperaterId;
        private static bool? isAdministrationApp;
        private static int? maxHoursDifference;
        private static int? instreamVideoDuraionLimit;
        private static int? trackingEvent_ValidForSecondMax;
        private static int? trackingEvent_ValidForSecondMin;

        private static string _TrackConversionsHttpUrl = null;
        private static string _TrackConversionsHttpsUrl = null;
        private static int? messagesDurationTime;
        private static string userAgreementVersion = string.Empty;
        private static string dspuserAgreementVersion = string.Empty;
        private static DateTime? userAgreementEffectiveDate = null;
        private static int? maxAdGroupTrackingEvents = null;
        private static string _hostName = null;
        private static string _publicHostName = null;
        private static DateTime? dspuserAgreementEffectiveDate = null;
        public static decimal? appSiteRevenuePercentage;
        private static string _AudianceSegmentCostModelTypeAllowed = null;

        private static int? sizeHTML5 = null;
        private static int? sizeCSV = null;
        private static string _ScriptBannerCreative = null;
        private static string _ImageURLHTTPConv = null;
        private static string _ImageURLHTTPSConv = null;
        private static string _MobileTrackingURLConvHTTPIOS = null;
        private static string _MobileTrackingURLConvHTTPSIOS = null;
        private static string _MobileTrackingURLConvHTTPAND = null;
        private static string _MobileTrackingURLConvHTTPSAND = null;
        private static string _ConfigForMeasureDimensionFilter = null;
        private static string _ScriptTagConv = null;

        private static string _ConfigForCriteriaSearchReportBuilder = null;
        private static string _ConfigForCriteriaReportBuilder = null;
        



        public static decimal AppSiteRevenuePercentage
        {
            get
            {
                if (!(appSiteRevenuePercentage.HasValue))
                {
                    appSiteRevenuePercentage = Convert.ToDecimal(ConfigurationManager.GetConfigurationSetting(null, null, "AppsiteRevenuePercentage"));
                }

                return appSiteRevenuePercentage.Value;
            }
        }
        public static string AudianceSegmentCostModelTypeAllowed
        {
            get
            {
                if (_AudianceSegmentCostModelTypeAllowed == null)
                {
                    _AudianceSegmentCostModelTypeAllowed = configurationManager.GetConfigurationSetting(null, null, "AudianceSegmentCostModelTypeAllowed");
                }

                return _AudianceSegmentCostModelTypeAllowed;
            }
        }

        public static int MaxHoursDifference
        {
            get
            {
                if (!maxHoursDifference.HasValue)
                {
                    var _maxHoursDifference = 10;
                    int.TryParse(ConfigurationManager.GetConfigurationSetting(null, null, "MaxHoursDifference"), out _maxHoursDifference);
                    maxHoursDifference = _maxHoursDifference;
                }
                return maxHoursDifference.Value;
            }
        }


        public static bool IsAdministrationApp
        {
            get
            {
                if (!isAdministrationApp.HasValue)
                {
                    var _isAdministrationApp = false;
                    bool.TryParse(ConfigurationManager.GetConfigurationSetting(ApplicationId, null, "IsAdministrationApp"), out _isAdministrationApp);
                    isAdministrationApp = _isAdministrationApp;
                }
                return isAdministrationApp.Value;
            }
        }

        public static bool IsShowAdminSection
        {
            get { return IsAdmin && IsAdministrationApp; }
        }
        public static bool IsAdminInAdminApp
        {
            get { return IsAdmin && IsAdministrationApp; }
        }

        public static bool IsAdOpsAdminInAdminApp
        {
            get { return (IsAdmin || IsAdOps || IsAccountManager) && IsAdministrationApp; }
        }

        public static bool IsAppOpsAdminInAdminApp
        {
            get { return IsAppOpsAdmin && IsAdministrationApp; }
        }

        public static bool IsAppOpsAdmin
        {
            get { return (IsAdmin || IsAppOps); }
        }

        public static bool IsAdOpsAdmin
        {
            get { return (IsAdmin || IsAdOps || IsAccountManager); }
        }


        public static bool IsAdmin
        {
            get
            {
                return IsInRole("Administrator");
            }
        }

        public static bool IsAdOps
        {
            get
            {
                return IsInRole("AdOps");
            }
        }
        public static bool IsAccountManager
        {
            get
            {
                return IsInRole("AccountManager");
            }
        }


        public static bool IsAppOps
        {
            get
            {
                return IsInRole("AppOps");
            }
        }

        public static bool IsInRole(string role)
        {
            return OperationContext.Current.CurrentPrincipal.IsInRole(role);
        }

        public static int CurrentHourMinMinute
        {
            get
            {
                if (!currentHourMinMinute.HasValue)
                {
                    var _currentHourMinMinute = 30;
                    int.TryParse(ConfigurationManager.GetConfigurationSetting(ApplicationId, null, "CurrentHourMinMinute"), out _currentHourMinMinute);
                    currentHourMinMinute = _currentHourMinMinute;
                }
                return currentHourMinMinute.Value;
            }
        }
        public static double DefaultRevenuePercentage
        {
            get
            {
                if (!defaultRevenuePercentage.HasValue)
                {
                    var _defaultRevenuePercentage = 0.6;
                    double.TryParse(ConfigurationManager.GetConfigurationSetting(ApplicationId, null, "DefaultAppsiteRevenuePercentage"), out _defaultRevenuePercentage);
                    defaultRevenuePercentage = _defaultRevenuePercentage;
                }
                return (defaultRevenuePercentage.Value * 100);
            }
        }
        public static int WIFIOperaterId
        {
            get
            {
                if (!WifiOperaterId.HasValue)
                {
                    var wifiOperaterId = -1;
                    int.TryParse(ConfigurationManager.GetConfigurationSetting(ApplicationId, null, "WiFiOperatorId"), out wifiOperaterId);
                    WifiOperaterId = wifiOperaterId;
                }
                return WifiOperaterId.Value;
            }
        }
        public static int PageSize
        {
            get
            {
                if (!pageSize.HasValue)
                {
                    var psize = 10;
                    int.TryParse(ConfigurationManager.GetConfigurationSetting(null, null, "PageSize"), out psize);
                    pageSize = psize;
                }
                return pageSize.Value;
            }
        }

        public static int? ApplicationId
        {
            get
            {

                if (!applicationId.HasValue)
                {
                    int temp;
                    if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["ApplicationId"], out temp))
                        applicationId = temp;
                }
                return applicationId;
            }
        }
        private static string GetDateFormat(string dateFormat)
        {
            //TODO:Osaleh to reconsider changeing this code with more genaric code
            var tempShortDateFormat = dateFormat;
            if (CurrentDirection == "rtl")
            {
                var arr = tempShortDateFormat.ToCharArray();
                Array.Reverse(arr);
                tempShortDateFormat = new string(arr);
            }
            return tempShortDateFormat;
        }

        public static string ShortDateFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(shortDateFormat))
                {
                    shortDateFormat = ConfigurationManager.GetConfigurationSetting(null, null, "ShortDateFormat");
                    if (string.IsNullOrWhiteSpace(shortDateFormat))
                    {
                        shortDateFormat = "dd-MM-yyyy";
                    }
                }
                return GetDateFormat(shortDateFormat);

            }
        }

        public static string MainShortDateFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(shortDateFormat))
                {
                    shortDateFormat = ConfigurationManager.GetConfigurationSetting(null, null, "ShortDateFormat");
                    if (string.IsNullOrWhiteSpace(shortDateFormat))
                    {
                        shortDateFormat = "dd-MM-yyyy";
                    }
                }
                return shortDateFormat;

            }
        }

        public static string TimeFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(timeFormat))
                {
                    timeFormat = ConfigurationManager.GetConfigurationSetting(null, null, "TimeFormat");
                    if (string.IsNullOrWhiteSpace(timeFormat))
                    {
                        timeFormat = "hh:mm";
                    }
                }
                return timeFormat;
            }
        }

        public static string LongDateFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(longDateFormat))
                {
                    longDateFormat = ConfigurationManager.GetConfigurationSetting(null, null, "LongDateFormat");
                    if (string.IsNullOrWhiteSpace(longDateFormat))
                    {
                        longDateFormat = "dd-MM-yyyy";
                    }
                }
                return GetDateFormat(longDateFormat);

            }
        }
        public static string ClientShortDateFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(clientShortDateFormat))
                {
                    clientShortDateFormat = ConfigurationManager.GetConfigurationSetting(null, null, "ClientShortDateFormat");
                    if (string.IsNullOrWhiteSpace(clientShortDateFormat))
                    {
                        clientShortDateFormat = "dd-mm-yy";
                    }
                }
                return GetDateFormat(clientShortDateFormat);
            }
        }
        public static string ClientLongDateFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(clientLongDateFormat))
                {
                    clientLongDateFormat = ConfigurationManager.GetConfigurationSetting(null, null, "ClientLongDateFormat");
                    if (string.IsNullOrWhiteSpace(clientLongDateFormat))
                    {
                        clientLongDateFormat = "dd-mm-yy";
                    }
                }
                return GetDateFormat(clientLongDateFormat);
            }
        }
        public static IConfigurationManager ConfigurationManager
        {
            get
            {
                return configurationManager ?? (configurationManager = Framework.IoC.Instance.Resolve<IConfigurationManager>());
            }
        }
        public static string CurrentLanguage
        {
            get { return System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName; }
        }

        public static string CurrentFloat
        {
            get
            {
                var value = string.Empty;
                var lang = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                switch (lang)
                {
                    case "ar":
                        {
                            value = "right";
                            break;
                        }
                    default:
                        {
                            value = "left";
                            break;
                        }
                }
                return value;
            }
        }

        public static string OppositeFloat
        {
            get
            {
                var value = string.Empty;
                var lang = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                switch (lang)
                {
                    case "ar":
                        {
                            value = "left";
                            break;
                        }
                    default:
                        {
                            value = "right";
                            break;
                        }
                }
                return value;
            }
        }

        public static string CurrentDirection
        {
            get
            {
                var value = string.Empty;
                var lang = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                switch (lang)
                {
                    case "ar":
                        {
                            value = "rtl";
                            break;
                        }
                    default:
                        {
                            value = "ltr";
                            break;
                        }
                }
                return value;
            }
        }
        public static string DefaultController
        {
            get
            {
                if (string.IsNullOrWhiteSpace(defaultController))
                {
                    defaultController = ConfigurationManager.GetConfigurationSetting(null, null, "DefaultController");
                    if (string.IsNullOrWhiteSpace(defaultController))
                    {
                        defaultController = "Campaign";
                    }
                }
                return defaultController;
            }
        }
        public static string DefaultAction
        {
            get
            {
                if (string.IsNullOrWhiteSpace(defaultAction))
                {
                    defaultAction = ConfigurationManager.GetConfigurationSetting(null, null, "DefaultAction");
                    if (string.IsNullOrWhiteSpace(defaultAction))
                    {
                        defaultAction = "Index";
                    }
                }
                return defaultAction;
            }
        }
        public static string RootUrl
        {
            get
            {
                var returnUrl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + System.Web.HttpContext.Current.Request.ApplicationPath;
                return returnUrl.Trim('/') + "/";
            }
        }
        public static string CookieDomain
        {
            get
            {
                //TODO:Osaleh to remove this temp value
                if (string.IsNullOrWhiteSpace(cookieDomain))
                {
                    cookieDomain = ConfigurationManager.GetConfigurationSetting(ApplicationId, null, "CookieDomain");
                    if (string.IsNullOrWhiteSpace(cookieDomain))
                    {
                        cookieDomain = ".adfalocn.com";
                    }
                }
                // cookieDomain = ".localadfalocn.com";
                return cookieDomain;
            }
        }

        public static string UserAgreementVersion
        {
            get
            {
                if (string.IsNullOrWhiteSpace(userAgreementVersion))
                {
                    userAgreementVersion = ConfigurationManager.GetConfigurationSetting(null, null, "UserAgreementVersion");
                    if (string.IsNullOrWhiteSpace(userAgreementVersion))
                    {
                        userAgreementVersion = "1.0";
                    }
                }
                return userAgreementVersion;
            }
        }

        public static DateTime? UserAgreementEffectiveDate
        {
            get
            {

                if (!userAgreementEffectiveDate.HasValue)
                {
                    DateTime temp;
                    if (DateTime.TryParseExact(ConfigurationManager.GetConfigurationSetting(null, null, "UserAgreementEffectiveDate"), "dd-MM-yyyy", System.Threading.Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out temp))
                        userAgreementEffectiveDate = temp;
                }
                return userAgreementEffectiveDate;
            }
        }
        public static string DSPUserAgreementVersion
        {
            get
            {
                if (string.IsNullOrWhiteSpace(dspuserAgreementVersion))
                {
                    dspuserAgreementVersion = ConfigurationManager.GetConfigurationSetting(null, null, "DSPUserAgreementVersion");
                    if (string.IsNullOrWhiteSpace(dspuserAgreementVersion))
                    {
                        dspuserAgreementVersion = "1.0";
                    }
                }
                return dspuserAgreementVersion;
            }
        }

        public static DateTime? DSPUserAgreementEffectiveDate
        {
            get
            {

                if (!dspuserAgreementEffectiveDate.HasValue)
                {
                    DateTime temp;
                    if (DateTime.TryParseExact(ConfigurationManager.GetConfigurationSetting(null, null, "DSPUserAgreementEffectiveDate"), "dd-MM-yyyy", System.Threading.Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out temp))
                        dspuserAgreementEffectiveDate = temp;
                }
                return dspuserAgreementEffectiveDate;
            }
        }


        public static int MaxAdGroupTrackingEvents
        {
            get
            {
                if (!maxAdGroupTrackingEvents.HasValue)
                {
                    maxAdGroupTrackingEvents = int.Parse(configurationManager.GetConfigurationSetting(null, null, "MaxAdGroupTrackingEvents"));
                }

                return maxAdGroupTrackingEvents.Value;
            }
        }
        public static bool AllowPrivateIPValidation
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "AllowPrivateIPValidation") != null || configurationManager.GetConfigurationSetting(null, null, "AllowPrivateIPValidation") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "AllowPrivateIPValidation"));
                }

                return false;
            }
        }
        public static string HostName
        {
            get
            {
                if (string.IsNullOrEmpty(_hostName))
                {
                    _hostName = configurationManager.GetConfigurationSetting(ApplicationId, null, "HostName");
                }

                return _hostName;
            }
        }

        public static string PublicHostName
        {
            get
            {
                if (string.IsNullOrEmpty(_publicHostName))
                {
                    _publicHostName = configurationManager.GetConfigurationSetting(2, null, "HostName");
                }

                return _publicHostName;
            }
        }
        public static string TrackConversionsHttpUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_TrackConversionsHttpUrl))
                {
                    _TrackConversionsHttpUrl = configurationManager.GetConfigurationSetting(2, null, "TrackConversionsHttpUrl");
                }

                return _TrackConversionsHttpUrl;
            }
        }
        public static string TrackConversionsHttpsUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_TrackConversionsHttpsUrl))
                {
                    _TrackConversionsHttpsUrl = configurationManager.GetConfigurationSetting(2, null, "TrackConversionsHttpsUrl");
                }

                return _TrackConversionsHttpsUrl;
            }
        }
        public static int InstreamVideoDuraionLimit
        {
            get
            {
                if (!instreamVideoDuraionLimit.HasValue)
                {
                    instreamVideoDuraionLimit = int.Parse(configurationManager.GetConfigurationSetting(null, null, "InstreamVideoDuraionLimit"));
                }

                return instreamVideoDuraionLimit.Value;
            }
        }

        public static int TrackingEvent_ValidForSecondMax
        {
            get
            {
                if (!trackingEvent_ValidForSecondMax.HasValue)
                {
                    trackingEvent_ValidForSecondMax = int.Parse(configurationManager.GetConfigurationSetting(null, null, "TrackingEvent_ValidForSecondMax"));
                }

                return trackingEvent_ValidForSecondMax.Value;
            }
        }

        public static int TrackingEvent_ValidForSecondMin
        {
            get
            {
                if (!trackingEvent_ValidForSecondMin.HasValue)
                {
                    trackingEvent_ValidForSecondMin = int.Parse(configurationManager.GetConfigurationSetting(null, null, "TrackingEvent_ValidForSecondMin"));
                }

                return trackingEvent_ValidForSecondMin.Value;
            }
        }
        public static int MessagesDurationTime
        {
            get
            {
                if (!messagesDurationTime.HasValue)
                {
                    messagesDurationTime = int.Parse(configurationManager.GetConfigurationSetting(ApplicationId, null, "MessagesDurationTime"));
                }

                return messagesDurationTime.Value;
            }
        }

        public static int SizeHTML5
        {
            get
            {
                if (!sizeHTML5.HasValue)
                {
                    sizeHTML5 = int.Parse(configurationManager.GetConfigurationSetting(null, null, "SizeHTML5"));
                }

                return sizeHTML5.Value;
            }
        }

        public static int SizeCSV
        {
            get
            {
                if (!sizeCSV.HasValue)
                {
                    sizeCSV = int.Parse(configurationManager.GetConfigurationSetting(null, null, "SizeCSV"));
                }

                return sizeCSV.Value;
            }
        }

        public static bool IsDataCostElem
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "IsDataCostElem") != null || configurationManager.GetConfigurationSetting(null, null, "IsDataCostElem") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "IsDataCostElem"));
                }

                return false;
            }
        }
        public static bool IsThirdPartyCostElem
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "IsThirdPartyCostElem") != null || configurationManager.GetConfigurationSetting(null, null, "IsThirdPartyCostElem") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "IsThirdPartyCostElem"));
                }

                return false;
            }
        }
        public static bool IsPlatformCostElem
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "IsPlatformCostElem") != null || configurationManager.GetConfigurationSetting(null, null, "IsPlatformCostElem") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "IsPlatformCostElem"));
                }

                return false;
            }
        }
        public static bool IsAVRCostElem
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "IsAVRCostElem") != null || configurationManager.GetConfigurationSetting(null, null, "IsAVRCostElem") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "IsAVRCostElem"));
                }

                return false;
            }
        }
        public static bool IsExchangeDiscrepancyCostElem
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "IsExchangeDiscrepancyCostElem") != null || configurationManager.GetConfigurationSetting(null, null, "IsExchangeDiscrepancyCostElem") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "IsExchangeDiscrepancyCostElem"));
                }

                return false;
            }
        }



        public static bool IsDataFee
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "IsDataFee") != null || configurationManager.GetConfigurationSetting(null, null, "IsDataFee") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "IsDataFee"));
                }

                return false;
            }
        }
        public static bool IsThirdPartyFee
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "IsThirdPartyFee") != null || configurationManager.GetConfigurationSetting(null, null, "IsThirdPartyFee") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "IsThirdPartyFee"));
                }

                return false;
            }
        }
        public static bool IsPlatformFee
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "IsPlatformFee") != null || configurationManager.GetConfigurationSetting(null, null, "IsPlatformFee") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "IsPlatformFee"));
                }

                return false;
            }
        }
        public static bool IsAVRFee
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "IsAVRFee") != null || configurationManager.GetConfigurationSetting(null, null, "IsAVRFee") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "IsAVRFee"));
                }

                return false;
            }
        }
        public static bool IsExchangeDiscrepancyFee
        {
            get
            {
                if (configurationManager.GetConfigurationSetting(null, null, "IsExchangeDiscrepancyFee") != null || configurationManager.GetConfigurationSetting(null, null, "IsExchangeDiscrepancyFee") != string.Empty)
                {
                    return bool.Parse(configurationManager.GetConfigurationSetting(null, null, "IsExchangeDiscrepancyFee"));
                }

                return false;
            }
        }



        public static string ImageURLHTTPConv
        {
            get
            {
                if (string.IsNullOrEmpty(_ImageURLHTTPConv))
                {
                    _ImageURLHTTPConv = configurationManager.GetConfigurationSetting(null, null, "ImageURLHTTPConv");
                }

                return _ImageURLHTTPConv;
            }
        }
        public static string ImageURLHTTPSConv
        {
            get
            {
                if (string.IsNullOrEmpty(_ImageURLHTTPSConv))
                {
                    _ImageURLHTTPSConv = configurationManager.GetConfigurationSetting(null, null, "ImageURLHTTPSConv");
                }

                return _ImageURLHTTPSConv;
            }
        }


        public static string ScriptTagConv
        {
            get
            {
                if (string.IsNullOrEmpty(_ScriptTagConv))
                {
                    _ScriptTagConv = configurationManager.GetConfigurationSetting(null, null, "ScriptTagConv");
                }

                return _ScriptTagConv;
            }
        }


        public static string MobileTrackingURLConvHTTPIOS
        {
            get
            {
                if (string.IsNullOrEmpty(_MobileTrackingURLConvHTTPIOS))
                {
                    _MobileTrackingURLConvHTTPIOS = configurationManager.GetConfigurationSetting(null, null, "MobileTrackingURLConvHTTPIOS");
                }

                return _MobileTrackingURLConvHTTPIOS;
            }
        }
        public static string MobileTrackingURLConvHTTPSIOS
        {
            get
            {
                if (string.IsNullOrEmpty(_MobileTrackingURLConvHTTPSIOS))
                {
                    _MobileTrackingURLConvHTTPSIOS = configurationManager.GetConfigurationSetting(null, null, "MobileTrackingURLConvHTTPSIOS");
                }

                return _MobileTrackingURLConvHTTPSIOS;
            }
        }
        public static string MobileTrackingURLConvHTTPAND
        {
            get
            {
                if (string.IsNullOrEmpty(_MobileTrackingURLConvHTTPAND))
                {
                    _MobileTrackingURLConvHTTPAND = configurationManager.GetConfigurationSetting(null, null, "MobileTrackingURLConvHTTPAND");
                }

                return _MobileTrackingURLConvHTTPAND;
            }
        }
        public static string MobileTrackingURLConvHTTPSAND
        {
            get
            {
                if (string.IsNullOrEmpty(_MobileTrackingURLConvHTTPSAND))
                {
                    _MobileTrackingURLConvHTTPSAND = configurationManager.GetConfigurationSetting(null, null, "MobileTrackingURLConvHTTPSAND");
                }

                return _MobileTrackingURLConvHTTPSAND;
            }
        }



        public static string ConfigForMeasureDimensionFilter
        {
            get
            {
                if (string.IsNullOrEmpty(_ConfigForMeasureDimensionFilter))
                {
                    _ConfigForMeasureDimensionFilter = configurationManager.GetConfigurationSetting(null, null, "ConfigForMeasureDimensionFilter");
                }

                return _ConfigForMeasureDimensionFilter;
            }
        }


        public static string ConfigForCriteriaSearchReportBuilder
        {
            get
            {
                if (string.IsNullOrEmpty(_ConfigForCriteriaSearchReportBuilder))
                {
                    _ConfigForCriteriaSearchReportBuilder = configurationManager.GetConfigurationSetting(null, null, "ConfigForCriteriaSearchReportBuilder");
                }

                return _ConfigForCriteriaSearchReportBuilder;
            }
        }



        public static string ConfigForCriteriaReportBuilder
        {
            get
            {
                if (string.IsNullOrEmpty(_ConfigForCriteriaReportBuilder))
                {
                    _ConfigForCriteriaReportBuilder = configurationManager.GetConfigurationSetting(null, null, "ConfigForCriteriaReportBuilder");
                }

                return _ConfigForCriteriaReportBuilder;
            }
        }



        public static string ScriptBannerCreative
        {
            get
            {
                if (string.IsNullOrEmpty(_ScriptBannerCreative))
                {
                    _ScriptBannerCreative = configurationManager.GetConfigurationSetting(null, null, "ScriptBannerCreative");
                }

                return _ScriptBannerCreative;
            }


        }




    }
}
