using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.DomainServices;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Domain.Model.Account
{
    //[DataContract()]
    //public enum AccountFundTransTypeIds
    //{
    //    [EnumMember]
    //    CreditCard = 1,
    //    [EnumMember]
    //    WireTransfer = 2,
    //    [EnumMember]
    //    Cash = 3,
    //    [EnumMember]
    //    BalanceTransfer = 4,
    //    [EnumMember]
    //    BankCheck = 5,
    //    [EnumMember]
    //    FreeCredit = 6,
    //    [EnumMember]
    //    PayPal = 7,
    //    [EnumMember]
    //    FundTransfer = 8,
    //    [EnumMember]
    //    OverBudgetRefund = 9
    //}

    [Serializable]
    public partial class AccountFundTransType : LookupBase<AccountFundTransType, int>
    {
        #region Values
        /*
         CreditCard = 1,
        WireTransfer = 2,
        Cash = 3,
        BalanceTransfer = 4,
        BankCheck = 5,
        FreeCredit = 6,
        PayPal=7*/
        private static IAccountFundTransTypeRepository _accountFundTransTypeRepository = null;
        private static IAccountFundTransTypeRepository AccountFundTransTypeRepository
        {
            get
            {
                if (_accountFundTransTypeRepository == null)
                {
                    _accountFundTransTypeRepository = Framework.IoC.Instance.Resolve<IAccountFundTransTypeRepository>();
                }
                return _accountFundTransTypeRepository;
            }
        }
        static AccountFundTransType _wireTransfer = null;
        static AccountFundTransType _creditCard = null;
        static AccountFundTransType _cash = null;
        static AccountFundTransType _balanceTransfer = null;
        static AccountFundTransType _check = null;
        static AccountFundTransType _freeCredit = null;
        static AccountFundTransType _payPal = null;
        static AccountFundTransType _fundTransfer = null;
        static AccountFundTransType _overBudgetRefund = null;
        static readonly object LockObj = new object();


        public static AccountFundTransType OverBudgetRefund
        {
            get
            {
                if (CheckStatus(_overBudgetRefund))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_overBudgetRefund))
                        {
                            _overBudgetRefund = AccountFundTransTypeRepository.Get((int)AccountFundTransTypeIds.OverBudgetRefund);
                        }
                    }
                }
                return _overBudgetRefund;
            }
        }
        public static AccountFundTransType FundTransfer
        {
            get
            {
                if (CheckStatus(_fundTransfer))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_fundTransfer))
                        {
                            _fundTransfer = AccountFundTransTypeRepository.Get((int)AccountFundTransTypeIds.FundTransfer);
                        }
                    }
                }
                return _fundTransfer;
            }
        }
        public static AccountFundTransType CreditCard
        {
            get
            {
                if (CheckStatus(_creditCard))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_creditCard))
                        {
                            _creditCard = AccountFundTransTypeRepository.Get((int)AccountFundTransTypeIds.CreditCard);
                        }
                    }
                }
                return _creditCard;
            }
        }
        public static AccountFundTransType WireTransfer
        {
            get
            {
                if (CheckStatus(_wireTransfer))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_wireTransfer))
                        {
                            _wireTransfer = AccountFundTransTypeRepository.Get((int)AccountFundTransTypeIds.WireTransfer);
                        }
                    }
                }
                return _wireTransfer;
            }
        }
        public static AccountFundTransType Cash
        {
            get
            {
                if (CheckStatus(_cash))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_cash))
                        {
                            _cash = AccountFundTransTypeRepository.Get((int)AccountFundTransTypeIds.Cash);
                        }
                    }
                }
                return _cash;
            }
        }
        public static AccountFundTransType BalanceTransfer
        {
            get
            {
                if (CheckStatus(_balanceTransfer))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_balanceTransfer))
                        {
                            _balanceTransfer = AccountFundTransTypeRepository.Get((int)AccountFundTransTypeIds.BalanceTransfer);
                        }
                    }
                }
                return _balanceTransfer;
            }
        }
        public static AccountFundTransType Check
        {
            get
            {
                if (CheckStatus(_check))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_check))
                        {
                            _check = AccountFundTransTypeRepository.Get((int)AccountFundTransTypeIds.BankCheck);
                        }
                    }
                }
                return _check;
            }
        }
        public static AccountFundTransType FreeCredit
        {
            get
            {
                if (CheckStatus(_freeCredit))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_freeCredit))
                        {
                            _freeCredit = AccountFundTransTypeRepository.Get((int)AccountFundTransTypeIds.FreeCredit);
                        }
                    }
                }
                return _freeCredit;
            }
        }
        public static AccountFundTransType PayPal
        {
            get
            {
                if (CheckStatus(_payPal))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_payPal))
                        {
                            _payPal = AccountFundTransTypeRepository.Get((int)AccountFundTransTypeIds.PayPal);
                        }
                    }
                }
                return _payPal;
            }
        }
        private static bool CheckStatus(AccountFundTransType type)
        {
            if (type != null)
            {
                try
                {
                    type.Name.ToString();
                    return false;
                }
                catch
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        #endregion

        public virtual bool AllowImpersonate
        {
            get;
            set;
        }
        //public virtual ISet<AccountFundTransHistory> AccountFundTransHistories
        //{
        //    get;
        //    set;
        //}

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            return Equals(obj as AccountFundTransType);
        }

        public virtual bool Equals(AccountFundTransType obj)
        {
            if (obj == null) return false;
            return Equals(ID, obj.ID);
        }

        //public override int GetHashCode()
        //{
        //    int result = 1;

        //    result = (result * 397) ^ AllowImpersonate.GetHashCode();
        //    result = (result * 397) ^ Id.GetHashCode();
        //    result = (result * 397) ^ IsActive.GetHashCode();
        //    result = (result * 397) ^ NameId.GetHashCode();
        //    return result;
        //}
    }
}