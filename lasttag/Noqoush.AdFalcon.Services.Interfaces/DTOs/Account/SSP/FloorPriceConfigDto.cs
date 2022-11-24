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
    public class FloorPriceConfigDto
    {
        [DataMember]
        public int ID { get; set; }





        [DataMember]
        public bool IsDeleted
        {
            get;
            set;
        }
        [DataMember]
        public int ZoneID
        {
            get;
            set;
        }
        [DataMember]
        public string BidConfigType
        {
            get;
            set;
        }

        [DataMember]
        public int SiteID
        {
            get;
            set;
        }

        [DataMember]
        [Required()]
        public int ConfigTypeId
        {
            get;
            set;
        }
        [DataMember]
        public FloorPriceConfigType ConfigType
        {
            get;
            set;
        }

        [DataMember]
        public string TargetingName
        {
            get;
            set;
        }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int TargetingId
        {
            get;
            set;
        }
        [Required()]
        [DataMember]
        public string TargetingIdString
        {
            get
            {

                if (TargetingId > -1)
                {
                    return TargetingId.ToString();
                }
                return string.Empty;
            }
            set { }
        }
        [DataMember]
        [Required()]
        public decimal Price
        {
            get;
            set;
        }

    }


    [DataContract]
    public class ResultFloorPriceConfigDto
    {
        [DataMember]
        public int ZoneId { get; set; }
        [DataMember]
        public string ZoneName { get; set; }
        [DataMember]
        public string ZoneIdStr { get; set; }
        [DataMember]
        public string SiteIdStr { get; set; }
        [DataMember]
        public int SiteId { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public int BusinessId { get; set; }
        [DataMember]
        public string BusinessName { get; set; }

        [DataMember]
        public List<FloorPriceConfigDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }

    }
}
