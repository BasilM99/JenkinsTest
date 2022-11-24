using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.Framework.DomainServices.AuditTrial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Services
{
    public class AudittrialsService : IAudittrialsService
    {
        private IAuditTrialRepository _AuditTrialRepository;

        public AudittrialsService(IAuditTrialRepository AuditTrialRepository)
        {
            this._AuditTrialRepository = AuditTrialRepository;

        }
        public void test()
        {
            int gg = 0;
            //var testing = _AuditTrialRepository.GeAuditTrialMainRoots(0, 0, 0, null, null, 0,out gg);


        }


    }
}
