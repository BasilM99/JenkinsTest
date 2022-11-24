using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting
{
    public class GeoFencingTargeting : TargetingBase
    {
        public virtual decimal Latitude { get; set; }

        public virtual decimal Longitude { get; set; }

        public virtual decimal Radius { get; set; }
        public override string GetDescription()
        {
            return string.Format("{0}:{1}", Latitude.ToString(), Longitude.ToString()) ;
        }
        public override TargetingBase Copy()
        {
            var cloneObj = new GeoFencingTargeting()
            {
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                Radius = this.Radius,
                AdGroup = this.AdGroup,
                Type = this.Type,
                IsDeleted = this.IsDeleted
            };
            return cloneObj;
        }
    }
}
