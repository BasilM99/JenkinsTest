using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    
    public class lookalikejobMapping : ClassMap<lookalikejob>
    {
        public lookalikejobMapping()
        {
            Table("lookalike_jobs");
            Id(x => x.ID, "Id").GeneratedBy.Identity();
           
            Map(x => x.LookalikeAudienceListCode);

            Map(x => x.LookalikePercentage);
            Map(x => x.PopulationCountryFilter);
            Map(x => x.SeedAudienceListCode);

        }
    }
}
