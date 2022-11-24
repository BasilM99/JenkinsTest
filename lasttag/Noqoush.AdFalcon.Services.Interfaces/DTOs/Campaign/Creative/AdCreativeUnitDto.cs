using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{

    [DataContract]
    public class AdCreativeUnitDto
    {

        [DataMember]
        public IList<AdCreativeUnitVendorDto> AdCreativeVendorIds { get; set; }
        [DataMember]
        public IList<int> CreativeVendorIds { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int CreativeUnitId { get; set; }
        [DataMember]
        public CreativeUnitDto CreativeUnit { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DocumentName { get; set; }
        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public int? DocumentId { get; set; }

        [DataMember]
        public int? SnapshotDocumentId { get; set; }

        [DataMember]
        public string SnapshotUrl { get; set; }

        [DataMember]
        public string ImpressionTrackerRedirect { get; set; }
        [DataMember]
        public string ImpressionTrackerJSRedirect { get; set; }
        [DataMember]
        public InStreamVideoCreativeUnitDto InStreamVideoCreativeUnit { get; set; }

        [DataMember]
        public string UniqueId { get; set; }


        [DataMember]
        public int AdId { get; set; }
        [DataMember]
        public IEnumerable<AdCreativeAttributeDto> Attributes { get; set; }

    }
}
