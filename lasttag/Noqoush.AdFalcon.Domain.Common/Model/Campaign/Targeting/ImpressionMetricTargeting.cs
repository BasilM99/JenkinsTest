﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting
{
    public class ImpressionMetricTargeting : TargetingBase
    {


        public override string GetDescription()
        {
            return ImpressionMetric.GetDescription();
        }
        public override TargetingBase Copy()
        {
            var cloneObj = new ImpressionMetricTargeting()
            {
                ImpressionMetric = this.ImpressionMetric,
                MinValue = this.MinValue,
                Ignore = this.Ignore,
                AdGroup = this.AdGroup,
                Type = this.Type,
                IsDeleted = this.IsDeleted
            };
            return cloneObj;
        }




        public virtual MetricVendor MetricVendor { get; set; }

        public virtual ImpressionMetric ImpressionMetric { get; set; }
        public virtual float MinValue { get; set; }

        public virtual bool Ignore { get; set; }

    }
}

