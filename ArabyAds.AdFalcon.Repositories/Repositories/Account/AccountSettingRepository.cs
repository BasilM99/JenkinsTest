using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Repositories.Account;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Account
{
    
        public class DSPAccountSettingRepository : RepositoryBase<DSPAccountSetting, int>, IDSPAccountSettingRepository
    {
            public DSPAccountSettingRepository(RepositoryImplBase<DSPAccountSetting, int> repository)
                : base(repository)
            {
            }

        }
    }
