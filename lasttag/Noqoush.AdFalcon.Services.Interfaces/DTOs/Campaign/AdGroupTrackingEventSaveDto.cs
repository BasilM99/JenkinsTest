using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdGroupTrackingEventSaveDto
    {
        [DataMember]
        [StringLength(6, MinimumLength = 2, ErrorMessage = "Invalid")]
        [RegularExpression("^\\w+$", ResourceName = "TrackingEventCodeFormat")]
        [Required]
        public string Code { get; set; }

        [DataMember]
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Invalid")]
        public string Description { get; set; }
        [DataMember]
        [Required]
        [Range(1, int.MaxValue)]
        public int ValidFor { get; set; }

        [DataMember]
        public List<string> PreRequisites
        {
            get;
            set;
        }


        [DataMember]
        public bool AllPreRequisitesRequired { get; set; }

        [DataMember]
        public bool IsBillable { get; set; }

        [DataMember]
        public bool AllowDuplicate { get; set; }


        [DataMember]
        public string SegmentString { get; set; }


        [DataMember]
        public string SegmentsId { get; set; }

        [DataMember]
        public string SegmentsMapId { get; set; }

        [DataMember]
        public int ID { get; set; }
    }



    [DataContract]
    public class AdGroupConversionEventSaveDto
    {
        [DataMember]
        [StringLength(6, MinimumLength = 2, ErrorMessage = "Invalid")]
        [RegularExpression("^\\w+$", ResourceName = "TrackingEventCodeFormat")]
        [Required]
        public string Code { get; set; }

        [DataMember]
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Invalid")]
        public string Description { get; set; }
        [DataMember]
        [Required]
        [Range(1, int.MaxValue)]
        public int ValidFor { get; set; }

        [DataMember]
        public List<string> PreRequisites
        {
            get;
            set;
        }


        [DataMember]
        public bool AllPreRequisitesRequired { get; set; }

        [DataMember]
        public bool IsBillable { get; set; }

        [DataMember]
        public bool AllowDuplicate { get; set; }


        [DataMember]
        public string PixelString { get; set; }


        [DataMember]
        public string PixelsId { get; set; }

        [DataMember]
        public string PixelsMapId { get; set; }

        [DataMember]
        public int ID { get; set; }
    }

}
