using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.Framework.Resources;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using System.ComponentModel;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
 

    [ProtoContract]
    public class AudienceSegmentDto : LookupDto
    {
       [ProtoMember(1)]
        public int IntegrationId { get; set; }
       [ProtoMember(2)]
        public int AdvertiserId { get; set; }
       [ProtoMember(3)]
        public int AccountId { get; set; }

       [ProtoMember(4)]
        public int UserId { get; set; }
        
       [ProtoMember(5)]
        public int Level { get; set; }

       [ProtoMember(6)]
        public bool IsDeleted { get; set; }
       [ProtoMember(7)]
        public int ProviderId { get; set; }
       [ProtoMember(8)]
        public string ProviderName { get; set; }

       [ProtoMember(9)]
        public string ParentName { get; set; }
       [ProtoMember(10)]
        [Required]
        public decimal Price { get; set; }



       [ProtoMember(11)]
        public string Description { get; set; }


        public string StatusString { get {

                if (IsDeleted)
                {
                   return  ResourceManager.Instance.GetResource("StatusNotActive", "PMPDeals");
                }
                else
                {
                    return ResourceManager.Instance.GetResource("Active", "JobGrid");
                }
            } set {


            } }
       [ProtoMember(12)]
        [Required]
        [StringLength(200)]

        public string OperatorSegmentCode { get; set; }
       [ProtoMember(13)]
        public bool Selectable { get; set; }
       [ProtoMember(14)]
        public bool IsChlid { get; set; }
       [ProtoMember(15)]
        public bool IsSelectedable { get; set; }
       [ProtoMember(16)]
        public int ParentId { get; set; }
        [Range(0, 10999999, ResourceName = "RangeMessage")]
        [Required]
        [RegularExpression(@"^[0-9]*$", ResourceName = "OnlyIntegers", ResourceSet ="Global")]


       [ProtoMember(17)]
        public int CodeUQ { get; set; }

       [ProtoMember(18)]
        public string Path { get; set; }


       [ProtoMember(19)]
        public string ar { get; set; }

       [ProtoMember(20)]
        public string en { get; set; }

       [ProtoMember(21)]
        public string roots { get; set; }


       [ProtoMember(22)]
        public bool IsPermissionNeed { get; set; }

        public string priceString { get { return (Price * 1000).ToString("F2") + "$"; } set {  } }
       [ProtoMember(23)]
        public IList<string> Names { get; set; } = new List<string>();



        [ProtoMember(24)]
        public AudienceListPerformanceDto Performance { get; set; }


        #region "looklike "


       [ProtoMember(25)]
        public int SeedAudienceListCode { get; set; }

       [ProtoMember(26)]
        public int LookalikeAudienceListCode { get; set; }

       [ProtoMember(27)]
        public string PopulationCountryFilter { get; set; }

       [ProtoMember(28)]
        public float LookalikePercentage { get; set; }





        #endregion


        [ProtoMember(29)]
        public bool showrecency { get; set; }

        [ProtoMember(30)]
        public int recency { get; set; }

        public string Status
        {
            get
            {

                if (IsDeleted)
                {
                    return "NotActive";
                }
                else
                {
                    return "Active";
                }
            }
         
        }

        [ProtoMember(31)]
        public bool Positive { get; set; }
        
        [ProtoMember(32)]
        public string Condition { get; set; }

    }



    [ProtoContract]
    public class RuleDto
    {
        [ProtoMember(1,IsRequired = false)]
        public int Id { get; set; }

        [ProtoMember(2,IsRequired = false)]
        public string Name { get; set; }

        [ProtoMember(3,IsRequired = false)]
        public decimal Price { get; set; }

        [ProtoMember(4,IsRequired = false)]
        public string Condition { get; set; }

        [ProtoMember(5,IsRequired = false)]
        public group group { get; set; }

    }

    [ProtoContract]
    public class group
    {
        [ProtoMember(1,IsRequired = false)]
        public int Id { get; set; }

        [ProtoMember(2,IsRequired = false)]
        public string Operator { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public IList<RuleDto> rules { get; set; } = new List<RuleDto>();

    }
}
