using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class CreativeUnitDto : LookupDto
    {

       [ProtoMember(1)]
        public string Code { get; set; }

       [ProtoMember(2)]
        public string Description { get; set; }

       [ProtoMember(3)]
        public int Width { get; set; }

       [ProtoMember(4)]
        public int Height { get; set; }

       [ProtoMember(5)]
        public int HD_Width { get; set; }

       [ProtoMember(6)]
        public int HD_Height { get; set; }

       [ProtoMember(7)]
        public int PreviewWidth { get; set; }

       [ProtoMember(8)]
        public int PreviewHeight { get; set; }

       [ProtoMember(9)]
        public IList<FormatDto> Formats { get; set; } = new List<FormatDto>();

        [ProtoMember(10)]
        public int RequiredType { get; set; }

       [ProtoMember(11)]
        public OrientationType OrientationType { get; set; }

       [ProtoMember(12)]
        public EnvironmentType EnvironmentType { get; set; }

       [ProtoMember(13)]
        public int? OrientationReplacementId { get; set; }

       [ProtoMember(14)]
        public LookupDto DeviceType { get; set; }

       [ProtoMember(15)]
        public int AdSupportedId { get; set; }

       [ProtoMember(16)]
        public string Url { get; set; }


       [ProtoMember(17)]
        public int AdId { get; set; }


        [ProtoMember(18)]
        public int AdType { get; set; }


        [ProtoMember(19)]
        public AdSubTypes? AdSubType { get; set; }

        [ProtoMember(20)]
        public IList<string> groupCodes { get; set; }


    }
}
