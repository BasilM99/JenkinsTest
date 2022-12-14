using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class KeywordMapping : ClassMap<Keyword>
    {
        public KeywordMapping()
        {
            Table("keywords");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Keyword'");
            Map(x => x.Usage, "`Usage`");
            Map(x => x.Code);
            Map(x => x.IsDeleted);
            Map(x => x.IsHidden);

            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}