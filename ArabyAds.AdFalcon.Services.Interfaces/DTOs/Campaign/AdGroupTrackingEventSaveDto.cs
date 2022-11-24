using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdGroupTrackingEventSaveDto
    {
       [ProtoMember(1)]
        [StringLength(6, MinimumLength = 2, ErrorMessage = "Invalid")]
        [RegularExpression("^\\w+$", ResourceName = "TrackingEventCodeFormat")]
        [Required]
        public string Code { get; set; }

       [ProtoMember(2)]
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Invalid")]
        public string Description { get; set; }
       [ProtoMember(3)]
        [Required]
        [Range(1, int.MaxValue)]
        public int ValidFor { get; set; }

       [ProtoMember(4)]
        public List<string> PreRequisites
        {
            get;
            set;
        }


       [ProtoMember(5)]
        public bool AllPreRequisitesRequired { get; set; }

       [ProtoMember(6)]
        public bool IsBillable { get; set; }

       [ProtoMember(7)]
        public bool AllowDuplicate { get; set; }


       [ProtoMember(8)]
        public string SegmentString { get; set; }


       [ProtoMember(9)]
        public string SegmentsId { get; set; }

       [ProtoMember(10)]
        public string SegmentsMapId { get; set; }

       [ProtoMember(11)]
        public int ID { get; set; }


        [ProtoMember(12)]
        public List<string> PreRequisitesList
        {
            get;
            set;
        }

        [ProtoMember(13)]
        public bool IsCustom
        {
            get;
            set;
        }


        [ProtoMember(14)]
        public string Name
        {
            get;
            set;
        }

       
    }



    [ProtoContract]
    public class AdGroupConversionEventSaveDto
    {
       [ProtoMember(1)]
        [StringLength(6, MinimumLength = 2, ErrorMessage = "Invalid")]
        [RegularExpression("^\\w+$", ResourceName = "TrackingEventCodeFormat")]
        [Required]
        public string Code { get; set; }

       [ProtoMember(2)]
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Invalid")]
        public string Description { get; set; }
       [ProtoMember(3)]
        [Required]
        [Range(1, int.MaxValue)]
        public int ValidFor { get; set; }

       [ProtoMember(4)]
        public List<string> PreRequisites
        {
            get;
            set;
        }


       [ProtoMember(5)]
        public bool AllPreRequisitesRequired { get; set; }

       [ProtoMember(6)]
        public bool IsBillable { get; set; }

       [ProtoMember(7)]
        public bool AllowDuplicate { get; set; }


       [ProtoMember(8)]
        public string PixelString { get; set; }


       [ProtoMember(9)]
        public string PixelsId { get; set; }

       [ProtoMember(10)]
        public string PixelsMapId { get; set; }

       [ProtoMember(11)]
        public int ID { get; set; }
    }

}
