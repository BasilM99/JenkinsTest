using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    [ProtoInclude(100,typeof(DocumentDto))]
    public class DocumentBaseDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public string Name { get; set; }
       [ProtoMember(3)]
        public int DocumentTypeId { get; set; }
       [ProtoMember(4)]
        public string Extension { get; set; }
       [ProtoMember(5)]
        public int Size { get; set; }
       [ProtoMember(6)]
        public DateTime UploadedDate { get; set; }
       [ProtoMember(7)]
        public bool IsDeleted { get; set; }

       [ProtoMember(8)]
        public bool IsWebHDFS { get; set; }
       [ProtoMember(9)]
        public string CurrentNameUp{ get {

                if (!string.IsNullOrEmpty(UsedNameUp))
                {
                    return UsedNameUp;
                }
                return Name;
            } set { } }

       [ProtoMember(11)]
        public string UsedNameUp { get; set; }
    }
    [ProtoContract]
    public class DocumentDto:DocumentBaseDto
    {
       [ProtoMember(1)]
        public byte[] Content { get; set; }

       [ProtoMember(2)]
        public string InputPath { get; set; }
    }
}
