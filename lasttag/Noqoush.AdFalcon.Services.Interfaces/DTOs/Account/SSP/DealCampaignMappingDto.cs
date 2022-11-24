
using Noqoush.AdFalcon.Domain.Common.Model.Account.SSP;
using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.SSP
{


    [DataContract]
    public class DealCampaignMappingDto
    {
        [DataMember]
        public int ID { get; set; }

         [DataMember]
        public int PartnerID { get; set; }




        [DataMember]
        public bool IsDeleted
        {
            get;
            set;
        }

        

        [DataMember]
        [Required()]
        public string DealId
        {
            get;
            set;
        }


        [DataMember]
        [Required()]
        public int AdFalconCampaignId
        {
            get;
            set;
        }
        [DataMember]
        [Required()]
        public string CampaignName
        {
            get;
            set;
        }
    }


           


    [DataContract]
    public class ResultDealCampaignMappingDto
    {
        [DataMember]
        public string ZoneIdStr { get; set; }
        [DataMember]
        public string SiteIdStr { get; set; }
        [DataMember]
        public int ZoneId { get; set; }
        [DataMember]
        public string ZoneName { get; set; }
        [DataMember]
        public int SiteId { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public int BusinessId { get; set; }
        [DataMember]
        public string BusinessName { get; set; }

        [DataMember]
        public List<DealCampaignMappingDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }

    }
}
