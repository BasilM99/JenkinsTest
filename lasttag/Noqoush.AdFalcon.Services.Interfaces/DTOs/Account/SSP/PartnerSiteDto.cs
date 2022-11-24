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
    public class PartnerSiteDto
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Description { get; set; }

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
        public string SiteName { get; set; }
        [DataMember]
        [Required()]
        public string SiteID { get; set; }

    }


    [DataContract]
    public class ResultPartnerSiteDto
    {
        [DataMember]
        public int BusinessId { get; set; }
        [DataMember]
        public string BusinessName { get; set; }
        [DataMember]
        public List<PartnerSiteDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }

    }
}
