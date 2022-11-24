
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account
{
    public class AccountInvitationCriteria 
    {
        public int id { get; set; }
        public string invitationcode { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public string EmailAddress { get; set; }
        public int accountid { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }

    }
}
