using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework;
using Noqoush.Framework.ConfigurationSetting;
using Noqoush.Framework.DistributedEventBroker.PubSub.Entities;
using Noqoush.Framework.DistributedEventBroker.PubSub.Publishing;

using Noqoush.Framework.Kafka;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Domain
{
    public static class Configuration
    {
        private static IConfigurationManager configurationManager = null;

        public static IConfigurationManager ConfigurationManager
        {
            get
            {
                return configurationManager ?? (configurationManager = Framework.IoC.Instance.Resolve<IConfigurationManager>());
            }
        }

        private static Id64Generator _id64Generator;

        private static bool? _KafkaEnabled;
        private static int? maxHoursDifference;
        private static bool? sendAdServerEmails;
        private static string eventBrokerEmail;
        private static string financeEmail;
        private static string adOpsEmail;
        private static string adminEmail;
        private static string ftpBaseDirectory;
        private static string cdnBaseUrl;
        private static string normalExpandableRichMediaTemplate;
        private static string mRAID1ExpandableRichMediaTemplate;
        private static string mRAID2ExpandableRichMediaTemplate;
        private static string mediaplex;
        private static string doubleclick;
        private static string celtra;
        private static string openx;
        private static string adform;
        private static string crispa;
        private static string atlaSoft;
        private static string vicinity;
        private static string adlit;
        private static string flashTalking;
        private static string sizmik;
        private static string googletag;

        private static int? trackingEvent_ValidForSecondMax;
        private static int? trackingEvent_ValidForSecondMin;

        private static int? t_AdFalconPortalProduceBatchTimeOutKafka;
        private static string clickMacro;
        private static string impressionMacro;
        private static string clickRedirectMacro;
        private static string impressionRedirectMacro;
        private static List<string> impressionMacrosList;
        private static List<string> clickMacrosList;
        private static string impressionImageTemplate;
        private static string db;

        private static string shortDateFormat;
        private static DateTime? _ApplicationReleaseDate;
        private static int? _MaxAdGroupTrackingEvents;

        private static int? _MaxAdGroupConversionEvents;

        private static string _StatisticsColumnPrefixName;
        private static string _NativeAdCreativeUnitCode;
        private static string _CreativeUnitPrefixNameFormatClickTrackerURL;
        private static string HTML5PreTagScriptstr;
       // private static HttpHost[] KafkaServerList;
        private static IEventPublisher _KafkaEventPublisher;
        private static decimal? _DynamicBiddingDefaultBidPricePerc;
        private static decimal? _DynamicBiddingDefaultBidStep;
        private static decimal? _DynamicBiddingMinBidPricePerc;
        private static string ScriptBannerCreativestr;
        private static string _WebAPIHostAdServer;
        

        public static string WebAPIHostAdServer
        {
            get
            {
                if (_WebAPIHostAdServer == null)
                {

                    _WebAPIHostAdServer = System.Configuration.ConfigurationManager.AppSettings["WebAPIHostAdServer"];
                }

                return _WebAPIHostAdServer;
            }
        }
        public static Id64Generator Id64Generator
        {
            get
            {
                if (_id64Generator == null)
                {

                    _id64Generator = new Id64Generator(int.Parse(System.Configuration.ConfigurationManager.AppSettings["HostId"]));
                }

                return _id64Generator;
            }
    }

public static bool IsAdminOnly
        {
            get
            {
                // get is the logged in user is admin
                return OperationContext.Current.CurrentPrincipal.IsInRole("Administrator");
            }
        }

          
          
        public static bool IsAdmin
        {
            get
            {
                // get is the logged in user is admin
               return (OperationContext.Current.CurrentPrincipal.IsInRole("AdOps")) || (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator") || OperationContext.Current.CurrentPrincipal.IsInRole("AccountManager") );
            }
        }
        public static bool IsAdminOrAdOps
        {
            get
            {
                // get is the logged in user is admin
                return (OperationContext.Current.CurrentPrincipal.IsInRole("AdOps")) || (OperationContext.Current.CurrentPrincipal.IsInRole("Administrator"));
            }
        }


        public static string OpenX
        {
            get
            {
                if (string.IsNullOrWhiteSpace(openx))
                {
                    openx = ConfigurationManager.GetConfigurationSetting(null, null, "OpenX");
                }
                return openx;
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

        //
        public static bool SendAdServerEmails
        {
            get
            {
                if (!sendAdServerEmails.HasValue)
                {
                    var _sendAdServerEmails = false;
                    bool.TryParse(ConfigurationManager.GetConfigurationSetting(null, null, "SendAdServerEmails"), out _sendAdServerEmails);
                    sendAdServerEmails = _sendAdServerEmails;
                }
                return sendAdServerEmails.Value;
            }
        }

        public static string EventBrokerEmail
        {
            get
            {
                if (string.IsNullOrWhiteSpace(eventBrokerEmail))
                {
                    eventBrokerEmail = ConfigurationManager.GetConfigurationSetting(null, null, "EventBrokerEmail");
                }
                return eventBrokerEmail;
            }
        }

        public static string FinanceEmail
        {
            get
            {
                if (string.IsNullOrWhiteSpace(financeEmail))
                {
                    financeEmail = ConfigurationManager.GetConfigurationSetting(null, null, "FinanceEmail");
                }
                return financeEmail;
            }
        }

        public static string AdOpsEmail
        {
            get
            {
                if (string.IsNullOrWhiteSpace(adOpsEmail))
                {
                    adOpsEmail = ConfigurationManager.GetConfigurationSetting(null, null, "AdOpsEmail");
                }
                return adOpsEmail;
            }
        }
        public static string EmailAdmin
        {
            get
            {
                if (string.IsNullOrWhiteSpace(adminEmail))
                {
                    adminEmail = ConfigurationManager.GetConfigurationSetting(null, null, "EmailAdmin");
                }
                return adminEmail;
            }
        }
        public static string FtpBaseDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ftpBaseDirectory))
                {
                    ftpBaseDirectory = ConfigurationManager.GetConfigurationSetting(null, null, "ftpBaseDirectory");
                }
                return ftpBaseDirectory;
            }
        }
        public static string CdnBaseUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(cdnBaseUrl))
                {
                    cdnBaseUrl = ConfigurationManager.GetConfigurationSetting(null, null, "cdnBaseUrl");
                }
                return cdnBaseUrl;
            }
        }

        public static string NormalExpandableRichMediaTemplate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(normalExpandableRichMediaTemplate))
                {
                    normalExpandableRichMediaTemplate = ConfigurationManager.GetConfigurationSetting(null, null, "NormalExpandableRichMediaTemplate");
                }
                return normalExpandableRichMediaTemplate;
            }
        }

        public static string HTML5PreTagScript
        {
            get
            {
                if (string.IsNullOrWhiteSpace(HTML5PreTagScriptstr))
                {
                    HTML5PreTagScriptstr = ConfigurationManager.GetConfigurationSetting(null, null, "HTML5PreTagScript");
                }
                return HTML5PreTagScriptstr;
            }
        }
        public static string BannerCreativeScript
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ScriptBannerCreativestr))
                {
                    ScriptBannerCreativestr = ConfigurationManager.GetConfigurationSetting(null, null, "ScriptBannerCreative");
                }
                return ScriptBannerCreativestr;
            }
        }
        public static string MRAID1ExpandableRichMediaTemplate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(mRAID1ExpandableRichMediaTemplate))
                {
                    mRAID1ExpandableRichMediaTemplate = ConfigurationManager.GetConfigurationSetting(null, null, "MRAID1ExpandableRichMediaTemplate");
                }
                return mRAID1ExpandableRichMediaTemplate;
            }
        }

        public static string MRAID2ExpandableRichMediaTemplate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(mRAID2ExpandableRichMediaTemplate))
                {
                    mRAID2ExpandableRichMediaTemplate = ConfigurationManager.GetConfigurationSetting(null, null, "MRAID2ExpandableRichMediaTemplate");
                }
                return mRAID2ExpandableRichMediaTemplate;
            }
        }

        public static string ClickMacro
        {
            get
            {
                if (string.IsNullOrWhiteSpace(clickMacro))
                {
                    clickMacro = ConfigurationManager.GetConfigurationSetting(null, null, "ClickMacro");
                }
                return clickMacro;
            }
        }

        public static string ImpressionMacro
        {
            get
            {
                if (string.IsNullOrWhiteSpace(impressionMacro))
                {
                    impressionMacro = ConfigurationManager.GetConfigurationSetting(null, null, "ImpressionMacro");
                }
                return impressionMacro;
            }
        }

        public static string ClickRedirectMacro
        {
            get
            {
                if (string.IsNullOrWhiteSpace(clickRedirectMacro))
                {
                    clickRedirectMacro = ConfigurationManager.GetConfigurationSetting(null, null, "ClickRedirectMacro");
                }
                return clickRedirectMacro;
            }
        }

        public static string ImpressionRedirectMacro
        {
            get
            {
                if (string.IsNullOrWhiteSpace(impressionRedirectMacro))
                {
                    impressionRedirectMacro = ConfigurationManager.GetConfigurationSetting(null, null, "ImpressionRedirectMacro");
                }
                return impressionRedirectMacro;
            }
        }


        public static List<string> ImpressionMacrosList
        {
            get
            {
                if (impressionMacrosList == null)
                {
                    string impressionMacrosString = ConfigurationManager.GetConfigurationSetting(null, null, "ImpressionMacrosList");
                    impressionMacrosList = impressionMacrosString.Split(';').Where(p => !string.IsNullOrEmpty(p)).ToList();
                }
                return impressionMacrosList;
            }
        }

        public static List<string> ClickMacrosList
        {
            get
            {
                if (clickMacrosList == null)
                {
                    string clickMacrosString = ConfigurationManager.GetConfigurationSetting(null, null, "ClickMacrosList");
                    clickMacrosList = clickMacrosString.Split(';').Where(p => !string.IsNullOrEmpty(p)).ToList();
                }
                return clickMacrosList;
            }
        }

        public static string ImpressionImageTemplate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(impressionImageTemplate))
                {
                    impressionImageTemplate = ConfigurationManager.GetConfigurationSetting(null, null, "ImpressionImageTemplate");
                }
                return impressionImageTemplate;
            }
        }

        public static string Doubleclick
        {
            get
            {
                if (string.IsNullOrWhiteSpace(doubleclick))
                {
                    doubleclick = ConfigurationManager.GetConfigurationSetting(null, null, "Doubleclick");
                }
                return doubleclick;
            }
        }

        public static string MediaPlex
        {
            get
            {
                if (string.IsNullOrWhiteSpace(mediaplex))
                {
                    mediaplex = ConfigurationManager.GetConfigurationSetting(null, null, "MediaPlex");
                }
                return mediaplex;
            }
        }
        public static string Celtra
        {
            get
            {
                if (string.IsNullOrWhiteSpace(celtra))
                {
                    celtra = ConfigurationManager.GetConfigurationSetting(null, null, "Celtra");
                }
                return celtra;
            }
        }
        public static string Crisp
        {
            get
            {
                if (string.IsNullOrWhiteSpace(crispa))
                {
                    crispa = ConfigurationManager.GetConfigurationSetting(null, null, "Crisp");
                }
                return crispa;
            }
        }
        public static string Sizmik
        {
            get
            {
                if (string.IsNullOrWhiteSpace(sizmik))
                {
                    sizmik = ConfigurationManager.GetConfigurationSetting(null, null, "Sizmik");
                }
                return sizmik;
            }
        }

        public static string Vicinity
        {
            get
            {
                if (string.IsNullOrWhiteSpace(vicinity))
                {
                    vicinity = ConfigurationManager.GetConfigurationSetting(null, null, "vicinity");
                }
                return vicinity;
            }
        }
        public static string AtlaSoft
        {
            get
            {
                if (string.IsNullOrWhiteSpace(atlaSoft))
                {
                    atlaSoft = ConfigurationManager.GetConfigurationSetting(null, null, "atlaSoft");
                }
                return atlaSoft;
            }
        }
        public static string AdForm
        {
            get
            {
                if (string.IsNullOrWhiteSpace(adform))
                {
                    adform = ConfigurationManager.GetConfigurationSetting(null, null, "AdForm");
                }
                return adform;
            }
        }
        public static string Adlit
        {
            get
            {
                if (string.IsNullOrWhiteSpace(adlit))
                {
                    adlit = ConfigurationManager.GetConfigurationSetting(null, null, "adlit");
                }
                return adlit;
            }
        }

        public static string FlashTalking
        {
            get
            {
                if (string.IsNullOrWhiteSpace(flashTalking))
                {
                    flashTalking = ConfigurationManager.GetConfigurationSetting(null, null, "flashTalking");
                }
                return flashTalking;
            }
        }
        public static string GoogleTag
        {
         
            get
            {
                if (string.IsNullOrWhiteSpace(googletag))
                {
                    googletag = ConfigurationManager.GetConfigurationSetting(null, null, "GoogleTag");
                }
                return googletag;
            }
        }
        

        public static string DB
        {
            get
            {
                if (string.IsNullOrWhiteSpace(db))
                {
                    db = ConfigurationManager.GetConfigurationSetting(null, null, "AdFalconDB");
                }
                return db;
            }
        }

        public static string ShortDateFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(shortDateFormat))
                {
                    shortDateFormat = ConfigurationManager.GetConfigurationSetting(null, null, "ShortDateFormat");
                }
                return shortDateFormat;
            }
        }

       
        public static DateTime ApplicationReleaseDate
        {
            get
            {
                if (!_ApplicationReleaseDate.HasValue)
                {
                    string applicationReleaseDateValue = ConfigurationManager.GetConfigurationSetting(null, null, "ApplicationReleaseDate");

                    _ApplicationReleaseDate = DateTime.ParseExact(applicationReleaseDateValue, "dd/MM/yyyy", null);
                }

                return _ApplicationReleaseDate.Value;
            }
        }

        public static int MaxAdGroupTrackingEvents
        {
            get
            {
                if (!_MaxAdGroupTrackingEvents.HasValue)
                {
                    string maxAdGroupTrackingEvents = ConfigurationManager.GetConfigurationSetting(null, null, "MaxAdGroupTrackingEvents");

                    _MaxAdGroupTrackingEvents = int.Parse(maxAdGroupTrackingEvents);
                }

                return _MaxAdGroupTrackingEvents.Value;
            }
        }
        public static int MaxAdGroupConversionEvents
        {
            get
            {
                if (!_MaxAdGroupConversionEvents.HasValue)
                {
                    string maxAdGroupTrackingEvents = ConfigurationManager.GetConfigurationSetting(null, null, "MaxAdGroupConversionEvents");

                    _MaxAdGroupConversionEvents = int.Parse(maxAdGroupTrackingEvents);
                }

                return _MaxAdGroupConversionEvents.Value;
            }
        }





        public static string StatisticsColumnPrefixName
        {
            get
            {
                if (string.IsNullOrEmpty(_StatisticsColumnPrefixName))
                {
                    _StatisticsColumnPrefixName = ConfigurationManager.GetConfigurationSetting(null, null, "StatisticsColumnPrefixName");
                }

                return _StatisticsColumnPrefixName;
            }
        }

        public static string NativeAdCreativeUnitCode
        {
            get
            {
                if (string.IsNullOrEmpty(_NativeAdCreativeUnitCode))
                {
                    _NativeAdCreativeUnitCode = ConfigurationManager.GetConfigurationSetting(null, null, "NativeAdCreativeUnitCode");
                }

                return _NativeAdCreativeUnitCode;
            }
        }
        public static string CreativeUnitPrefixNameFormatClickTrackerURL
        {
            get
            {
                if (string.IsNullOrEmpty(_CreativeUnitPrefixNameFormatClickTrackerURL))
                {
                    _CreativeUnitPrefixNameFormatClickTrackerURL = ConfigurationManager.GetConfigurationSetting(null, null, "CreativeUnitPrefixNameFormatClickTrackerURL");
                }

                return _CreativeUnitPrefixNameFormatClickTrackerURL;
            }
        }

        public static int TrackingEvent_ValidForSecondMax
        {
            get
            {
                if (!trackingEvent_ValidForSecondMax.HasValue)
                {
                    trackingEvent_ValidForSecondMax = int.Parse(ConfigurationManager.GetConfigurationSetting(null, null, "TrackingEvent_ValidForSecondMax"));
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
                    trackingEvent_ValidForSecondMin = int.Parse(ConfigurationManager.GetConfigurationSetting(null, null, "TrackingEvent_ValidForSecondMin"));
                }

                return trackingEvent_ValidForSecondMin.Value;
            }
        }

        public static int AdFalconPortalProduceBatchTimeOutKafka
        {
            get
            {
                if (!t_AdFalconPortalProduceBatchTimeOutKafka.HasValue)
                {
                    t_AdFalconPortalProduceBatchTimeOutKafka = int.Parse(ConfigurationManager.GetConfigurationSetting(null, null, "AdFalconPortalProduceBatchTimeOutKafka"));
                }

                return t_AdFalconPortalProduceBatchTimeOutKafka.Value;
            }
        }

        //public static HttpHost[] AdFalconPortalProduceKafkaServerList
        //{
        //    get
        //    {
        //        if (KafkaServerList==null )
        //        {
        //            KafkaServerList = HttpHostUtil.ParseConnectionString(ConfigurationManager.GetConfigurationSetting(null, null, "AdFalconPortalProduceKafkaServerList"));
        //        }

        //        return KafkaServerList;
        //    }
        //}
        public static IEventPublisher KafkaEventPublisher
        {
            get
            {
                if (_KafkaEventPublisher == null)
                {
                    _KafkaEventPublisher = EventPublisher.Create("netcore31_distributed_events", System.Configuration.ConfigurationManager.AppSettings["HostId"].ToString(), "event.pubsub");
           
                }

                return _KafkaEventPublisher;
            }
        }

        public static bool KafkaEnabled
        {
            get
            {
                if (_KafkaEnabled == null)
                {
                    _KafkaEnabled= bool.Parse(ConfigurationManager.GetConfigurationSetting(null, null, "AdFalconPortalKafkaEnabled"));
                }

                return _KafkaEnabled.Value;
            }
        }


        public static decimal DynamicBiddingDefaultBidPricePerc
        {
            get
            {
                if (!_DynamicBiddingDefaultBidPricePerc.HasValue)
                {
                    _DynamicBiddingDefaultBidPricePerc = decimal.Parse(ConfigurationManager.GetConfigurationSetting(null, null, "DynamicBiddingDefaultBidPricePerc"));
                }

                return _DynamicBiddingDefaultBidPricePerc.Value;
            }
        }


        public static decimal DynamicBiddingDefaultBidStep
        {
            get
            {
                if (!_DynamicBiddingDefaultBidStep.HasValue)
                {
                    _DynamicBiddingDefaultBidStep = decimal.Parse(ConfigurationManager.GetConfigurationSetting(null, null, "DynamicBiddingDefaultBidStep"));
                }

                return _DynamicBiddingDefaultBidStep.Value;
            }
        }

        public static decimal DynamicBiddingMinBidPricePerc
        {
            get
            {
                if (!_DynamicBiddingMinBidPricePerc.HasValue)
                {
                    _DynamicBiddingMinBidPricePerc = decimal.Parse(ConfigurationManager.GetConfigurationSetting(null, null, "DynamicBiddingMinBidPricePerc"));
                }

                return _DynamicBiddingMinBidPricePerc.Value;
            }
        }

    }
}
