using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class BuyerMapping : ClassMap<Domain.Model.Account.Buyer>
    {
        public BuyerMapping()
        {
            Table("`buyers`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                      MappingSettings._nextHi,
                                      MappingSettings._maxLo,
                                      "TableKey = 'Buyer'");
            Map(x => x.Code);
            Map(x => x.Name);
          
        }
    }
}