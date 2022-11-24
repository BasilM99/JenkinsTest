
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AppMarketingPartnerDto : LookupDto
    {
       [ProtoMember(1)]
        public virtual string Code { get; set; }
       [ProtoMember(2)]
        public virtual  string Description { get; set; }
   

    
       [ProtoMember(3)]
        public virtual IEnumerable<AppMarketingPartnerTrackerDto> Trackers { get; set; } = new List<AppMarketingPartnerTrackerDto>();
    }

    
    [ProtoContract]
    public class AppMarketingPartnerTrackerDto {


       [ProtoMember(1)]
        public virtual int ID { get; set; }
       [ProtoMember(2)]
        public virtual int TypeID { get; set; }

       [ProtoMember(3)]
        public PlatformDto Platform { get; set; }
       [ProtoMember(4)]
        public virtual int? AdGroupID { get; set; }


        

                   [ProtoMember(5)]
        public virtual string TrackerUrlTemplate { get; set; }
       [ProtoMember(6)]
        public virtual string ClickTrackerUrlTemplate { get; set; }

       [ProtoMember(7)]
        public virtual string EventPostbackUrlTemplate { get; set; }
    
    }
}
