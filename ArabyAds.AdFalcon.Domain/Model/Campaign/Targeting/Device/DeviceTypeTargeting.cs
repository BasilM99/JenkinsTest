using ArabyAds.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.Device
{
    public class DeviceTypeTargeting : DeviceTargetingBase
    {
        public override string GetDescription()
        {
            return DeviceType.GetDescription();
        }
        public virtual DeviceType DeviceType { get; set; }

        public override ITargetingBase Copy()
        {
            var cloneObj = new DeviceTypeTargeting()
            {
                DeviceType = this.DeviceType,
                IsDeleted = this.IsDeleted,
                DeviceTargeting = this.DeviceTargeting,
                IsAll = this.IsAll
            };
            return cloneObj;
        }
    }
}
