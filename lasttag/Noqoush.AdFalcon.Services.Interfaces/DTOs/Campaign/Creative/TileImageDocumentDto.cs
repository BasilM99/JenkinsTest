using System;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class TileImageDocumentDto : LookupDto
    {

        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public virtual DocumentBaseDto Document { get; set; }
        [DataMember]
        public virtual TileImageSizeDto TileImageSize { get; set; }
        [DataMember]
        public virtual bool IsDeleted { get; set; }
    }
}
