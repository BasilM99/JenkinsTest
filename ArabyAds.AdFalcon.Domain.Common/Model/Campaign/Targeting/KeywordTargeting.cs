//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting
{
    public class KeywordTargeting : TargetingBase
    {
        public virtual Keyword Keyword{get;set;}

        public virtual bool Include { get; set; }

        public override string GetDescription()
        {
            return Keyword.GetDescription();
        }
        public override TargetingBase Copy()
        {
            var cloneObj = new KeywordTargeting()
            {
                Keyword = this.Keyword,
                AdGroup = this.AdGroup,
                Include=this.Include,
                Type = this.Type,
                IsDeleted = this.IsDeleted
            };
            return cloneObj;
        }
    }
}

