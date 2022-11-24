using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class InStreamVideoCreativeUnitDto
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public int? CreativeVendorId { get; set; }

        [DataMember]
        public int OriginalCreativeUnitID { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int VideoType { get; set; }
        [DataMember]
        public string VideoTypeCode { get; set; }
        [DataMember]
        public int DeliveryMethod { get; set; }
        [DataMember]
        public int VideoDuration { get; set; }
        [DataMember]
        public int VideoHeight { get; set; }
        [DataMember]
        public int VideoWidth { get; set; }
        [DataMember]
        public int BitRate { get; set; }
        [DataMember]
        public string VideoUrl { get; set; }
        [DataMember]
        public string XmlUrl { get; set; }
        [DataMember]
        public string Xml { get; set; }
        [DataMember]
        public bool IsXmlUrl { get; set; }
        [DataMember]
        public bool IsVideo { get; set; }
        [DataMember]
        public bool Vpaid { get; set; }
        [DataMember]
        public bool Vpaid_1 { get; set; }
        [DataMember]
        public bool Vpaid_2 { get; set; }

        [DataMember]
        public int? ThumbnailDocId { get; set; }
        [DataMember]
        public IList<AdCreativeUnitTrackerDto> ImpressionTrackerRedirectList { get; set; }

        [DataMember]
        public IList<AdCreativeUnitTrackerDto> ImpressionTrackerJSRedirectList { get; set; }

        [DataMember]
        public VASTProtocolsVersion VASTProtocol { get; set; }

    }
}
