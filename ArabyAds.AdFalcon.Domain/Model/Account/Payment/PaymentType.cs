using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.Payment;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Account.Payment;

namespace ArabyAds.AdFalcon.Domain.Model.Account.Payment
{
    //[DataContract()]
    //public enum PaymentTypeIds
    //{
    //    [EnumMember]
    //    Cash = 1,
    //    [EnumMember]
    //    WireTransfer = 2,
    //    [EnumMember]
    //    PayPal = 3,
    //    [EnumMember]
    //    Check = 4,
    //    [EnumMember]
    //    FundTransfer = 6,
    //    [EnumMember]
    //    OverBudgetReturn =8,
            
    //}
    public class PaymentType : LookupBase<PaymentType, int>
    {
        #region Values
        /*
        Cash = 1,
        WireTransfer = 2,
        PayPal =3,
        Check=4
         */
        private static IPaymentTypeRepository _paymentTypeRepository = null;
        private static IPaymentTypeRepository PaymentTypeRepository
        {
            get
            {
                if (_paymentTypeRepository == null)
                {
                    _paymentTypeRepository = Framework.IoC.Instance.Resolve<IPaymentTypeRepository>();
                }
                return _paymentTypeRepository;
            }
        }
        static PaymentType _wireTransfer = null;
        static PaymentType _fundTransfer = null;
        static PaymentType _cash = null;
        static PaymentType _payPal = null;
        static PaymentType _check = null;
        static PaymentType _overBudgetReturn = null;
        static readonly object LockObj = new object();


        public static PaymentType OverBudgetReturn
        {
            get
            {
                if (checkStatus(_overBudgetReturn))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_overBudgetReturn))
                        {
                            _overBudgetReturn = PaymentTypeRepository.Get((int)PaymentTypeIds.OverBudgetReturn);
                        }
                    }
                }
                return _overBudgetReturn;
            }
        }



        public static PaymentType FundTransfer
        {
            get
            {
                if (checkStatus(_fundTransfer))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_fundTransfer))
                        {
                            _fundTransfer = PaymentTypeRepository.Get((int)PaymentTypeIds.FundTransfer);
                        }
                    }
                }
                return _fundTransfer;
            }
        }

        public static PaymentType WireTransfer
        {
            get
            {
                if (checkStatus(_wireTransfer))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_wireTransfer))
                        {
                            _wireTransfer = PaymentTypeRepository.Get((int)PaymentTypeIds.WireTransfer);
                        }
                    }
                }
                return _wireTransfer;
            }
        }
        public static PaymentType Cash
        {
            get
            {
                if (checkStatus(_cash))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_cash))
                        {
                            _cash = PaymentTypeRepository.Get((int)PaymentTypeIds.Cash);
                        }
                    }
                }
                return _cash;
            }
        }
        public static PaymentType PayPal
        {
            get
            {
                if (checkStatus(_payPal))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_payPal))
                        {
                            _payPal = PaymentTypeRepository.Get((int)PaymentTypeIds.PayPal);
                        }
                    }
                }
                return _payPal;
            }
        }
        public static PaymentType Check
        {
            get
            {
                if (checkStatus(_check))
                {
                    lock (LockObj)
                    {
                        if (checkStatus(_check))
                        {
                            _check = PaymentTypeRepository.Get((int)PaymentTypeIds.Check);
                        }
                    }
                }
                return _check;
            }
        }

        private static bool checkStatus(PaymentType type)
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
    }
}

