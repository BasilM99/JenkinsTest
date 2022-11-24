using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Linq;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;

using System.Collections.Generic;
using System;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
   


    public class ContextualPartnerRepository : RepositoryBase<ContextualPartner, int>, IContextualPartnerRepository
    {
        public ContextualPartnerRepository(RepositoryImplBase<ContextualPartner, int> repository)
            : base(repository)
        {


        }
      
    }
}
