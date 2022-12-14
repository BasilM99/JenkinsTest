using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
   public  class CampaignReportRecipient : IEntity<int>
    {
        public virtual int ID { get; private set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string Email { get; set; }

        public virtual CampaignReportScheduler CampaignReportScheduler { get; set; }

        public virtual string GetDescription()
        {
            return this.Email;
        }
    }

}
