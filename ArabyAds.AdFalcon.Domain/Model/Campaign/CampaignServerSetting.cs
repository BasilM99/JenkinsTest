using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class CampaignServerSetting : IEntity<int>
    {
        public CampaignServerSetting()
        {
        }

        public CampaignServerSetting(Campaign campaign)
        {
            Campaign = campaign;
        }

        public virtual int ID { get; protected set; }

        public virtual Campaign Campaign { get; protected set; }
        private int CampaignId { get; set; }

        public virtual int? AdRequestCacheLifeTime { get; set; }

        public virtual IList<CampaignFrequencyCapping> FrequencyCappingList { get; set; }
        public virtual AgencyCommission? AgencyCommission { get; set; }
        public virtual decimal AgencyCommissionValue { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual string GetDescription()
        {

            return string.Format("{0}:{1} {2}", Campaign.Name, Campaign.ID, Framework.Resources.ResourceManager.Instance.GetResource("CampaignSettings", "Titles"));
        }
        public virtual void setAgencyCommission(AgencyCommission agC)
        {
            if (agC==Common.Model.Account.AgencyCommission.Undefined)
            {
                this.AgencyCommission = null;
            }
            else
            {
                this.AgencyCommission = agC;
            }

        }

        public virtual AgencyCommission getAgencyCommission()
        {
            if (this.AgencyCommission.HasValue)
                return this.AgencyCommission.Value;
            else
                return ArabyAds.AdFalcon.Domain.Common.Model.Account.AgencyCommission.Undefined;

        }
        public virtual IList<CampaignFrequencyCapping> GetFrequencyCappingList()
        {
            if (FrequencyCappingList == null)
                return new List<CampaignFrequencyCapping>();

            return FrequencyCappingList.Where(p => !p.IsDeleted).ToList();
        }
    }
}
