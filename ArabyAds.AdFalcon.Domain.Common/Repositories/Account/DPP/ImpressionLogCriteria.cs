using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.DPP;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign
{
    [ProtoContract]
    public class ImpressionLogCriteria 
    {
        [ProtoMember(1)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(2)]
        public DateTime? DataTo { get; set; }
        [ProtoMember(3)]
        public int DataFromInt { get; set; }
        [ProtoMember(4)]
        public int DataToInt { get; set; }
        [ProtoMember(5)]
        public int? Page { get; set; }
        [ProtoMember(6)]
        public int Size { get; set; }

        [ProtoMember(7)]
        public ImpressionLogType Type { get; set; }
        [ProtoMember(8)]
        public string Name { get; set; }
        [ProtoMember(9)]
        public int? DataProviderId { get; set; }



    }

}
