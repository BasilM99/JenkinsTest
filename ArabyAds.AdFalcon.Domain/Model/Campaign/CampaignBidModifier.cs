using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class CampaignBidModifier : IEntity<int>
    {

        public virtual int ID { get; set; }

        public virtual Campaign Campaign { get; set; }

        public virtual string DimensionValue { get; set; }

        public virtual DimentionType DimentionType { get; set; }

        public virtual decimal Multiplier { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string GetDescription()
        {
            return string.Format("Type:{0}:Value:{1}:Modifier:{2}", DimentionType.ToText(), this.DimensionValue, this.Multiplier);
        }

        public virtual CampaignBidModifier Clone()
        {
            var cloneObj = new CampaignBidModifier()
            {
                Campaign = this.Campaign,
                IsDeleted = this.IsDeleted,
                DimentionType = this.DimentionType,

                DimensionValue = this.DimensionValue,
                Multiplier = this.Multiplier,


                //IsDeleted = this.IsDeleted

            };
            return cloneObj;

        }
        public virtual void Delete()
        {
            this.IsDeleted = true;
        }
    }
}
