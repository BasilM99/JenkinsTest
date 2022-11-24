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
    public class AdGroupTrackingEventDto
    {
       [ProtoMember(1)]
        public bool IsDeleted { get; set; }
       [ProtoMember(2)]
        public int Id { get; set; }
       [ProtoMember(3)]
        public int ValidFor { get; set; }
       [ProtoMember(4)]
        public string Code { get; set; }

       [ProtoMember(5)]
        public string Description { get; set; }
       
       [ProtoMember(6)]
        public string Name { get; set; }


       [ProtoMember(7)]
        public string PreRequisites
        {
            get;
            set;
        }

       [ProtoMember(8)]
        public List<string> PreRequisitesList
        {
            get
            {
                return string.IsNullOrEmpty(PreRequisites) ? new List<string>() : PreRequisites.Split(',').ToList();
            }
            set { }
        }

       [ProtoMember(9)]
        public bool AllPreRequisitesRequired { get; set; }

       [ProtoMember(10)]
        public bool AllowDuplicate { get; set; }

       [ProtoMember(11)]
        public bool IsCustom { get; set; }

       [ProtoMember(12)]
        public string IsCustomBlock { get { if (this.IsCustom) return "block"; else return "none"; } set { } }

       [ProtoMember(13)]
        public string IsNonBlock { get { if (this.IsCustom) return "none" ; else return "block"; } set { } }
       [ProtoMember(14)]
        public bool IsBillable { get; set; }


       [ProtoMember(15)]
        public string SegmentString { get; set; }


       [ProtoMember(16)]
        public string SegmentsId { get; set; }

       [ProtoMember(17)]
        public string SegmentsMapId { get; set; }


       [ProtoMember(18)]
        public string PixelString { get; set; }


       [ProtoMember(19)]
        public bool IsConversion { get; set; }


       [ProtoMember(20)]
        public string PixelsId { get; set; }

       [ProtoMember(21)]
        public string PixelsMapId { get; set; }



       [ProtoMember(22)]
        public bool IsNotChanged { get; set; }


        [ProtoMember(23)]
        public string PreRequisitesListString
        {
            get
            {
                return string.IsNullOrEmpty(PreRequisites) ? "" : PreRequisites;
            }
            set { }
        }

    }

    [ProtoContract]
    public class AdGroupTrackingEventResultDto
    {
       [ProtoMember(1)]
        public IList<AdGroupTrackingEventDto> Items { get; set; } = new List<AdGroupTrackingEventDto>();

        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }




    [ProtoContract]
    public class AdGroupConversionEventDto
    {

       [ProtoMember(1)]
        public bool IsDeleted { get; set; }
       [ProtoMember(2)]
        public int Id { get; set; }
       [ProtoMember(3)]
        public int ValidFor { get; set; }
       [ProtoMember(4)]
        public string Code { get; set; }

       [ProtoMember(5)]
        public string Description { get; set; }

       [ProtoMember(6)]
        public string Name { get; set; }


       [ProtoMember(7)]
        public string PreRequisites
        {
            get;
            set;
        }

       [ProtoMember(8)]
        public List<string> PreRequisitesList
        {
            get
            {
                return string.IsNullOrEmpty(PreRequisites) ? new List<string>() : PreRequisites.Split(',').ToList();
            }
            set { }
        }

       [ProtoMember(9)]
        public bool AllPreRequisitesRequired { get; set; }

       [ProtoMember(10)]
        public bool AllowDuplicate { get; set; }

       [ProtoMember(11)]
        public bool IsCustom { get; set; }

       [ProtoMember(12)]
        public string IsCustomBlock { get { if (this.IsCustom) return "block"; else return "none"; } set { } }

       [ProtoMember(13)]
        public string IsNonBlock { get { if (this.IsCustom) return "none"; else return "block"; } set { } }
       [ProtoMember(14)]
        public bool IsBillable { get; set; }

       [ProtoMember(15)]
        public bool IsPrimary { get; set; }
       [ProtoMember(16)]
        public string PixelString { get; set; }


       [ProtoMember(17)]
        public bool IsConversion { get; set; }

       [ProtoMember(18)]
        public decimal Revenue { get; set; }
       [ProtoMember(19)]
        public string PixelsId { get; set; }

       [ProtoMember(20)]
        public string PixelsMapId { get; set; }

       [ProtoMember(21)]
        public bool IsNotChanged { get; set; }
    }

    [ProtoContract]
    public class AdGroupConversionEventResultDto
    {
       [ProtoMember(1)]
        public IList<AdGroupConversionEventDto> Items { get; set; } = new List<AdGroupConversionEventDto>();

        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }
}
