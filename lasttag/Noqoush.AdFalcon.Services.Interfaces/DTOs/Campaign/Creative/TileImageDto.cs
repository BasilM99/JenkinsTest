using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class TileImageDto : LookupDto
    {
       
        [DataMember]
        public bool IsCustom { get; set; }
        [DataMember]
        public IEnumerable<TileImageDocumentDto> Images  { get; set; }
    }
}
