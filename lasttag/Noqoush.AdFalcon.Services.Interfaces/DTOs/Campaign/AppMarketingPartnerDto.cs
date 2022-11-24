
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AppMarketingPartnerDto : LookupDto
    {
        [DataMember]
        public virtual string Code { get; set; }
        [DataMember]
        public virtual  string Description { get; set; }
   

    
        [DataMember]
        public virtual IEnumerable<AppMarketingPartnerTrackerDto> Trackers { get; set; }
    }

    
    [DataContract]
    public class AppMarketingPartnerTrackerDto {


        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public virtual int TypeID { get; set; }

        [DataMember]
        public PlatformDto Platform { get; set; }
        [DataMember]
        public virtual int? AdGroupID { get; set; }


        

                    [DataMember]
        public virtual string TrackerUrlTemplate { get; set; }
        [DataMember]
        public virtual string ClickTrackerUrlTemplate { get; set; }

        [DataMember]
        public virtual string EventPostbackUrlTemplate { get; set; }
    
    }
}
