
using ArabyAds.AdFalcon.Domain.Common.Model.Account.SSP;
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
    public class DealCampaignMappingDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }

        [ProtoMember(2)]
        public int PartnerID { get; set; }




       [ProtoMember(3)]
        public bool IsDeleted
        {
            get;
            set;
        }

        

       [ProtoMember(4)]
        [Required()]
        public string DealId
        {
            get;
            set;
        }


       [ProtoMember(5)]
        [Required()]
        public int AdFalconCampaignId
        {
            get;
            set;
        }
       [ProtoMember(6)]
        [Required()]
        public string CampaignName
        {
            get;
            set;
        }
    }


           


    [ProtoContract]
    public class ResultDealCampaignMappingDto
    {
       [ProtoMember(1)]
        public string ZoneIdStr { get; set; }
       [ProtoMember(2)]
        public string SiteIdStr { get; set; }
       [ProtoMember(3)]
        public int ZoneId { get; set; }
       [ProtoMember(4)]
        public string ZoneName { get; set; }
       [ProtoMember(5)]
        public int SiteId { get; set; }
       [ProtoMember(6)]
        public string SiteName { get; set; }
       [ProtoMember(7)]
        public int BusinessId { get; set; }
       [ProtoMember(8)]
        public string BusinessName { get; set; }

        [ProtoMember(9)]
        public List<DealCampaignMappingDto> Items { get; set; } = new List<DealCampaignMappingDto>();
       [ProtoMember(10)]
        public long TotalCount { get; set; }

    }
}
