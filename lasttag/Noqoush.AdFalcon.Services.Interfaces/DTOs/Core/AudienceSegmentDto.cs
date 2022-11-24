using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.Resources;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
 

    [DataContract]
    public class AudienceSegmentDto : LookupDto
    {
        [DataMember]
        public int IntegrationId { get; set; }
        [DataMember]
        public int AdvertiserId { get; set; }
        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public int UserId { get; set; }
        
        [DataMember]
        public int Level { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int ProviderId { get; set; }
        [DataMember]
        public string ProviderName { get; set; }

        [DataMember]
        public string ParentName { get; set; }
        [DataMember]
        [Required]
        public decimal Price { get; set; }



        [DataMember]
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
        [DataMember]
        [Required]
        [StringLength(200)]

        public string OperatorSegmentCode { get; set; }
        [DataMember]
        public bool Selectable { get; set; }
        [DataMember]
        public bool IsChlid { get; set; }
        [DataMember]
        public bool IsSelectedable { get; set; }
        [DataMember]
        public int ParentId { get; set; }
        [Range(0, 10999999, ResourceName = "RangeMessage")]
        [Required]
        [RegularExpression(@"^[0-9]*$", ResourceName = "OnlyIntegers", ResourceSet ="Global")]


        [DataMember]
        public int CodeUQ { get; set; }

        [DataMember]
        public string Path { get; set; }


        [DataMember]
        public string ar { get; set; }

        [DataMember]
        public string en { get; set; }

        [DataMember]
        public string roots { get; set; }


        [DataMember]
        public bool IsPermissionNeed { get; set; }

        public string priceString { get { return (Price * 1000).ToString("F2") + "$"; } set {  } }
        [DataMember]
        public IList<string> Names { get; set; }



        [DataMember]
        public AudienceListPerformanceDto Performance { get; set; }


        #region "looklike "


        [DataMember]
        public int SeedAudienceListCode { get; set; }

        [DataMember]
        public int LookalikeAudienceListCode { get; set; }

        [DataMember]
        public string PopulationCountryFilter { get; set; }

        [DataMember]
        public float LookalikePercentage { get; set; }

       


        
        #endregion
    }



    [DataContract]
    public class RuleDto
    {
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal Price { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Condition { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public group group { get; set; }

    }

    [DataContract]
    public class group
    {
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Operator { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public IList<RuleDto> rules { get; set; }

    }
}
