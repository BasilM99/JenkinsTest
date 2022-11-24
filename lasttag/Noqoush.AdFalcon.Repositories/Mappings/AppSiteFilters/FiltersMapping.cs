using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.AppSite.Filtering;

namespace Noqoush.AdFalcon.Persistence.Mappings.AppSiteFilters
{
    public class AppSiteFilterMapping : ClassMap<AppSiteFilter>
    {
        public AppSiteFilterMapping()
        {
            Table("filters");
            Id(p => p.ID, "Id").GeneratedBy.HiLo(
                MappingSettings.HiLowTableName,
                MappingSettings._nextHi,
                MappingSettings._maxLo,
                "TableKey = 'Filter'");
            Map(p => p.IsDeleted);
            References(p => p.AppSite, "AppSiteId");

        }
    }

    public class UrlfilterMapping : SubclassMap<UrlFilter>
    {
        public UrlfilterMapping()
        {
            Table("urlfilters");
            KeyColumn("Id");
            Map(x => x.URL, "URL");

            Map(x => x.CIsDeleted).Column("IsDeleted");
            Map(x => x.CAppSiteId).Column("AppSiteId");
        }
    }

    public class TextFilterMapping : SubclassMap<TextFilter>
    {
        public TextFilterMapping()
        {
            Table("textfilters");
            References(x => x.MatchType, "MatchTypeId");
            KeyColumn("Id");
            Map(x => x.Text);

            Map(x => x.CIsDeleted).Column("IsDeleted");
            Map(x => x.CAppSiteId).Column("AppSiteId");
        }
    }

    public class LanguageFilterMapping : SubclassMap<LanguageFilter>
    {
        public LanguageFilterMapping()
        {
            Table("languagefilters");
            KeyColumn("Id");
            References(x => x.Language, "LanguageId");

            Map(x => x.CIsDeleted).Column("IsDeleted");
            Map(x => x.CAppSiteId).Column("AppSiteId");
        }
    }

    public class KeywordsFilterMapping : SubclassMap<KeywordsFilter>
    {
        public KeywordsFilterMapping()
        {
            Table("keywordsfilters");
            KeyColumn("Id");
            References(x => x.Keyword, "KeywordId");
        }
    }
}
