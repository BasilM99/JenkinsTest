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
    public class FloorPriceConfigDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }





       [ProtoMember(2)]
        public bool IsDeleted
        {
            get;
            set;
        }
       [ProtoMember(3)]
        public int ZoneID
        {
            get;
            set;
        }
       [ProtoMember(4)]
        public string BidConfigType
        {
            get;
            set;
        }

       [ProtoMember(5)]
        public int SiteID
        {
            get;
            set;
        }

       [ProtoMember(6)]
        [Required()]
        public int ConfigTypeId
        {
            get;
            set;
        }
       [ProtoMember(7)]
        public FloorPriceConfigType ConfigType
        {
            get;
            set;
        }

       [ProtoMember(8)]
        public string TargetingName
        {
            get;
            set;
        }
       [ProtoMember(9)]
        public string Description { get; set; }

       [ProtoMember(10)]
        public int TargetingId
        {
            get;
            set;
        }
        [Required()]
       [ProtoMember(11)]
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
       [ProtoMember(12)]
        [Required()]
        public decimal Price
        {
            get;
            set;
        }

    }


    [ProtoContract]
    public class ResultFloorPriceConfigDto
    {
       [ProtoMember(1)]
        public int ZoneId { get; set; }
       [ProtoMember(2)]
        public string ZoneName { get; set; }
       [ProtoMember(3)]
        public string ZoneIdStr { get; set; }
       [ProtoMember(4)]
        public string SiteIdStr { get; set; }
       [ProtoMember(5)]
        public int SiteId { get; set; }
       [ProtoMember(6)]
        public string SiteName { get; set; }
       [ProtoMember(7)]
        public int BusinessId { get; set; }
       [ProtoMember(8)]
        public string BusinessName { get; set; }

        [ProtoMember(9)]
        public List<FloorPriceConfigDto> Items { get; set; } = new List<FloorPriceConfigDto>();
       [ProtoMember(10)]
        public long TotalCount { get; set; }

    }
}
