using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API.Criteria
{
    [ProtoContract]
    public class AppSiteStatisticsCriteriaDto : BasePagingCriteriaDto
    {
       [ProtoMember(1)]
        public int userId { get; set; }
       [ProtoMember(2)]
        public bool IsPrimaryUser { get; set; }


       [ProtoMember(3)]
        public int SummaryBy { get; set; }

       [ProtoMember(4)]
        public string Layout { get; set; }

       [ProtoMember(5)]
        public string ItemsList { get; set; }

       [ProtoMember(6)]
        public string AdvancedCriteria { get; set; }

       [ProtoMember(7)]
        public string GroupBy { get; set; }

       [ProtoMember(8)]
        public int AccountId { get; set; }

    }
}
