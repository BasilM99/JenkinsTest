using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Persistence.Mappings.AppSite
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
