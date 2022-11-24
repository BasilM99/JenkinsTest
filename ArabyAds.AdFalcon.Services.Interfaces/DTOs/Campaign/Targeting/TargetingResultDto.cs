using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    
        [ProtoContract]
        public class TargetingResultDto
    {
        
                   [ProtoMember(1)]
        public int CampId { get; set; }
       [ProtoMember(2)]
        public string Message { get; set; }
       [ProtoMember(3)]
        public bool PMPDealInventorySourceConflicts { get; set; }

       [ProtoMember(4)]
            public bool Result { get; set; }

       [ProtoMember(5)]
        public bool PMPDealConfictCountries { get; set; }
       [ProtoMember(6)]
        public bool PMPDealConfictWithInventorySource { get; set; }
       [ProtoMember(7)]
        public bool PMPDealConfictAdType { get; set; }
       [ProtoMember(8)]
        public bool PMPDealConfictPrice{ get; set; }

       [ProtoMember(9)]
        public bool AddDefaultCostElement { get; set; }
       [ProtoMember(10)]
        public bool AddDefaultFee { get; set; }

       [ProtoMember(11)]
        public bool InventroySourceAllowGeofencing { get; set; }

       [ProtoMember(12)]
        public bool DealAllowGeofencing { get; set; }

       [ProtoMember(13)]
        public bool AdminLessThanMinBid { get; set; }

       [ProtoMember(14)]
        public bool FireEvents { get; set; }
       [ProtoMember(15)]
        public List<int> IdsDiffrent { get; set; }
       [ProtoMember(16)]
        public int CountExternalAudienceList { get; set; }
       [ProtoMember(17)]
        public string DataPriceAudienceSegment { get; set; }


       [ProtoMember(18)]
        public List<int> IdsAdd { get; set; }

    }
}
