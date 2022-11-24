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
    public class AdGroupTrackingEventDto
    {
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ValidFor { get; set; }
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }
       
        [DataMember]
        public string Name { get; set; }


        [DataMember]
        public string PreRequisites
        {
            get;
            set;
        }

        [DataMember]
        public List<string> PreRequisitesList
        {
            get
            {
                return string.IsNullOrEmpty(PreRequisites) ? new List<string>() : PreRequisites.Split(',').ToList();
            }
            set { }
        }

        [DataMember]
        public bool AllPreRequisitesRequired { get; set; }

        [DataMember]
        public bool AllowDuplicate { get; set; }

        [DataMember]
        public bool IsCustom { get; set; }

        [DataMember]
        public string IsCustomBlock { get { if (this.IsCustom) return "block"; else return "none"; } set { } }

        [DataMember]
        public string IsNonBlock { get { if (this.IsCustom) return "none" ; else return "block"; } set { } }
        [DataMember]
        public bool IsBillable { get; set; }


        [DataMember]
        public string SegmentString { get; set; }


        [DataMember]
        public string SegmentsId { get; set; }

        [DataMember]
        public string SegmentsMapId { get; set; }


        [DataMember]
        public string PixelString { get; set; }


        [DataMember]
        public bool IsConversion { get; set; }


        [DataMember]
        public string PixelsId { get; set; }

        [DataMember]
        public string PixelsMapId { get; set; }



        [DataMember]
        public bool IsNotChanged { get; set; }
    }

    [DataContract]
    public class AdGroupTrackingEventResultDto
    {
        [DataMember]
        public IList<AdGroupTrackingEventDto> Items { get; set; }

        [DataMember]
        public long TotalCount { get; set; }
    }




    [DataContract]
    public class AdGroupConversionEventDto
    {

        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ValidFor { get; set; }
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Name { get; set; }


        [DataMember]
        public string PreRequisites
        {
            get;
            set;
        }

        [DataMember]
        public List<string> PreRequisitesList
        {
            get
            {
                return string.IsNullOrEmpty(PreRequisites) ? new List<string>() : PreRequisites.Split(',').ToList();
            }
            set { }
        }

        [DataMember]
        public bool AllPreRequisitesRequired { get; set; }

        [DataMember]
        public bool AllowDuplicate { get; set; }

        [DataMember]
        public bool IsCustom { get; set; }

        [DataMember]
        public string IsCustomBlock { get { if (this.IsCustom) return "block"; else return "none"; } set { } }

        [DataMember]
        public string IsNonBlock { get { if (this.IsCustom) return "none"; else return "block"; } set { } }
        [DataMember]
        public bool IsBillable { get; set; }

        [DataMember]
        public bool IsPrimary { get; set; }
        [DataMember]
        public string PixelString { get; set; }


        [DataMember]
        public bool IsConversion { get; set; }

        [DataMember]
        public decimal Revenue { get; set; }
        [DataMember]
        public string PixelsId { get; set; }

        [DataMember]
        public string PixelsMapId { get; set; }

        [DataMember]
        public bool IsNotChanged { get; set; }
    }

    [DataContract]
    public class AdGroupConversionEventResultDto
    {
        [DataMember]
        public IList<AdGroupConversionEventDto> Items { get; set; }

        [DataMember]
        public long TotalCount { get; set; }
    }
}
