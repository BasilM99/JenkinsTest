using System;
using System.Collections.Generic;

namespace Noqoush.AdFalcon.Domain.Model.Account
{
    [Serializable]
    public partial class AccountFundTransHistoryPgw : AccountFundTransHistory
    {
        public AccountFundTransHistoryPgw()
        {
        }

        public virtual BankAccountPaymentDetails SystemPaymentDetail { get; set; }

        public virtual string ErrorCode { get; set; }

        public virtual int AccountFundPgwId { get; set; }

        public virtual string ExtraInfo { get; set; }
        public virtual string PgwStatus { get; set; }
        public virtual string ReceiptNumber { get; set; }

        public virtual DateTime? ResponseDate { get; set; }
        //public virtual AccountFundPgw AccountFundPgw
        //{
        //    get;
        //    set;
        //}


    }
}