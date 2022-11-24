using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
   
    public class ClickTagTracker : IEntity<int>
    {
        public virtual int ID { get; protected set; }
        public virtual bool IsDeleted { get; set; }

        public virtual AdCreative Creative { get; set; }

        public virtual string TrackingUrl { get; set; }
        public virtual string VariableName { get; set; }
        public virtual string GetDescription()
        {
            return this.VariableName + "-"+this.TrackingUrl;
        }
        public virtual ClickTagTracker Clone()
        {

            return new ClickTagTracker { Creative = this.Creative, TrackingUrl = this.TrackingUrl, VariableName = this.VariableName, IsDeleted = this.IsDeleted };
        }

        }


    public class ThirdPartyTracker : IEntity<int>
    {
        public virtual int ID { get;  set; }
        public virtual bool IsDeleted { get; set; }

        public virtual AdCreative Creative { get; set; }

        public virtual string VendorID { get; set; }
        public virtual string ScriptURL { get; set; }

        public virtual string ExecutionErrorTrackerURL { get; set; }

        public virtual string ParametersURL { get; set; }

        public virtual string GetDescription()
        {
            return this.VendorID + "-" + this.ScriptURL;
        }
        public virtual ThirdPartyTracker Clone()
        {

            return new ThirdPartyTracker { Creative = this.Creative, VendorID = this.VendorID, ScriptURL = this.ScriptURL, ExecutionErrorTrackerURL = this.ExecutionErrorTrackerURL, ParametersURL = this.ParametersURL, IsDeleted = this.IsDeleted };
        }

    }
}
