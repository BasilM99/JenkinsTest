using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Account.PMP
{
   [ProtoContract]
    public class PMPDealCriteria 
    {
       [ProtoMember(1)]
        public IList<int> Geographies { get; set; }

       [ProtoMember(2)]
        public IList<int> AdFormats { get; set; }

        [ProtoMember(3)]
        public List<int> ExchangeFiltred { get; set; }
        [ProtoMember(4)]
        public IList<int> AdSizes { get; set; }
        [ProtoMember(5)]
        public string PublisherName { get; set; }
        [ProtoMember(6)]
        public bool? Archived { get; set; }
        [ProtoMember(7)]
        public int AccountId { get; set; }
        [ProtoMember(8)]
        public int? PublisherId { get; set; }
        [ProtoMember(9)]
        public int? AdvertiserId { get; set; }

        [ProtoMember(10)]
        public int? AdvertiserAccountId { get; set; }
        [ProtoMember(11)]
        public int? ExchangeId { get; set; }
        [ProtoMember(12)]
        public int? userId { get; set; }
        [ProtoMember(13)]
        public bool IsPrimaryUser { get; set; }

        [ProtoMember(14)]
        public bool IsGlobal { get; set; }

        [ProtoMember(15)]
        public bool OnlyGlobal { get; set; }

        [ProtoMember(16)]
        public bool ShowAdvertiser { get; set; }
        [ProtoMember(17)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(18)]
        public DateTime? DateTo { get; set; }
        [ProtoMember(19)]
        public DealType Type { get; set; }
        //public int? StatusId { get; set; }
        [ProtoMember(20)]
        public int? Page { get; set; }
        [ProtoMember(21)]
        public int Size { get; set; }
        [ProtoMember(22)]
        public int? AppSiteId { get; set; }
        [ProtoMember(23)]
        public string Name { get; set; }
        [ProtoMember(24)]
        public bool OnlyMyGlobal { get; set; }

        public PMPDealCriteria()
        {
          //  Type = DealType.PrivateAuction;
        }
 
    }

}
