//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using System.Collections.Generic;
using ArabyAds.Framework.DomainServices;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{


    public class SubtypeAction : IEntity<int>
    {

        public virtual int ID { set; get; }

        public virtual AdSubType SubType { set; get; }

        public virtual AdActionTypeBase ActionType { set; get; }

        public virtual bool IsDeleted { set; get; }

        public virtual string GetDescription()
        {
            return "";

        }

    }

}
