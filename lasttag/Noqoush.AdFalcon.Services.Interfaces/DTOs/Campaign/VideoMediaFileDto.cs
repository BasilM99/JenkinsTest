using System;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class VideoMediaFileDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int DocumentId { get; set; }
        [DataMember]
        public int AdCreativeUnitId { get; set; }
        [DataMember]
        public int VideoTypeId { get; set; }
        [DataMember]
        public int DeliveryMethodId { get; set; }

        [DataMember]
        public int duration { get; set; }
        [DataMember]
        public int bitRate { get; set; }
        [DataMember]
        public int width { get; set; }
        [DataMember]
        public int height { get; set; }
        [DataMember]
        public int VideoAdId { get; set; }
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public int CreativeUnitId { get; set; }

        [DataMember]
        public string  videoExtension { get; set; }
        [DataMember]
        public int AdCreativeId { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
