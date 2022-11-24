using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{

    [ProtoContract]
    public class AdCreativeUnitDto
    {

       [ProtoMember(1)]
        public IList<AdCreativeUnitVendorDto> AdCreativeVendorIds { get; set; } = new List<AdCreativeUnitVendorDto>();
        [ProtoMember(2)]
        public IList<int> CreativeVendorIds { get; set; }
       [ProtoMember(3)]
        public int ID { get; set; }
       [ProtoMember(4)]
        public bool IsDeleted { get; set; }
       [ProtoMember(5)]
        public int CreativeUnitId { get; set; }
       [ProtoMember(6)]
        public CreativeUnitDto CreativeUnit { get; set; }
       [ProtoMember(7)]
        public string Name { get; set; }
       [ProtoMember(8)]
        public string DocumentName { get; set; }
       [ProtoMember(9)]
        public string Content { get; set; }

       [ProtoMember(10)]
        public int? DocumentId { get; set; }

       [ProtoMember(11)]
        public int? SnapshotDocumentId { get; set; }

       [ProtoMember(12)]
        public string SnapshotUrl { get; set; }

       [ProtoMember(13)]
        public string ImpressionTrackerRedirect { get; set; }
       [ProtoMember(14)]
        public string ImpressionTrackerJSRedirect { get; set; }
       [ProtoMember(15)]
        public InStreamVideoCreativeUnitDto InStreamVideoCreativeUnit { get; set; }

       [ProtoMember(16)]
        public string UniqueId { get; set; }


       [ProtoMember(17)]
        public int AdId { get; set; }
       [ProtoMember(18)]
        public IEnumerable<AdCreativeAttributeDto> Attributes { get; set; } = new List<AdCreativeAttributeDto>();


        [ProtoMember(19)]
        public string FileExtension { get; set; }

        [ProtoMember(20)]
        public int Width { get; set; }

        [ProtoMember(21)]
        public int Height { get; set; }


        [ProtoMember(22)]
        public int CreativeUnitCode { get; set; }

        [ProtoMember(23)]
        public int? OrientationReplacementId { get; set; }

        


    }
}
