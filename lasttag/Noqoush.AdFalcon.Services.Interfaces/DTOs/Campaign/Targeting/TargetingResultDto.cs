using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    
        [DataContract]
        public class TargetingResultDto
    {
        
                    [DataMember]
        public int CampId { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public bool PMPDealInventorySourceConflicts { get; set; }

        [DataMember]
            public bool Result { get; set; }

        [DataMember]
        public bool PMPDealConfictCountries { get; set; }
        [DataMember]
        public bool PMPDealConfictWithInventorySource { get; set; }
        [DataMember]
        public bool PMPDealConfictAdType { get; set; }
        [DataMember]
        public bool PMPDealConfictPrice{ get; set; }

        [DataMember]
        public bool AddDefaultCostElement { get; set; }
        [DataMember]
        public bool AddDefaultFee { get; set; }

        [DataMember]
        public bool InventroySourceAllowGeofencing { get; set; }

        [DataMember]
        public bool DealAllowGeofencing { get; set; }

        [DataMember]
        public bool AdminLessThanMinBid { get; set; }

        [DataMember]
        public bool FireEvents { get; set; }
        [DataMember]
        public List<int> IdsDiffrent { get; set; }
        [DataMember]
        public int CountExternalAudienceList { get; set; }
        [DataMember]
        public string DataPriceAudienceSegment { get; set; }


        [DataMember]
        public List<int> IdsAdd { get; set; }

    }
}
