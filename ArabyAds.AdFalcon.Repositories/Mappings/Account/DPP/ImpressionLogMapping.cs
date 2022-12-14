using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using FluentNHibernate;
using ArabyAds.AdFalcon.Domain.Model.Account.DPP;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.DPP;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account.DPP
{
    public class ImpressionLogMapping : ClassMap<Domain.Model.Account.DPP.ImpressionLog>
    {
        public ImpressionLogMapping()
        {
            Table("data_provider_logs");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                      MappingSettings._nextHi,
                                      MappingSettings._maxLo,
                                      "TableKey = 'ImpressionLogs'");
            Map(x => x.Day);
            Map(x => x.CreationDate);
            Map(x => x.LastUpdate);

            Map(x => x.Type, "logtype").CustomType(typeof(ImpressionLogType)); ;
            Map(x => x.IsDeleted);
            Map(x => x.Path);
            Map(X => X.Written);
            References(x => x.Provider, "dataproviderid");
        }
    }
}