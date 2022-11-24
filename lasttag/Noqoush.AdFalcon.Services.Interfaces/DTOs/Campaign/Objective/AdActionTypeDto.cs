using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective
{
    [DataContract]
    public class AdActionTypeDto : LookupDto
    {
        [DataMember]
        public IList<AdActionTypeConstraintDto> Constraints { get; set; }

        [DataMember]
        public int? ShowInAppId { get; set; }

        [DataMember]
        public List<int> CostModelWrappers { get; set; }

        [DataMember]
        public List<AdTypeDto> AdTypes { get; set; }



        [DataMember]
        public List<AdActionCostModelWrapperDto> AdActionCostModelWrappers { get; set; }

        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public bool hide { get; set; }

    }
}
