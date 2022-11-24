using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Repositories.Account
{
    public class AccountInvitationCriteria : CriteriaBase<Model.Account.AccountInvitation>
    {
        public int id { get; set; }
        public string invitationcode { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public string EmailAddress { get; set; }
        public int accountid { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }



        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Account.AccountInvitationCriteria Commoncr)
        {









            Page = Commoncr.Page;

            Size = Commoncr.Size;
            EmailAddress = Commoncr.EmailAddress;

            accountid = Commoncr.accountid;


            id = Commoncr.id;
            invitationcode = Commoncr.invitationcode;

            DataFrom = Commoncr.DataFrom;
            DataTo = Commoncr.DataTo;

        }
        public override Expression<Func<Model.Account.AccountInvitation, bool>> GetExpression()
        {

            if (EmailAddress == null)
            {
                EmailAddress = string.Empty;
            }
            Expression<Func<Model.Account.AccountInvitation, bool>> filter =
                (c => c.Account.ID == accountid
                && (string.IsNullOrEmpty(EmailAddress) || c.EmailAddress.ToLower().Contains(EmailAddress.ToLower()))
                );
            return filter;
        }

        public override Func<Model.Account.AccountInvitation, bool> GetWhere()
        {
            // Func<Model.Campaign.Campaign, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId) && c.Account.ID == AccountId);
            Func<Model.Account.AccountInvitation, bool> filter = (c => c.Account.ID == accountid && (string.IsNullOrEmpty(EmailAddress) || c.EmailAddress.ToLower().Contains(EmailAddress.ToLower())));
            return filter;
        }
    }
}
