using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class DocumentBaseDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int DocumentTypeId { get; set; }
        [DataMember]
        public string Extension { get; set; }
        [DataMember]
        public int Size { get; set; }
        [DataMember]
        public DateTime UploadedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public bool IsWebHDFS { get; set; }
        [DataMember]
        public string CurrentNameUp{ get {

                if (!string.IsNullOrEmpty(UsedNameUp))
                {
                    return UsedNameUp;
                }
                return Name;
            } set { } }

        [DataMember]
        public string UsedNameUp { get; set; }
    }
    [DataContract]
    public class DocumentDto:DocumentBaseDto
    {
        [DataMember]
        public byte[] Content { get; set; }

        [DataMember]
        public string InputPath { get; set; }
    }
}
