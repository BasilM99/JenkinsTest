
using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Performance
{
    public class CampaignPerformance
    {
        public virtual int ID { get; protected set; }
        public virtual int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual decimal Spend { get; set; }
    }
}

