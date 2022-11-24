using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    public class PlatformVersion : IEntity<int>
    {
        public virtual Platform Platform { get; set; }

        public virtual string Version { get; set; }

        public virtual string Code { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual int ID
        {
            get;
            set;
        }

        public virtual string GetDescription()
        {
            return this.Version;
        }
    }
}
