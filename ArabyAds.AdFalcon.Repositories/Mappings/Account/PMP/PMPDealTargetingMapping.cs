using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account.PMP
{
   

    public class PMPDealTargetingMapping : ClassMap<PMPDealTargeting>
    {
        public PMPDealTargetingMapping()
        {
            Table("`pmp_targetings`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'PMPDealTargeting'");
    
            Map(x => x.IsDeleted);


       
       
            Map(x => x.Type, "TypeId").CustomType(typeof(PMPDealTargetingType));



            References(x => x.Deal, "PMPDealId").Not.Nullable();
           
        }
    }


    public class GeographicPMPDealTargetingMapping : SubclassMap<GeographicPMPDealTargeting>
    {
        public GeographicPMPDealTargetingMapping()
        {
            Table("pmpdeal_geographictargetings");
            KeyColumn("Id");
            References(x => x.Location, "LocationId");
        }
    }
    public class AdSizePMPDealTargetingMapping : SubclassMap<AdSizePMPDealTargeting>
    {
        public AdSizePMPDealTargetingMapping()
        {
            Table("pmpdeal_adsizetargetings");
            KeyColumn("Id");
            References(x => x.AdSize, "AdSizeId");
        }
    }

    public class AdTypeGroupPMPDealTargetingMapping : SubclassMap<AdTypeGroupPMPDealTargeting>
    {
        public AdTypeGroupPMPDealTargetingMapping()
        {
            Table("pmpdeal_adtypegroupetargetings");
            KeyColumn("Id");
            Map(x => x.AdTypeGroup, "AdGroupTypeId").CustomType(typeof(AdTypeGroup));
        }
    }

}
