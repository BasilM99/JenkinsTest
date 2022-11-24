using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Model.Core;


namespace ArabyAds.AdFalcon.Persistence.Mappings.Account.PMP
{


    public class PMPDealMapping : ClassMap<PMPDeal>
    {
        public PMPDealMapping()
        {
            Table("`buyer_deals`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'PMPDeal'");
            Map(x => x.Name);
            Map(x => x.IsDeleted);
            Map(x => x.IsGlobal);



            Map(x => x.DealID,"Code");
            Map(x => x.Description);

            Map(x => x.Note);
            Map(x => x.EndDate);
            Map(x => x.StartDate);
            Map(x => x.CreationDate);
            Map(x => x.UniqueId).Not.Update();
            Map(x => x.Price);
            Map(x => x.PublisherName);
            Map(x => x.Type, "TypeId").CustomType(typeof(DealType));

            References(x => x.Advertiser, "AdvertiserId");
            References(x => x.AdvertiserAccount, "AssociationAdvId");
            References(x => x.Account, "AccountId").Not.Nullable();
            References(x => x.User, "UserId").Not.Nullable();
            References(x => x.Publisher, "PublisherId").Nullable();
            References(x => x.Exchange, "ExchangeId").Not.Nullable();
            HasMany(d => d.Targetings).KeyColumn("PMPDealId").Cascade.AllDeleteOrphan();
        }
    }
}
