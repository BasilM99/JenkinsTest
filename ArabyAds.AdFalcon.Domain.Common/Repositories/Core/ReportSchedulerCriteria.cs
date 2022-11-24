
using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Core
{
 
    [ProtoContract]
    public class ReportSchedulerCriteria 
    {
        [ProtoMember(1)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(2)]
        public DateTime? DateTo { get; set; }

        [ProtoMember(3)]
        public int? Page { get; set; }
        [ProtoMember(4)]
        public int Size { get; set; }
        [ProtoMember(5)]
        public int AccountId { get; set; }

        [ProtoMember(6)]
        public int? UserId { get; set; }
        [ProtoMember(7)]
        public  ReportSectionType ReportSectionType { get; set; }

    }

    [ProtoContract]
    public class ReportJsonCriteria 
    {

        [ProtoMember(1)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(2)]
        public DateTime? DateTo { get; set; }

        [ProtoMember(3)]
        public int? Page { get; set; }
        [ProtoMember(4)]
        public int Size { get; set; }
        [ProtoMember(5)]
        public int AccountId { get; set; }

        [ProtoMember(6)]
        public int? UserId { get; set; }
        [ProtoMember(7)]
        public ReportSectionType ReportSectionType { get; set; }

    }
}
