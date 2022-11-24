using System.Collections.Generic;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.Framework;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class LookupEntry
    {
        public string View { get; set; }
        public string FilterView { get; set; }
        public string DispalyName { get; set; }
        public string SearchAction { get; set; }
        public string GroupKey { get; set; }
        public int DialogWidth { get; set; }
        public int DialogHeight { get; set; }
    }


    public static class LookupEntries
    {
        private static readonly object syncObj = new object();
        private const string DefaultView = "";
        private const string DefaultSearchAction = "_Index";
        private const string DefaultFilterView = "Filter";
        private const string DeviceFilterView = "DeviceFilter";
        private const string DeviceView = "Device";
        private const string DeviceSearchAction = "_IndexDevice";
        private const string CostElementView = "CostElement";
        private const string FeeView = "Fee";
        private const string ManufacturerView = "Manufacturer";
        private const string PlatformView = "Platform";
        private const string AdvertiserView = "Advertiser";
        private const string KeywordView = "Keyword";
        private const string LocationView = "Location";
        private const string LanguageView = "Language";

        private const string AttributeView = "Attribute";
        private const string DeviceCapabilityView = "DeviceCapability";
        public const string AgeGroupView = "AgeGroup";
        public const string ActionTypeView = "ActionType";
        public const string DeviceTypeView = "DeviceType";
        public const string GeographicTypeView = "GeographicType";
        public const string AudienceSegmentView = "AudienceSegment";

        private static Dictionary<string, LookupEntry> _Lookups = null;

        public static Dictionary<string, LookupEntry> Lookups
        {
            get
            {
                if (_Lookups == null)
                {
                    lock (syncObj)
                    {
                        if (_Lookups == null)
                        {
                            _Lookups = new Dictionary<string, LookupEntry>()
                                           {
                                               {
                                                   LookupNames.Device,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Device",
                                                           View = DeviceView,
                                                           FilterView = DeviceFilterView,
                                                           SearchAction = DeviceSearchAction,
                                                           GroupKey="Device"
                                                       }
                                                   },
                                               {
                                                   LookupNames.Currency,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Currency",
                                                           View = DefaultView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="Currency"
                                                       }
                                                   },
                                                  {
                                                   LookupNames.CreativeVendor,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Creative Vendor",
                                                           View = "CreativeVendor",
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="CreativeVendor"
                                                       }
                                                   },       {
                                                   LookupNames.impressionmetric,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Performance Metric",
                                                           View = DefaultView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="ImpressionMetric"
                                                       }
                                                   },

                                               {
                                                   LookupNames.Manufacturer,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Manufacturer",
                                                           View = ManufacturerView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="Manufacturer"
                                                       }
                                                   },
                                               {
                                                   LookupNames.Platform,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Platform",
                                                           View = PlatformView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="Platform"
                                                       }
                                                   },
                                                    {
                                                   LookupNames.Advertiser,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Advertiser",
                                                           View = AdvertiserView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="Advertiser"
                                                       }
                                                   },
                                               {
                                                   LookupNames.Keyword,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Keyword",
                                                           View = KeywordView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="keyword"
                                                       }
                                                   },
                                                   {
                                                   LookupNames.language,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Language",
                                                           View = LanguageView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="Language"
                                                       }
                                                   },
                                {  LookupNames.viewabilityvendor,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "ViewAbilityVendor",
                                                           View = DefaultView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="ViewAbilityVendor"
                                                       }
                                                   },

                                               {
                                                   LookupNames.LocationBase,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Location",
                                                           View = LocationView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="Location"
                                                       }
                                                   },
                                               {
                                                   LookupNames.Operator,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Operator",
                                                           View = DefaultView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="Operator"
                                                       }
                                                   },
                                               {
                                                   LookupNames.DeviceCapability,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Device Capability",
                                                           View = DeviceCapabilityView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="DeviceCapability"
                                                       }
                                                   },
                                               {
                                                   LookupNames.CostElement,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Cost Element",
                                                           View = CostElementView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="CostElement"
                                                       }
                                                   },     { LookupNames.Fee,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Fee",
                                                           View = FeeView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey="Fee"
                                                       }
                                                   },
                                                    {
                                                   LookupNames.JobPosition,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Job Position",
                                                           View = DefaultView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey = "Job Position"
                                                       }
                                                       },
                                                   //          {
                                                   //LookupNames.AgeGroup,
                                                   //new LookupEntry
                                                   //    {
                                                   //        DispalyName = "Age Group",
                                                   //        View = DefaultView,
                                                   //        FilterView = DefaultFilterView,
                                                   //        SearchAction = DefaultSearchAction,
                                                   //        GroupKey = "Age Group"
                                                   //    }
                                                   //    },

                                                       {
                                                       LookupNames.Attributes,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Attributes",
                                                           View = DefaultView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey = "Attributes"
                                                       }
                                                       }
                                                       ,
                                                   //   {
                                                   //    LookupNames.ActionType,
                                                   //new LookupEntry
                                                   //    {
                                                   //        DispalyName = "Action Type",
                                                   //        View = DefaultView,
                                                   //        FilterView = DefaultFilterView,
                                                   //        SearchAction = DefaultSearchAction,
                                                   //        GroupKey = "Action Type"
                                                   //    }
                                                   //    } ,
                                                      {
                                                       LookupNames.DeviceType,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Device Type",
                                                           View = DefaultView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey = "Device Type"
                                                       }
                                                       },
                                                      {
                                                       LookupNames.Geographic,
                                                   new LookupEntry
                                                       {
                                                           DispalyName = "Geographic",
                                                           View = DefaultView,
                                                           FilterView = DefaultFilterView,
                                                           SearchAction = DefaultSearchAction,
                                                           GroupKey = "Geographic"
                                                       }}




                                           };
                        }
                    }
                }
                return _Lookups;
            }
        }

        public static string FindLookupView(string lookupName)
        {
            if (Lookups.ContainsKey(lookupName))
            {
                return Lookups[lookupName].View;
            }
            else
            {
                return null;
            }
        }
        public static string FindLookupGroupKey(string lookupName)
        {
            if (Lookups.ContainsKey(lookupName))
            {
                return Lookups[lookupName].GroupKey;
            }
            else
            {
                return null;
            }
        }

        public static string FindLookupFilterView(string lookupName)
        {
            if (Lookups.ContainsKey(lookupName))
            {
                return Lookups[lookupName].FilterView;
            }
            else
            {
                return null;
            }
        }

        public static TRepository FindLookupRepository<TRepository>()
            where TRepository : class
        {
            //if (Lookups.ContainsKey(lookupName))
            //{
            return IoC.Instance.Resolve<TRepository>();
            ;
            /* }
             else
             {
                 return null;
             }*/
        }
    }
}
