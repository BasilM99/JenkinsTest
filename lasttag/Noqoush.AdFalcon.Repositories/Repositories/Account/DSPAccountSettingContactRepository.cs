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
    
    public class DSPAccountSettingContactRepository : RepositoryBase<DSPAccountSettingContact, int>, IDSPAccountSettingContactRepository
    {
        public DSPAccountSettingContactRepository(RepositoryImplBase<DSPAccountSettingContact, int> repository)
            : base(repository)
        {
        }

    }
}

