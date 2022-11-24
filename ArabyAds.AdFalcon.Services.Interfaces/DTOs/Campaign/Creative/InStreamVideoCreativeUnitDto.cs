using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class InStreamVideoCreativeUnitDto
    {
       [ProtoMember(1)]
        public string Code { get; set; }
       [ProtoMember(2)]
        public int? CreativeVendorId { get; set; }

       [ProtoMember(3)]
        public int OriginalCreativeUnitID { get; set; }
       [ProtoMember(4)]
        public int ID { get; set; }
       [ProtoMember(5)]
        public int VideoType { get; set; }
       [ProtoMember(6)]
        public string VideoTypeCode { get; set; }
       [ProtoMember(7)]
        public int DeliveryMethod { get; set; }
       [ProtoMember(8)]
        public int VideoDuration { get; set; }
       [ProtoMember(9)]
        public int VideoHeight { get; set; }
       [ProtoMember(10)]
        public int VideoWidth { get; set; }
       [ProtoMember(11)]
        public int BitRate { get; set; }
       [ProtoMember(12)]
        public string VideoUrl { get; set; }
       [ProtoMember(13)]
        public string XmlUrl { get; set; }
       [ProtoMember(14)]
        public string Xml { get; set; }
       [ProtoMember(15)]
        public bool IsXmlUrl { get; set; }
       [ProtoMember(16)]
        public bool IsVideo { get; set; }
       [ProtoMember(17)]
        public bool Vpaid { get; set; }
       [ProtoMember(18)]
        public bool Vpaid_1 { get; set; }
       [ProtoMember(19)]
        public bool Vpaid_2 { get; set; }

       [ProtoMember(20)]
        public int? ThumbnailDocId { get; set; }
       [ProtoMember(21)]
        public IList<AdCreativeUnitTrackerDto> ImpressionTrackerRedirectList { get; set; }  = new List<AdCreativeUnitTrackerDto>();

        [ProtoMember(22)]
        public IList<AdCreativeUnitTrackerDto> ImpressionTrackerJSRedirectList { get; set; } = new List<AdCreativeUnitTrackerDto>();

        [ProtoMember(23)]
        public VASTProtocolsVersion VASTProtocol { get; set; }

    }
}
