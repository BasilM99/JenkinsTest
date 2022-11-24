using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;

using System.Linq;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using System.Collections.Generic;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign
{
    [ProtoContract]
    public class AdvertiserAccountMasterAppSiteCriteria 
    {
        [ProtoMember(1)]
        public int? AccountId { get; set; }
        [ProtoMember(2)]
        public string culture { get; set; }
        [ProtoMember(3)]
        public MasterAppSiteStatus  Status { get; set; }
        [ProtoMember(4)]
        public MasterAppSiteType Type { get; set; }

        [ProtoMember(5)]
        public int? userId { get; set; }
        [ProtoMember(6)]
        public bool showActive { get; set; }
        [ProtoMember(7)]
        public bool showArchived { get; set; }
        [ProtoMember(8)]
        public bool showGlobalAndAccount { get; set; }
        [ProtoMember(9)]
        public bool showAccountAndAdvertiser { get; set; }
        [ProtoMember(10)]
        public bool IsPrimaryUser { get; set; }
        [ProtoMember(11)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(12)]
        public DateTime? DataTo { get; set; }

        [ProtoMember(13)]
        public int? Page { get; set; }
        [ProtoMember(14)]
        public int Size { get; set; }
        [ProtoMember(15)]
        public bool? GlobalScope { get; set; }
        [ProtoMember(16)]
        public string Name { get; set; }
        [ProtoMember(17)]
        public int? AdvAccountId { get; set; }

    }




    [ProtoContract]
    public class AudienceSegmentCriteria 
    {
        [ProtoMember(1)]
        public string Value { get; set; }
        [ProtoMember(2)]
        public string Culture { get; set; }
        [ProtoMember(3)]
        public int? Page { get; set; }
        [ProtoMember(4)]
        public int Size { get; set; }
        [ProtoMember(5)]
        public string Name { get; set; }
        [ProtoMember(6)]
        public int AdvAccountId { get; set; }
        [ProtoMember(7)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(8)]
        public DateTime? DataTo { get; set; }
        [ProtoMember(9)]
        public bool showArchived { get; set; }


    }
    [ProtoContract]
    public class AdvertiserAccountMasterAppSiteItemCriteria 
    {
        [ProtoMember(1)]
        public int? AccountId { get; set; }
        [ProtoMember(2)]
        public string culture { get; set; }
        [ProtoMember(3)]
        public int? StatusId { get; set; }
        [ProtoMember(4)]
        public int? userId { get; set; }
        [ProtoMember(5)]
        public MasterAppSiteItemType Type { get; set; }
        [ProtoMember(6)]
        public bool showActive { get; set; }
        [ProtoMember(7)]
        public bool showArchived { get; set; }
        [ProtoMember(8)]
        public bool IsPrimaryUser { get; set; }
        [ProtoMember(9)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(10)]
        public DateTime? DataTo { get; set; }

        [ProtoMember(11)]
        public int? Page { get; set; }
        [ProtoMember(12)]
        public int Size { get; set; }

        [ProtoMember(13)]
        public string Name { get; set; }
        [ProtoMember(14)]
        public string BundleId { get; set; }
        [ProtoMember(15)]
        public string AppSiteId { get; set; }
        [ProtoMember(16)]
        public string Domain { get; set; }
        [ProtoMember(17)]
        public int MasterListId { get; set; }

    }

    [ProtoContract]
    public class PixelCriteria 
    {

        [ProtoMember(1)]
        public int? AccountId { get; set; }
        [ProtoMember(2)]
        public string culture { get; set; }
        [ProtoMember(3)]
        public PixelStatus Status { get; set; }
        [ProtoMember(4)]
        public MasterAppSiteType Type { get; set; }

        [ProtoMember(5)]
        public int? userId { get; set; }
        [ProtoMember(6)]
        public bool showActive { get; set; }
        [ProtoMember(7)]
        public bool showArchived { get; set; }
        [ProtoMember(8)]
        public bool showGlobalAndAccount { get; set; }
        [ProtoMember(9)]
        public bool showAccountAndAdvertiser { get; set; }
        [ProtoMember(10)]
        public bool IsPrimaryUser { get; set; }
        [ProtoMember(11)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(12)]
        public DateTime? DataTo { get; set; }

        [ProtoMember(13)]
        public int? Page { get; set; }
        [ProtoMember(14)]
        public int Size { get; set; }
        [ProtoMember(15)]
        public bool? GlobalScope { get; set; }
        [ProtoMember(16)]
        public string Name { get; set; }

        [ProtoMember(17)]
        public string Value { get; set; }
        [ProtoMember(18)]
        public int? AdvAccountId { get; set; }

    }
}
