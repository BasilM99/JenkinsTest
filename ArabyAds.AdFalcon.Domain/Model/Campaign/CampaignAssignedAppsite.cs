using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class CampaignAssignedAppsite : IEntity<int>
    {
        public CampaignAssignedAppsite()
        {
           
        }
        public virtual ArabyAds.AdFalcon.Domain.Model.AppSite.SubAppsite SubAppsite { get; set; }

        public CampaignAssignedAppsite(Campaign campaign)
        {
            Campaign = campaign;
        }
        private const string _format = "{0}:{1}";
        public virtual int ID { get; set; }

        public virtual Campaign Campaign { get; set; }

        public virtual string SubPublisherId { get; set; }

        public virtual ArabyAds.AdFalcon.Domain.Model.AppSite.AppSite AppSite { get; set; }

        public virtual bool Include { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string GetDescription()
        {
            return string.Format(_format, Campaign.GetDescription(), AppSite.Name.ToString());
        }

        public virtual ArabyAds.AdFalcon.Domain.Model.Account.Account Account { get; set; }
        public virtual CampaignAssignedAppsite Clone()
        {
            var cloneObj = new CampaignAssignedAppsite()
            {
                Include = this.Include,
                IsDeleted = this.IsDeleted,
                AppSite = this.AppSite,

                Account = this.Account,
                SubAppsite = this.SubAppsite,


                SubPublisherId = this.SubPublisherId

            };
            return cloneObj;

        }
        public virtual void Delete()
        {
            this.IsDeleted = true;
        }
    }
}
