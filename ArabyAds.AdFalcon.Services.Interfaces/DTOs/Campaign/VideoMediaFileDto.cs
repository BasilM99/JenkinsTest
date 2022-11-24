using System;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class VideoMediaFileDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public int DocumentId { get; set; }
       [ProtoMember(3)]
        public int AdCreativeUnitId { get; set; }
       [ProtoMember(4)]
        public int VideoTypeId { get; set; }
       [ProtoMember(5)]
        public int DeliveryMethodId { get; set; }

       [ProtoMember(6)]
        public int duration { get; set; }
       [ProtoMember(7)]
        public int bitRate { get; set; }
       [ProtoMember(8)]
        public int width { get; set; }
       [ProtoMember(9)]
        public int height { get; set; }
       [ProtoMember(10)]
        public int VideoAdId { get; set; }
       [ProtoMember(11)]
        public string URL { get; set; }
       [ProtoMember(12)]
        public int CreativeUnitId { get; set; }

       [ProtoMember(13)]
        public string  videoExtension { get; set; }
       [ProtoMember(14)]
        public int AdCreativeId { get; set; }

       [ProtoMember(15)]
        public bool IsDeleted { get; set; }
    }
}
