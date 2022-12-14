using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;

namespace Noqoush.AdFalcon.Domain.Model.Account
{
    //[DataContract()]
    //public enum AccountFundTransStatusIds
    //{
    //    [EnumMember]
    //    Committed = 0,
    //    [EnumMember]
    //    Pending = 1,
    //    [EnumMember]
    //    Failed = 2,
    //}

    [Serializable]
    public partial class AccountFundTransStatus : LookupBase<AccountFundTransStatus, int>
    {

        #region Values
        /*
         * Committed 0
         * Pending 1
         * Failed 2
         */
        private static IAccountFundTransStatusRepository _accountFundTransStatusRepository = null;
        private static IAccountFundTransStatusRepository AccountFundTransStatusRepository
        {
            get
            {
                if (_accountFundTransStatusRepository == null)
                {
                    _accountFundTransStatusRepository = Framework.IoC.Instance.Resolve<IAccountFundTransStatusRepository>();
                }
                return _accountFundTransStatusRepository;
            }
        }

        static AccountFundTransStatus _committed = null;
        static AccountFundTransStatus _pending = null;
        static AccountFundTransStatus _failed = null;
        static readonly object LockObj = new object();
        public static AccountFundTransStatus Committed
        {
            get
            {
                if (checkStatus(_committed))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_committed))
                        {
                            _committed = AccountFundTransStatusRepository.Get((int)AccountFundTransStatusIds.Committed);
                        }
                    }
                }
                return _committed;
            }
        }
        public static AccountFundTransStatus Pending
        {
            get
            {
                if (checkStatus(_pending))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_pending))
                        {
                            _pending = AccountFundTransStatusRepository.Get((int)AccountFundTransStatusIds.Pending);
                        }
                    }
                }
                return _pending;
            }
        }
        public static AccountFundTransStatus Failed
        {
            get
            {
                if (checkStatus(_failed))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_failed))
                        {
                            _failed = AccountFundTransStatusRepository.Get((int)AccountFundTransStatusIds.Failed);
                        }
                    }
                }
                return _failed;
            }
        }
        private static bool checkStatus(AccountFundTransStatus status)
        {
            if (status != null)
            {
                try
                {
                    status.Name.ToString();
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
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            return Equals(obj as AccountFundTransStatus);
        }

        public virtual bool Equals(AccountFundTransStatus obj)
        {
            if (obj == null) return false;

            if (Equals(ID, obj.ID) == false) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int result = 1;

            result = (result * 397) ^ ID.GetHashCode();
            result = (result * 397) ^ Name.ID.GetHashCode();
            return result;
        }
    }
}