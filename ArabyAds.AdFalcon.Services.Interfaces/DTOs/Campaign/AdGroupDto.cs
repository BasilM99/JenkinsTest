using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdGroupDto
    {
       [ProtoMember(1)]
        public virtual int CampaignId { get; set; }

       [ProtoMember(2)]
        [Required(ResourceName = "GroupNameRequiredMsg", ResourceSet = "Msgs")]
        [StringLength(255, ResourceName = "GroupNamLengthMsg", ResourceSet = "Msgs")]
        public virtual string Name { get; set; }

       [ProtoMember(3)]
        [Required(ResourceName = "ActionTypeRequiredMsg", ResourceSet = "Msgs")]
        public AdActionTypeIds ActionTypeId { get; set; }

       [ProtoMember(4)]
        [Required(ResourceName = "ObjectiveTypeRequiredMsg", ResourceSet = "Msgs")]
        public AdGroupObjectiveTypeIds ObjectiveTypeId { get; set; }

       [ProtoMember(5)]
        public AdTypeIds? TypeId { get; set; }

       [ProtoMember(6)]
        public bool IsCostModelChanged { get; set; }
        [ProtoMember(7)]
        public int AdActionTypeCode { get; set; }
        [ProtoMember(8)]
        public  decimal Bid { get; set; }


        [ProtoMember(9)]
        public BiddingStrategy BiddingStrategy { get; set; }



        [ProtoMember(10)]
        public decimal BidOptimizationValue { get; set; }


        [ProtoMember(11)]
        public decimal MaxBidPrice { get; set; }



        [ProtoMember(12)]
        public bool KeepBiddingAtMinimum { get; set; }
        [ProtoMember(13)]

        public BidOptimizationType BidOptimizationType { get; set; }
    }
}
