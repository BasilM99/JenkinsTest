using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories
{
    public interface IAppSiteRepository : IKeyedRepository<AppSite, int>
    {
        IEnumerable<SubAppsite> QueryByCratiriaForSubAppSite(AllAppSiteCriteria criteria);
        int QueryByCratiriaForSubAppSiteCount(AllAppSiteCriteria criteria);
        string GetObjectName(int Id);
        int getAccountId(int id);
        AppSiteServerSetting getServerSetting(int id);
    }
}
