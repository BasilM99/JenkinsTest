using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AdActionValueTracker : IEntity<int>
    {
        public virtual int ID { get; protected set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string Url { get; set; }

        public virtual AdActionValue AdActionValue { get; set; }

        public virtual string GetDescription()
        {
            return this.Url;
        }
    }
}
