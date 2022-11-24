using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class AdGeoLocationDto : BaseCampaignResultDto
    {
       [ProtoMember(1)]
        public Int64 TotalCount;
       [ProtoMember(2)]
        public string CountryName { get; set; }

       [ProtoMember(3)]
        public string CampaignName { get; set; }

       
    }
}
