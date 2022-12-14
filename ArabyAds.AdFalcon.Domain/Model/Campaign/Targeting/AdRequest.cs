//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting
{
    public class AdRequestTargeting :TargetingBase
    {

      
        public override string GetDescription()
        {
            return MinimumVersion.ToString();
        }
        public override TargetingBase Copy()
        {
            var cloneObj = new AdRequestTargeting()
            {
                AdRequestType = this.AdRequestType,
                AdRequestPlatform = this.AdRequestPlatform,
                AdGroup = this.AdGroup,
                Type = this.Type,
                IsDeleted = this.IsDeleted
            };
            return cloneObj;
        }


 


        public virtual AdRequestType AdRequestType { get;  set; }
        public virtual AdRequestPlatform AdRequestPlatform { get; set; }

        public virtual string MinimumVersion { get; set; }
     
    }
}

