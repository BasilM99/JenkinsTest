using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Common.Repositories;

using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Account
{
    [DataContract()]
    public enum AccountFundTypeIds
    {
        [EnumMember]
        Fund = 1,
        [EnumMember]
        Refund = 2,
        [EnumMember]
        ServiceCharge = 3
    }

    /*[Serializable]
    public partial class AccountFundType : LookupBase<AccountFundType, int>
    {
        #region Values
        private static IAccountFundTypeRepository _accountFundTypeRepository = null;
        private static IAccountFundTypeRepository AccountFundTypeRepository
        {
            get
            {
                if (_accountFundTypeRepository == null)
                {
                    _accountFundTypeRepository = Framework.IoC.Instance.Resolve<IAccountFundTypeRepository>();
                }
                return _accountFundTypeRepository;
            }
        }
        static AccountFundType _fund = null;
        static AccountFundType _reFund = null;
        static AccountFundType _serviceCharge = null;

        static readonly object LockObj = new object();
        public static AccountFundType ReFund
        {
            get
            {
                if (CheckStatus(_reFund))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_reFund))
                        {
                            _reFund = AccountFundTypeRepository.Get((int)AccountFundTypeIds.Refund);
                        }
                    }
                }
                return _reFund;
            }
        }
        public static AccountFundType Fund
        {
            get
            {
                if (CheckStatus(_fund))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_fund))
                        {
                            _fund = AccountFundTypeRepository.Get((int)AccountFundTypeIds.Fund);
                        }
                    }
                }
                return _fund;
            }
        }
        public static AccountFundType ServiceCharge
        {
            get
            {
                if (CheckStatus(_serviceCharge))
                {
                    lock (LockObj)
                    {
                        if (CheckStatus(_serviceCharge))
                        {
                            _serviceCharge = AccountFundTypeRepository.Get((int)AccountFundTypeIds.ServiceCharge);
                        }
                    }
                }
                return _serviceCharge;
            }
        }
        private static bool CheckStatus(AccountFundType type)
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

        public virtual short Multiplier
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            return Equals(obj as AccountFundType);
        }

        public virtual bool Equals(AccountFundType obj)
        {
            if (obj == null) return false;
            return Equals(ID, obj.ID);
        }
    }*/
}