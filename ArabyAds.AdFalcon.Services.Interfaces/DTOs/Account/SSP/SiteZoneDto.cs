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
    public class SiteZoneDto
    {

       [ProtoMember(1)]
        public string Description { get; set; }
       [ProtoMember(2)]
        public int ID { get; set; }
       [ProtoMember(3)]
        public int SiteID { get; set; }
       [ProtoMember(4)]
        public bool IsDeleted
        {
            get;
            set;
        }
       [ProtoMember(5)]
        [Required()]
        public string ZoneName { get; set; }
       [ProtoMember(6)]
       [Required()]
        public string ZoneID { get; set; }
    }

    [ProtoContract]
    public class ResultSiteZoneDto
    {


       [ProtoMember(1)]
        public int SiteId { get; set; }

       [ProtoMember(2)]
        public string SiteIdStr { get; set; }
       [ProtoMember(3)]
        public string SiteName { get; set; }
       [ProtoMember(4)]
        public int BusinessId { get; set; }
       [ProtoMember(5)]
        public string BusinessName { get; set; }

        [ProtoMember(6)]
        public List<SiteZoneDto> Items { get; set; } = new List<SiteZoneDto>();
       [ProtoMember(7)]
        public long TotalCount { get; set; }

    }
}
