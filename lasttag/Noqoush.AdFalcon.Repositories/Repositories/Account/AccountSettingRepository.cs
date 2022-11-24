using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Repositories.Account;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Repositories.Account
{
    
        public class DSPAccountSettingRepository : RepositoryBase<DSPAccountSetting, int>, IDSPAccountSettingRepository
    {
            public DSPAccountSettingRepository(RepositoryImplBase<DSPAccountSetting, int> repository)
                : base(repository)
            {
            }

        }
    }
