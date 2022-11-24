using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Objective;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdGroupDto
    {
        [DataMember]
        public virtual int CampaignId { get; set; }

        [DataMember]
        [Required(ResourceName = "GroupNameRequiredMsg", ResourceSet = "Msgs")]
        [StringLength(255, ResourceName = "GroupNamLengthMsg", ResourceSet = "Msgs")]
        public virtual string Name { get; set; }

        [DataMember]
        [Required(ResourceName = "ActionTypeRequiredMsg", ResourceSet = "Msgs")]
        public AdActionTypeIds ActionTypeId { get; set; }

        [DataMember]
        [Required(ResourceName = "ObjectiveTypeRequiredMsg", ResourceSet = "Msgs")]
        public AdGroupObjectiveTypeIds ObjectiveTypeId { get; set; }

        [DataMember]
        public AdTypeIds? TypeId { get; set; }

        [DataMember]
        public bool IsCostModelChanged { get; set; }


    }
}
