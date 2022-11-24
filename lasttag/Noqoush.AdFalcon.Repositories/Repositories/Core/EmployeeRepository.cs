using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class EmployeeRepository : RepositoryBase<Employee, int>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryImplBase<Employee, int> repository)
            : base(repository)
        {

        }
    }
}
