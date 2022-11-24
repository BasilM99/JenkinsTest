
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account
{
    public class AuditTrialCriteria
    {
        public int? AccountId { get; set; }
        public int? UserId { get; set; }

        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }

        public string UserName { get; set; }
        public int Type { get; set; }
        public int ObjectRootId { get; set; }
       // public int ObjectRootId { get; set; }
    }
}
