using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class AdGroupBidConfig : IEntity<int>
    {
        public virtual int ID { get; set; }

        public virtual AdGroup AdGroup { get; set; }

        public virtual Noqoush.AdFalcon.Domain.Model.Account.Account Account { get; set; }

        public virtual string SubPublisherId { get; set; }

        public virtual Noqoush.AdFalcon.Domain.Model.AppSite.AppSite AppSite { get; set; }

        public virtual decimal Bid { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string GetDescription()
        {
            return this.Bid.ToString();
        }
        public virtual SubAppsite SubAppSite { get; set; }


        public virtual decimal GetUserReadableValue()
        {

            var appsite = this.AppSite;
            var adgroup = this.AdGroup;
            int factor;

            if (appsite != null && adgroup != null)
            {
                if (!appsite.AppSiteServerSetting.IsUsingCampaignPricingModel)
                {
                    if (appsite.AppSiteServerSetting.GetPricingModel() != null)
                        factor = AppSite.AppSiteServerSetting.GetPricingModel().Factor;
                    else
                        throw new InvalidOperationException("AppSite's PricingModel should not be null");
                }
                else
                { //we don't use those lines for now , but we might later 

                    if (adgroup.CostModelWrapper != null)
                        factor = adgroup.CostModelWrapper.Factor;
                    else
                        throw new InvalidOperationException("adgroup's CostModelWrapper should not be null");
                }
            }
            else
            {
                throw new InvalidOperationException("AppSite and adgroup should not be null");
            }

            return this.Bid * factor;

        }

        public virtual void SetAdGroupBidConfigsBid(decimal value)
        {


            var appsite = this.AppSite;
            var adgroup = this.AdGroup;
            int factor;

            if (appsite != null && adgroup != null)
            {
                if (!appsite.AppSiteServerSetting.IsUsingCampaignPricingModel)
                {
                    if (appsite.AppSiteServerSetting.GetPricingModel() != null)
                        factor = AppSite.AppSiteServerSetting.GetPricingModel().Factor;
                    else
                        throw new InvalidOperationException("AppSite's PricingModel should not be null");
                }
                else
                {
                    if (adgroup.CostModelWrapper != null)
                        factor = adgroup.CostModelWrapper.Factor;
                    else
                        throw new InvalidOperationException("adgroup's CostModelWrapper should not be null");
                }
            }
            else
            {
                throw new InvalidOperationException("AppSite and adgroup should not be null");
            }

            this.Bid = value / factor;
        }



        public virtual void Delete()
        {
            this.IsDeleted = true;
        }
    }
}
