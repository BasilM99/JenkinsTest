using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class KPIFilterDto
    {

        [ProtoMember(1)]
        public DateTime FromDate { get; set; }

        [ProtoMember(2)]
        public DateTime ToDate { get; set; }

      
        [ProtoMember(3)]
        public int AdvertiserAccountId { get; set; }


        [ProtoMember(4)]
        public int CampaignId { get; set; }


        [ProtoMember(5)]
        public int DealId { get; set; }


        [ProtoMember(6)]
        public int DataProviderId { get; set; }

        [ProtoMember(7)]
        public int AppSiteId { get; set; }




    }
}
