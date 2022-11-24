using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.DomainServices.AuditTrial;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account
{
    public interface IAccountSummaryRepository:IKeyedRepository<Model.Account.AccountSummary, int>
    {
    }
}
