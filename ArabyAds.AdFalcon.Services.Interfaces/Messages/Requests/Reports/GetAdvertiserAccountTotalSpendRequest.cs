using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.QueryBuilder;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetAdvertiserAccountTotalSpendRequest : FromToDateMessage
    {
        [ProtoMember(1)]
        public int Id { set; get; }

        public override string ToString()
        {
            return $"{base.ToString()}_{Id}";
        }

    }
}
