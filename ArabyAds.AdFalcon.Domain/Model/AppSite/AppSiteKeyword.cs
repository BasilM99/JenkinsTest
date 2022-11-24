using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.AppSite
{
    public class AppSiteKeyword : IEntity<int>
    {
        public AppSiteKeyword()
        {

        }

        public AppSiteKeyword(AppSite appsite)
        {
            this.AppSite = appsite;
        }

        public virtual int ID { get; protected set; }
        public virtual bool IsDeleted { get; set; }

        public virtual Keyword Keyword { get; set; }
        public virtual AppSite AppSite { get; set; }

        public virtual string GetDescription()
        {
            return Keyword.GetDescription();
        }
    }
}
