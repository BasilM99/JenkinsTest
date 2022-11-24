using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class CreativeUnitDto : LookupDto
    {

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Height { get; set; }

        [DataMember]
        public int HD_Width { get; set; }

        [DataMember]
        public int HD_Height { get; set; }

        [DataMember]
        public int PreviewWidth { get; set; }

        [DataMember]
        public int PreviewHeight { get; set; }

        [DataMember]
        public IList<FormatDto> Formats { get; set; }

        [DataMember]
        public int RequiredType { get; set; }

        [DataMember]
        public OrientationType OrientationType { get; set; }

        [DataMember]
        public EnvironmentType EnvironmentType { get; set; }

        [DataMember]
        public int? OrientationReplacementId { get; set; }

        [DataMember]
        public LookupDto DeviceType { get; set; }

        [DataMember]
        public int AdSupportedId { get; set; }

        [DataMember]
        public string Url { get; set; }


        [DataMember]
        public int AdId { get; set; }
    }
}
