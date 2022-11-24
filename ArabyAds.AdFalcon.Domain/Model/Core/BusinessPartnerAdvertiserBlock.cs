using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArabyAds.AdFalcon.Domain.Model.Core
{
   

    public class BusinessPartnerAdvertiserBlock : IEntity<int>
    {
     
        public virtual BusinessPartner Partner { get; set; }
        public virtual Advertiser Advertiser { get; set; }
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string Description { get; set; }
        public virtual string GetDescription()
        {

            return string.Format("{0}-{1}", Advertiser.GetDescription(), Partner.Name);
        }
     




    }


    public class BusinessPartnerAccountWhite : IEntity<int>
    {

        public virtual BusinessPartner Partner { get; set; }
        public virtual Account.Account Account { get; set; }
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string Description { get; set; }
        public virtual string GetDescription()
        {

            return string.Format("{0}-{1}", Account.GetDescription(), Partner.Name);
        }





    }

    public class BusinessPartnerDomainBlock : IEntity<int>
    {

        public virtual BusinessPartner Partner { get; set; }
        public virtual string Domain { get; set; }
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string Description { get; set; }
        public virtual string GetDescription()
        {

            return string.Format("{0}-{1}", Domain, Partner.Name);
        }





    }
    
}
