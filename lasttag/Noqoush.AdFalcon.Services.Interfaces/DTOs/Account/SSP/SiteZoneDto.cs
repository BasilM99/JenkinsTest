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
    public class SiteZoneDto
    {

        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int SiteID { get; set; }
        [DataMember]
        public bool IsDeleted
        {
            get;
            set;
        }
        [DataMember]
        [Required()]
        public string ZoneName { get; set; }
        [DataMember]
       [Required()]
        public string ZoneID { get; set; }
    }

    [DataContract]
    public class ResultSiteZoneDto
    {


        [DataMember]
        public int SiteId { get; set; }

        [DataMember]
        public string SiteIdStr { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public int BusinessId { get; set; }
        [DataMember]
        public string BusinessName { get; set; }

        [DataMember]
        public List<SiteZoneDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }

    }
}
