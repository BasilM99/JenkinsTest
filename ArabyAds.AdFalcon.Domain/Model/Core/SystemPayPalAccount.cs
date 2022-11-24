using System;
using ArabyAds.Framework.DomainServices;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    public class SystemPayPalAccount : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime ActiveFrom { get; set; }
        public virtual DateTime? ActiveTo { get; set; }

        public virtual string GetDescription()
        {
            return UserName;
        }
    }
}

