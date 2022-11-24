using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
   
    public class AdCreativeUnitAttributeMapping : IEntity<int>
    {
        public virtual int ID { get; protected set; }

        public virtual bool IsDeleted { get; set; }

        public virtual AdCreativeAttribute Attribute { get; set; }

        public virtual AdCreativeUnit AdCreativeUnit { get; set; }

        public virtual string GetDescription()
        {
            return this.Attribute.GetDescription();
        }
    }
}
