using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Performance;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
   
    public class ThirdPartyTrackerMapping : ClassMap<ThirdPartyTracker>
    {
        public ThirdPartyTrackerMapping()
        {
            Table("video_ad_third_party_om_verifications");
            Id(p => p.ID,"Id").GeneratedBy.Identity();
            Map(p => p.ParametersURL, "UrlParameters");
            Map(p => p.ExecutionErrorTrackerURL, "ExecutionErrorTracker");

            Map(p => p.ScriptURL, "ScriptUrl");
            Map(p => p.VendorID, "VendorId");


            Map(p => p.IsDeleted);
            References(p => p.Creative, "VideoAdId");

        }
    }


}
