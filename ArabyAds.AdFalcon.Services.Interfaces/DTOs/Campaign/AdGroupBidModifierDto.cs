using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.Framework.DomainServices;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdGroupBidModifierDto
    {

        [ProtoMember(1)]
        public virtual int CampaignId { get; set; }
        [ProtoMember(2)]
        public virtual int AdGroupId { get; set; }

        [ProtoMember(3)]
        public virtual decimal Multiplier { get; set; }

        [ProtoMember(4)]
        public virtual string DimensionValue { get; set; }

        [ProtoMember(5)]
        public virtual DimentionType DimensionType { get; set; }
        [ProtoMember(6)]
        public virtual bool IsDeleted { get; set; }

        [ProtoMember(7)]
        public virtual bool IsNotChanged { get; set; }


        [ProtoMember(8)]
        public virtual int DimensionTypeId { get; set; }
        [ProtoMember(9)]
        public virtual string DimensionTypeStr { get; set; }
        [ProtoMember(10)]
        public virtual int ID { get; set; }

        public object DimensionTypeObj { get; set; }
        public object Dimension { get; set; }
    }

}
