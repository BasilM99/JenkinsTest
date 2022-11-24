using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.Device;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{


    public class TargetingMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.TargetingBase>
    {
        public TargetingMapping()
        {
            Table("targetings");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,MappingSettings._maxLo, "TableKey = 'Targeting'");
            //Map(x => x.Type, "TypeId");
            References(x => x.Type, "TypeId");
            //Map(x => x.GroupId);
            References(x => x.AdGroup, "AdGroupId");
            Map(x => x.IsDeleted);
        }
    }

    //public class AdPositionTargetingMapping : SubclassMap<AdPositionTargeting>
    //{
    //    public AdPositionTargetingMapping()
    //    {
    //        Table("ad_position_targetings");
    //        KeyColumn("Id");
    //        Map(X => X.PagePositionEnabled, "PagePositionEnabled");
     
    //        HasManyToMany(p => p.AdPositions).Table("ad_targeting_positions").ParentKeyColumn("AdPositionTargetingId").ChildKeyColumn("AdPositionId").Cascade.All().Fetch.Select();
         
   
    //    }
    //}

    public class VideoTargetingMapping : SubclassMap<VideoTargeting>
    {
        public VideoTargetingMapping()
        {
            Table("video_targetings");
            KeyColumn("Id");
            Map(X => X.MatchOrientation, "MatchDeviceOrientation");
            Map(X => X.RewardedAds, "AllowRewardedAd");
            Map(X => X.RewardedAdOnly, "RewardedAdOnly");
            HasManyToMany(p => p.InStreamPositions).Table("video_targeting_instream_positions").ParentKeyColumn("VideoTargetingId").ChildKeyColumn("InstreamPositionId").Cascade.All().Fetch.Select();
            HasManyToMany(p => p.PlacementTypes).Table("video_targeting_placement_types").ParentKeyColumn("VideoTargetingId").ChildKeyColumn("PlacementTypeId").Cascade.All().Fetch.Select();
            HasManyToMany(p => p.SkippableAds).Table("video_targeting_skippable_ad_options").ParentKeyColumn("VideoTargetingId").ChildKeyColumn("SkippableAdOptionId").Cascade.All().Fetch.Select();
            HasManyToMany(p => p.PlayBackMethods).Table("video_targeting_playback_methods").ParentKeyColumn("VideoTargetingId").ChildKeyColumn("PlaybackMethodId").Cascade.All().Fetch.Select();

            //HasMany(x => x.InStreamPositions).KeyColumn("DeviceTargetingId").Cascade.AllDeleteOrphan().Inverse();
            //HasMany(x => x.PlacementTypes).KeyColumn("DeviceTargetingId").Cascade.AllDeleteOrphan().Inverse();
            //HasMany(x => x.SkippableAds).KeyColumn("DeviceTargetingId").Cascade.AllDeleteOrphan().Inverse();
            //HasMany(x => x.PlayBackMethods).KeyColumn("DeviceTargetingId").Cascade.AllDeleteOrphan().Inverse();
            //  References(x => x.Language, "LanguageId");
        }
    }
    public class LanguageTargetingMapping : SubclassMap<LanguageTargeting>
    {
        public LanguageTargetingMapping()
        {
            Table("language_targetings");
            KeyColumn("Id");
            References(x => x.Language, "LanguageId");
        }
    }
    public class KeywordTargetingMapping : SubclassMap<KeywordTargeting>
    {
        public KeywordTargetingMapping()
        {
            Table("keywordtargetings");
            KeyColumn("Id");
            References(x => x.Keyword, "keywordId");

            Map(M=>M.Include);
        }
    }

    public class AdPMPDealTargetingMapping : SubclassMap<AdPMPDealTargeting>
    {
        public AdPMPDealTargetingMapping()
        {
            Table("buyer_deal_targetings");
            KeyColumn("Id");
            References(x => x.Deal, "DealId");
        }
    }

    public class DemographicTargetingMapping : SubclassMap<DemographicTargeting>
    {
        public DemographicTargetingMapping()
        {
            Table("demographictargeting");
            KeyColumn("Id");
            Component(x => x.Demographic, m =>
            {
                m.References(x => x.Gender, "GenderId");
                m.References(x => x.AgeGroup, "AgeGroupId");
            });
        }
    }

    public class GeographicTargetingMapping : SubclassMap<GeographicTargeting>
    {
        public GeographicTargetingMapping()
        {
            Table("geographictargetings");
            KeyColumn("Id");
            References(x => x.Location, "LocationId");
        }
    }
    public class OperatorTargetingMapping : SubclassMap<OperatorTargeting>
    {
        public OperatorTargetingMapping()
        {
            Table("operatortargetings");
            KeyColumn("Id");
            References(x => x.Operator, "OperatorId");
        }
    }

    public class IPTargetingMapping : SubclassMap<IPTargeting>
    {
        public IPTargetingMapping()
        {
            Table("ip_targetings");
            KeyColumn("Id");
            Map(x => x.StartRange, "IPRangeStart");
            Map(x => x.EndRange, "IPRangeEnd");
            Map(x => x.Description, "description");
        }
    }

    public class PlatformTargetingMapping : ClassMap<PlatformTargeting>
    {
        public PlatformTargetingMapping()
        {
            Table("devicetargetingplatforms");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'PlatformTargeting'");
            Map(x => x.IsAll);
            Map(x => x.MinimumVersion, "PlatformMinimumVersion");
            References(x => x.Platform, "PlatformId");
            References(x => x.DeviceTargeting, "DeviceTargetingId");

        }
    }

    public class ManufacturerTargetingMapping : ClassMap<ManufacturerTargeting>
    {
        public ManufacturerTargetingMapping()
        {
            Table("devicetargetingmanufacturers");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'ManufacturerTargeting'");
            Map(x => x.IsAll);
            References(x => x.Manufacturer, "ManufacturerId");
            References(x => x.DeviceTargeting, "DeviceTargetingId");
        }
    }

    public class ModelTargetingMapping : ClassMap<ModelTargeting>
    {
        public ModelTargetingMapping()
        {
            Table("devicetargetingdevices");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'ModelTargeting'");
            //References(x => x.AdGroup, "GroupId");
            References(x => x.Device, "DeviceId");
            References(x => x.DeviceTargeting, "DeviceTargetingId");

        }
    }

    public class DeviceTypeTargetingMapping : ClassMap<DeviceTypeTargeting>
    {
        public DeviceTypeTargetingMapping()
        {
            Table("device_targeting_type");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'DeviceTypeTargeting'");
            //References(x => x.AdGroup, "GroupId");
            References(x => x.DeviceType, "DeviceTypeId");
            References(x => x.DeviceTargeting, "DeviceTargetingId");

        }
    }


    public class DeviceCapabilityTargetingMapping : ClassMap<DeviceCapabilityTargeting>
    {
        public DeviceCapabilityTargetingMapping()
        {
            Table("device_targeting_device_capabilities");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'DeviceCapabilityTargeting'");
            Map(x => x.IsInclude, "Include");
            References(x => x.Capability, "CapabilityId");
            References(x => x.DeviceTargeting, "DeviceTargetingId");

        }
    }

    public class DeviceTargetingMapping : SubclassMap<DeviceTargeting>
    {
        public DeviceTargetingMapping()
        {
            Table("devicetargetings");
            KeyColumn("Id");
            References(x => x.TargetingType, "SubTypeId");
            Map(x => x.IsAll);

            HasMany(x => x.DeviceTypeTargetings).KeyColumn("DeviceTargetingId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.DevicesTargeting).KeyColumn("DeviceTargetingId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.ManufacturersTargeting).KeyColumn("DeviceTargetingId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.PlatformsTargeting).KeyColumn("DeviceTargetingId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.DeviceCapabilitiesTargeting).KeyColumn("DeviceTargetingId").Cascade.AllDeleteOrphan().Inverse();
        }
    }

    public class UrlTargetingMapping : SubclassMap<URLTargeting>
    {
        public UrlTargetingMapping()
        {
            Table("url_targetings");
            KeyColumn("Id");
            Map(x => x.URL, "URL");
        }
    }

    public class GeoFencingTargetingMapping : SubclassMap<GeoFencingTargeting>
    {
        public GeoFencingTargetingMapping()
        {
            Table("geofencing_targetings");
            KeyColumn("Id");
            Map(x => x.Latitude);
            Map(x => x.Longitude);
            Map(x => x.Radius);
        }
    }


    public class AdRequestTargetingMapping : SubclassMap<AdRequestTargeting>
    {
        public AdRequestTargetingMapping()
        {
            Table("adrequest_version_targetings");
            KeyColumn("Id");
            References(x => x.AdRequestType, "RequestVersionTypeId");
            References(x => x.AdRequestPlatform, "RequestVersionPlatformId");
            Map(x => x.MinimumVersion, "MinimumVersion").Nullable();

        }
    }
    public class ImpressionMetricTargetingMapping : SubclassMap<ImpressionMetricTargeting>
    {
        public ImpressionMetricTargetingMapping()
        {
            Table("impression_metric_targetings");
            KeyColumn("Id");
            References(x => x.ImpressionMetric, "ImpressionMetricId");
            References(x => x.MetricVendor, "MetricVendorId");

            Map(x => x.MinValue, "MinValue");
            Map(x => x.Ignore, "IgnoreIfNotAvailable");

        }
    }

    
    public class AudienceSegmentTargetingMapping : SubclassMap<AudienceSegmentTargeting>
    {
        public AudienceSegmentTargetingMapping()
        {
            Table("audience_segment_targetings");
            KeyColumn("Id");
            Map(x => x.IsExternal);
            References(x => x.DataProvider, "DataProviderId");
            Map(x => x.RulesJson);
            Map(x=>x.DataBid,"DataPrice");
            Map(x => x.MaxDataBid, "MaxDataPrice");
            Map(x => x.LogAdMarkup);
            Map(x => x.Category,"Options");
        }
        
    }
    public class ContextualSegmentTargetingMapping : SubclassMap<ContextualSegmentTargeting>
    {
        public ContextualSegmentTargetingMapping()
        {
            Table("contextual_segment_targetings");
            KeyColumn("Id");
            Map(x => x.Include);
            Map(x => x.IsBrandSafty);
            References(x => x.ContextualSegment, "ContextualSegmentId");
          
        }

    }

    


    public class MasterAppSiteTargetingMapping : SubclassMap<MasterAppSiteTargeting>
    {
        public MasterAppSiteTargetingMapping()
        {
            Table("content_list_targeting");
            KeyColumn("Id");
           
            References(x => x.List, "ListId");
          

        }
    }
}
