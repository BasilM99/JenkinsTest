
using Noqoush.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
  
    public class AppMarketingPartnerTracker : IEntity<int>
    {
        public virtual int ID { get;  set; }
        public virtual int TypeID { get; set; }
        public virtual int? AdGroupID { get; set; }

      
        public virtual string TrackerUrlTemplate { get; set; }
        public virtual Platform Platform
        {
            get;
            set;
        }
        public virtual bool IsDeleted { get; set; }
        public virtual AppMarketingPartner AppMarketingPartner { get; set; }

        public virtual string ClickTrackerUrlTemplate { get; set; }
        public virtual string EventPostbackUrlTemplate { get; set; }

        public virtual string GetDescription()
        {
            return ClickTrackerUrlTemplate;
        }
    }
}
