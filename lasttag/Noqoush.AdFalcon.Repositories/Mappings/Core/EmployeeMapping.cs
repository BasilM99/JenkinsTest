using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
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