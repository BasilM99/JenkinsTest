using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{

    [ProtoContract]
    public class ReportCriteriaSchedulerDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public int? UserId { get; set; }
       [ProtoMember(3)]
        public int? AccountId { get; set; }
       [ProtoMember(4)]
        public string Criteria { get; set; }
       [ProtoMember(5)]
        public string Name { get; set; }
       [ProtoMember(6)]
        public ReportSectionType SectionType { get; set; }

       [ProtoMember(7)]
        public ReportCriteriaScope ReportScope { get; set; }
       [ProtoMember(8)]
        public DateTime CreationDate { get; set; }

    }
}
