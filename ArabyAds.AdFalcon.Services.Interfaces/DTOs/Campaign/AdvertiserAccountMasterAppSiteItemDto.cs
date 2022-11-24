using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{

    [ProtoContract]
    public class AdvertiserAccountMasterAppSiteItemResultDto
    {
        
       [ProtoMember(1)]
        public IEnumerable<AdvertiserAccountMasterAppSiteItemDto> Items { get; set; } = new List<AdvertiserAccountMasterAppSiteItemDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }
    [ProtoContract]
    public class AdvertiserAccountMasterAppSiteItemDto
    {

       [ProtoMember(1)]

        public int Id { get; set; }
       [ProtoMember(2)]

        public int LinkId { get; set; }


       [ProtoMember(3)]

        public string AppSiteID { get; set; }

       [ProtoMember(4)]
        public MasterAppSiteItemType Type { get; set; }

       [ProtoMember(5)]
        public bool IsDeleted { get; set; }

       [ProtoMember(6)]

        public string BundleID { get; set; }
       [ProtoMember(7)]

        public string Domain { get; set; }


       [ProtoMember(8)]
        public string AppSiteName { get; set; }

       [ProtoMember(9)]
        public string TypeString { get { return Type.ToText(); } set { } }
       [ProtoMember(10)]

        public int UserId { get; set; }
       [ProtoMember(11)]

        public int AccountId { get; set; }
    }


    [ProtoContract]
    public class AudienceSegmentResultResultDto
    {

       [ProtoMember(1)]
        public IEnumerable<AudienceSegmentDto> Items { get; set; } = new List<AudienceSegmentDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }

}
