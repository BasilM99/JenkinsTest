using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdGroupTrackingEventMapping : ClassMap<AdGroupTrackingEvent>
    {
        public AdGroupTrackingEventMapping()
        {
            Table("adgroup_events");
            Where("IsTracking=1");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'AdGroupTrackingEvent'");
            Map(x => x.Code);
            Map(x => x.PreRequisites,"PrerequisiteIds");
            Map(x => x.AllPreRequisitesRequired);
            Map(x => x.IsCustom);
            Map(x => x.IsBillable);
            Map(x => x.AllowDuplicate);
            Map(x => x.IsTracking);
            Map(x => x.IsConversion).Not.Update();
            Map(x => x.Description, "Name");
            Map(x => x.StatisticsColumnName, "StatColumnName");
            Map(p => p.IsDeleted);
            Map(p => p.Revenue);
            Map(p => p.IsPrimary);
            //Map(p => p.ValidFor);
            References(x => x.AdGroup, "AdGroupId");
            HasMany(x => x.PixelListsMap).KeyColumn("AdGroupEventId").Cascade.Persist().Inverse();
            HasMany(x => x.AudienceSegmentListsMap).KeyColumn("AdGroupEventId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
    public class AudienceSegmentEventMapMapping : ClassMap<AudienceSegmentEventMap>
    {
        public AudienceSegmentEventMapMapping()
        {
            Table("adgroup_event_audience_segments");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'AudienceSegmentEventMap'");
            References(x => x.Event, "AdGroupEventId");
            References(x => x.AudienceSegment, "AudienceSegmentId");

        }
    }


    public class AdGroupConversionEventMapping : ClassMap<AdGroupConversionEvent>
    {
        public AdGroupConversionEventMapping()
        {
            Table("adgroup_events");
            Where("IsConversion=1");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'AdGroupTrackingEvent'");
            Map(x => x.Code);
            //Map(x => x.PreRequisites, "PrerequisiteIds");
           // Map(x => x.AllPreRequisitesRequired);
            Map(x => x.IsCustom);
            //Map(x => x.IsBillable);
            Map(x => x.IsPrimary);
            //Map(x => x.AllowDuplicate);
            Map(x => x.Description, "Name");
            Map(x => x.StatisticsColumnName, "StatColumnName");
            Map(p => p.IsDeleted);
            Map(p => p.Revenue);
            Map(p => p.IsConversion);
            Map(p => p.IsTracking);
            //Map(p => p.ValidFor);
            References(x => x.AdGroup, "AdGroupId");
            HasMany(x => x.AudienceSegmentListsMap).KeyColumn("AdGroupEventId").Cascade.Persist().Inverse();
            HasMany(x => x.PixelListsMap).KeyColumn("AdGroupEventId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
    public class PixelEventMapMapping : ClassMap<PixelEventMap>
    {
        public PixelEventMapMapping()
        {
            Table("pixel_adgroup_events");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'PixelEventMap'");
            References(x => x.Event, "AdGroupEventId");
            References(x => x.Pixel, "PixelId");

        }
    }
    public class AdGroupEventMappingMapping : ClassMap<AdGroupEvent>
    {
        public AdGroupEventMappingMapping()
        {
            Table("adgroup_events");
           
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'AdGroupTrackingEvent'");
            Map(x => x.Code);
            //Map(x => x.PreRequisites, "PrerequisiteIds");
            // Map(x => x.AllPreRequisitesRequired);
            Map(x => x.IsCustom);
            //Map(x => x.IsBillable);
            Map(x => x.IsPrimary);
            //Map(x => x.AllowDuplicate);
            Map(x => x.Description, "Name");
            Map(x => x.StatisticsColumnName, "StatColumnName");
     
            Map(p => p.Revenue);
        
            //Map(p => p.ValidFor);
            References(x => x.AdGroup, "AdGroupId");

            HasMany(x => x.PixelListsMap).KeyColumn("AdGroupEventId").Cascade.AllDeleteOrphan().Inverse();



         
      
            Map(x => x.PreRequisites, "PrerequisiteIds");
            Map(x => x.AllPreRequisitesRequired);
  
            Map(x => x.IsBillable);
            Map(x => x.AllowDuplicate);
            Map(x => x.IsTracking);
            Map(x => x.IsConversion);
  
            Map(p => p.IsDeleted);
      
            //Map(p => p.ValidFor);
       

            HasMany(x => x.AudienceSegmentListsMap).KeyColumn("AdGroupEventId").Cascade.AllDeleteOrphan().Inverse();

        }
    }
    
}

