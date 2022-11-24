using ArabyAds.Framework.DomainServices;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class CampaignDiscount : IEntity<int>
    {
        private const string _format = "{0}:{1}";
        public virtual Model.Campaign.Campaign Campaign { get; set; }
        public virtual Core.Discount Discount { get; set; }
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string GetDescription()
        {
            return string.Format(_format, Campaign.GetDescription(), Discount.GetDescription());
        }
        public virtual void DeActive()
        {
            this.Discount.DeActive();
        }


    }
}
