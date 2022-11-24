using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetCampsAdvertiserBydealRequest
    {
        [ProtoMember(1)]
        public int DealId { get; set; }
        [ProtoMember(2)]
        public int AdvertiserId { get; set; }

        public override string ToString()
        {
            return $"{DealId}_{AdvertiserId}";
        }
    }
}
