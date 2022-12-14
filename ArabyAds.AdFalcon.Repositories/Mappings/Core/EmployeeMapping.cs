using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class EmployeeMapping : SubclassMap<Employee>
    {
        public EmployeeMapping()
           
        {
            Table("employees");
            KeyColumn("Id");
            References(x => x.JobPosition, "job_position_id").Cascade.None() ;
            References(x => x.Account, "AccountId").LazyLoad().Cascade.None(); 
         
        }
    }
}