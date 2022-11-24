using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetSubAppsitesRequest
    {
        [ProtoMember(1)]
        public int AppSiteId { get; set; }
        [ProtoMember(2)]
        public string SubPublisherId { get; set; }


        [ProtoMember(3)]
        public int AccountId { get; set; }

        [ProtoMember(4)]
        public string AppSiteName { get; set; }


        [ProtoMember(5)]
        public int Page { get; set; }
        


        public override string ToString()
        {
            return $"{AppSiteId}_{SubPublisherId ?? "Null"}";
        }

    }
}
