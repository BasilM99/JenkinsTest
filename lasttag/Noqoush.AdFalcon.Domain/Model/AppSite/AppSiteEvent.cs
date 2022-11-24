
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.AppSite
{
    public class AppSiteEvent : IEntity<int>
    {
        private static ITrackingEventRepository _TrackingEventRepository = Framework.IoC.Instance.Resolve<ITrackingEventRepository>();


        public virtual int ID { get; protected set; }
        public virtual bool IsDeleted { get; set; }
        public virtual TrackingEvent Event { get; set; }
        public virtual bool IsBillable { get; set; }

        [PropertyDescriptorValue("Event")]
        public virtual decimal? MinBid { get; set; }
        public virtual string MinBidDescriper
        {
            get
            {
                if (MinBid.HasValue)
                {
                    return Event.CostModelWrapper.Factor.ToString();
                }
                else
                    return string.Empty;


            }
        }
        public virtual AppSiteServerSetting AppSiteServerSetting { get; set; }

        public virtual string GetDescription()
        {
            return Event.GetDescription();
        }

        public virtual string getMinBidValue(string value, string id)
        {
            decimal factor = 0;
            decimal returnValue = 0;

            if(!string.IsNullOrEmpty(id))
             factor = _TrackingEventRepository.Get(Convert.ToInt32(id)).CostModelWrapper.Factor;
            if (!string.IsNullOrEmpty(value))
                returnValue = Convert.ToDecimal(value) * factor;

            return returnValue.ToString("F2");

        }
    }
}
