using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;
namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{

    public class AudienceSegmentMapping : ClassMap<AudienceSegment>
    {

        public AudienceSegmentMapping()
        {
            Table("audience_segments");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'AudienceSegment'");
            References(x => x.Parent, "ParentId");

            References(x => x.Provider, "ProviderId");
            References(x => x.Name, "NameId").Cascade.All().Not.LazyLoad();
            Map(M=>M.Selectable);
            Map(M => M.Activated).Not.Update();
            Map(M => M.OperatorSegmentCode, "OperatorSegmentCode");
            Map(M => M.Code, "SegmentCode");
            Map(M => M.Description);
            Map(M => M.IsDeleted);
            Map(M => M.IsPermissionNeed);
            Map(M => M.BinIndex).Nullable();
            
            //Map(M => M.IntegrationId);
            References(x => x.Advertiser, "AdvertiserAccId");

            References(x => x.Account, "AccountId");
            References(x => x.User, "UserId");
            References(x => x.CostModel, "CostModelId");
            Map(M => M.Price);
            Cache.Transactional().ReadWrite().IncludeAll();
        }




    }

    public class AudienceSegmentOccupationMapping : ClassMap<AudienceSegmentOccupation>
    {

        public AudienceSegmentOccupationMapping()
        {
            Table("audience_list_bins_occupation");
            Id(x => x.ID);
           
            Map(M => M.BinIndex);
            Map(M => M.NumberOfSegments);
            
            Cache.Transactional().ReadWrite().IncludeAll();
        }




    }


}
