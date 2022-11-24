using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.SSP
{
    [ProtoContract]
    public class PartnerSiteDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }

       [ProtoMember(2)]
        public string Description { get; set; }

       [ProtoMember(3)]
        public int PartnerID { get; set; }
       [ProtoMember(4)]
        public bool IsDeleted
        {
            get;
            set;
        }
       [ProtoMember(5)]
        [Required()]
        public string SiteName { get; set; }
       [ProtoMember(6)]
        [Required()]
        public string SiteID { get; set; }

    }


    [ProtoContract]
    public class ResultPartnerSiteDto
    {
       [ProtoMember(1)]
        public int BusinessId { get; set; }
       [ProtoMember(2)]
        public string BusinessName { get; set; }
        [ProtoMember(3)]
        public List<PartnerSiteDto> Items { get; set; } = new List<PartnerSiteDto>();
       [ProtoMember(4)]
        public long TotalCount { get; set; }

    }
}
