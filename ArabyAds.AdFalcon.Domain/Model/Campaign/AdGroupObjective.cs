﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AdGroupObjective : IEntity<int>
    {
        public virtual int ID { get; protected set; }
        public virtual bool IsDeleted { get; set; }

        public virtual AdType AdType { get; set; }

        public virtual AdGroup AdGroup
        {
            get;
            set;
        }

        public virtual AdActionTypeBase AdAction
        {
            get;
            set;
        }

        public virtual AdGroupObjectiveType Objective
        {
            get;
            set;
        }
        public virtual string GetDescription()
        {
            return string.Format("{0}-{1}",Objective.GetDescription(),AdAction.GetDescription());
        }

        public virtual AdGroupObjective Copy()
        {
            var cloneObj = new AdGroupObjective()
                               {
                                  AdAction = this.AdAction,
                                  AdType = this.AdType,
                                  Objective = this.Objective,
                                  AdGroup = this.AdGroup,
                                  IsDeleted = this.IsDeleted
                               };
            return cloneObj;
        }

    }
}

