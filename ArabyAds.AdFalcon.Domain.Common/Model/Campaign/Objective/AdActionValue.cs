//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Repositories.Core;
namespace Noqoush.AdFalcon.Domain.Model.Campaign.Objective
{
    public class AdActionValue : IEntity<int>
    {
        public virtual int ID { get; protected set; }
        public virtual AdActionTypeBase ActionType { get; set; }
        public virtual AdCreative AdCreative { get; set; }
        public virtual bool IsDeleted { get; set; }
        private string _ValueUpdated;
       
        public virtual string Value { get {

               return  _ValueUpdated;

            } set {
                if (!string.IsNullOrEmpty(value) && this.ActionType != null && this.ActionType.ID != (int)AdActionTypeIds.ClickToCall && this.ActionType.ID != (int)AdActionTypeIds.ClickToSMS)
                    _ValueUpdated = value.AddToQueryString(new object[] { "adf_sid", "{AD_SESSION_ID}" });
                else
                    _ValueUpdated = value;



            } }
        public virtual string Value2 { get; set; }

        public virtual IList<AdActionValueTracker> Trackers { get; set; }

        public virtual string GetDescription()
        {
            return Value;
        }


        public virtual AdActionValue Copy()
        {
            var cloneObj = new AdActionValue
                               {
                                   ActionType = this.ActionType,
                                   AdCreative = this.AdCreative,
                                   IsDeleted = this.IsDeleted,
                                   Value = this.Value,
                                   Value2 = this.Value2
                               };

            if (Trackers != null)
            {
                cloneObj.Trackers = new List<AdActionValueTracker>();

                foreach (var item in Trackers.Where(p => !p.IsDeleted))
                {
                    var clonedActionValueTracker = new AdActionValueTracker()
                    {
                        Url = item.Url,
                        AdActionValue = cloneObj
                    };

                    cloneObj.Trackers.Add(clonedActionValueTracker);
                }
            }

            return cloneObj;
        }
    }
}

