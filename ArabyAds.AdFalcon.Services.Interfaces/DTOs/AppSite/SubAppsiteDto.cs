using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class SubAppsiteDto
    {
       [ProtoMember(1)]
        public int Id { get; set; }
       [ProtoMember(2)]
        public string SubPublisherId { get; set; }
       [ProtoMember(3)]
        public string SubPublisherName { get; set; }
       [ProtoMember(4)]
        public string SubPublisherUrl { get; set; }
       [ProtoMember(5)]
        public string SubPublisherMarketId { get; set; }
       [ProtoMember(6)]
        public int AppSiteId { get; set; }
       [ProtoMember(7)]
        public int ExchangeId { get; set; }
       [ProtoMember(8)]
        public string AppSiteName { get; set; }
       [ProtoMember(9)]
        public string ExchangeName { get; set; }

       [ProtoMember(10)]
        public bool IsAllowed { get; set; }


        [ProtoMember(11)]
        public string AccountName { get; set; }

        [ProtoMember(12)]
        public int AccountId { get; set; }
       

    }
}
