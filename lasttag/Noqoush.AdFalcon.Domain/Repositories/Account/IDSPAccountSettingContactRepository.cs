using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Repositories.Account
{
   

    public interface IDSPAccountSettingContactRepository : IKeyedRepository<DSPAccountSettingContact, int>
    {
     
    }
}
