using ArabyAds.Framework.Persistence;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account
{
    [ProtoContract]
    public class AccountInvitationCriteria : CriteriaBase<Model.Account.AccountInvitation>
    {
        [ProtoMember(1)]
        public int id { get; set; }
        [ProtoMember(2)]
        public string invitationcode { get; set; }
        [ProtoMember(3)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(4)]
        public DateTime? DataTo { get; set; }
        [ProtoMember(5)]
        public string EmailAddress { get; set; }
        [ProtoMember(6)]
        public int accountid { get; set; }
        [ProtoMember(7)]
        public int? Page { get; set; }
        [ProtoMember(8)]
        public int Size { get; set; }



        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.AccountInvitationCriteria Commoncr)
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
