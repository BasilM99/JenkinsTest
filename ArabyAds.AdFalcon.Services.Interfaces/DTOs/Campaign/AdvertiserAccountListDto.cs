using System.Collections.Generic;
using ProtoBuf;
using System;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdvertiserAccountListResultDto
    {
       [ProtoMember(1)]
        public AdvertiserPerformanceDto Performance { get; set; }
       [ProtoMember(2)]
        public IEnumerable<AdvertiserAccountListDto> Items { get; set; } = new List<AdvertiserAccountListDto>();
        [ProtoMember(3)]
        public long TotalCount { get; set; }
    }
    [ProtoContract]
    public class AdvertiserAccountListDto
    {
       [ProtoMember(1)]
        public bool IsDeleted { get; set; }
       [ProtoMember(2)]
        public string Status { get; set; }
       [ProtoMember(3)]
        public int Id { get; set; }
       [ProtoMember(4)]
        public string Name { get; set; }
       [ProtoMember(5)]
        public AdvertiserPerformanceDto Performance { get; set; }

       [ProtoMember(6)]
        public AdvertiserDto AdvertiserItem { get; set; }

       [ProtoMember(7)]
        public int? AdvertiserId { get; set; }

        public string AdvertiserAccId { get { return Id + "-" + this.AdvertiserItem.ID; } set { } }

        public string IsDeletedString { get { return IsDeleted.ToString(); } }


        [ProtoMember(8)]
        public int UserRole { get { return (int)ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>().AccountRole; } set { } }
        [ProtoMember(9)]
        public string StatusString { get; set; }
    }
}
