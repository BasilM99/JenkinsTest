using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Account.SSP
{
    public class SSPPartnerSupportedCreativeFormats : IEntity<int>
    {
        public virtual int ID { set; get; }
        public virtual EnvironmentType EnvironmentType { get; set; }

        public virtual BusinessPartner Partner
        {
            get;
            set;
        }
        public virtual CreativeFormat CreativeFormat
        {
            get;
            set;
        }

        public virtual string GetDescription()
        {
            return "Partner : " + Partner.Name + " , CreativeFormat : " + CreativeFormat.GetDescription();
        }

        public virtual bool IsDeleted { get; set; }

    }
}
