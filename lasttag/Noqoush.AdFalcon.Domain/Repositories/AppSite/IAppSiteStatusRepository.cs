using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories
{
    public interface IAppSiteStatusRepository : IKeyedRepository<AppSiteStatus, int>
    {

    }
}
