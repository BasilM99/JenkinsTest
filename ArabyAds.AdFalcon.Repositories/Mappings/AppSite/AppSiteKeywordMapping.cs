using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Persistence.Mappings.AppSite
{
    public class AppSiteKeywordMapping : ClassMap<AppSiteKeyword>
    {
        public AppSiteKeywordMapping()
        {
            Table("appsitekeywords");
            Id(x => x.ID).GeneratedBy.Identity();
            Map(p => p.IsDeleted);
            References(x => x.AppSite, "AppSiteId").ReadOnly().Cascade.None();
            References(x => x.Keyword, "KeywordId");
        }
    }
}
