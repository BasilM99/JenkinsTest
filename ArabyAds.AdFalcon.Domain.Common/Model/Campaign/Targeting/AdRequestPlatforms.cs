using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Targeting
{
    //public class AdRequestPlatform : LookupBase<AdRequestPlatform, int>
    //{
    //    public virtual string Code { get; set; }
    //}
    [ProtoContract]
    public class AudicanceBillSummary
    {
        [ProtoMember(1)]
        public decimal MinValue { get; set; }

        [ProtoMember(2)]
        public decimal MaxValue { get; set; }
    }
}
