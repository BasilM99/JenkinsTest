using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective
{
    [ProtoContract]
    public class AdActionTypeDto : LookupDto
    {
        [ProtoMember(1)]
        public IList<AdActionTypeConstraintDto> Constraints { get; set; } = new List<AdActionTypeConstraintDto>();

       [ProtoMember(2)]
        public int? ShowInAppId { get; set; }

       [ProtoMember(3)]
        public List<int> CostModelWrappers { get; set; }

       [ProtoMember(4)]
        public List<AdTypeDto> AdTypes { get; set; }



       [ProtoMember(5)]
        public List<AdActionCostModelWrapperDto> AdActionCostModelWrappers { get; set; } = new List<AdActionCostModelWrapperDto>();

        [ProtoMember(6)]
        public int Code { get; set; }

       [ProtoMember(7)]
        public bool hide { get; set; }


        [ProtoMember(8)]
        public bool ShowForObjective { get; set; }

    }
}
