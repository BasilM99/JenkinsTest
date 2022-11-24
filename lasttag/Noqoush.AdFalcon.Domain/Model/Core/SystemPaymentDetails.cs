using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Repositories.Account.Payment;
using Noqoush.Framework.ConfigurationSetting;

namespace Noqoush.AdFalcon.Domain.Model.Core
{
    //[DataContract()]
    //public enum SystemPaymentDetailTypes
    //{
    //    [EnumMember]
    //    CreditCard = 1,
    //    [EnumMember]
    //    Bank = 2,
    //    [EnumMember]
    //    PayPal = 3
    //}
    public class SystemPaymentDetails
    {
        private static IAccountPaymentDetailsRepository _accountPaymentDetailsRepository = null;
        private static IConfigurationManager _configurationManager = null;
        private static IAccountPaymentDetailsRepository AccountPaymentDetailsRepository
        {
            get
            {
                return _accountPaymentDetailsRepository ??
                       (_accountPaymentDetailsRepository = Framework.IoC.Instance.Resolve<IAccountPaymentDetailsRepository>());
            }
        }
        public static IConfigurationManager ConfigurationManager
        {
            get
            {
                return _configurationManager ??
                       (_configurationManager = Framework.IoC.Instance.Resolve<IConfigurationManager>());
            }
        }

        static AccountPaymentDetails _creditCard = null;
        static AccountPaymentDetails _bank = null;
        static AccountPaymentDetails _payPal = null;

        private static int GetDetailId(SystemPaymentDetailTypes systemPaymentDetailTypes)
        {
            int id = 0;
            switch (systemPaymentDetailTypes)
            {
                case SystemPaymentDetailTypes.CreditCard:
                    {
                        if (!int.TryParse(ConfigurationManager.GetConfigurationSetting(null, null, "CreditCardSystemPaymentDetailId"), out id))
                            throw new Exception("Credit Card System Payment Detail not found");
                        break;
                    }
                case SystemPaymentDetailTypes.Bank:
                    {
                        if (!int.TryParse(ConfigurationManager.GetConfigurationSetting(null, null, "BankSystemPaymentDetailId"), out id))
                            throw new Exception("Bank System Payment Detail not found");
                        break;
                    }
                case SystemPaymentDetailTypes.PayPal:
                    {
                        if (!int.TryParse(ConfigurationManager.GetConfigurationSetting(null, null, "PayPalSystemPaymentDetailId"), out id))
                            throw new Exception("PayPal System Payment Detail not found");
                        break;
                    }
            }
            return id;
        }
        static readonly object CreditCardLockObj = new object();
        static readonly object PayPalLockObj = new object();

        private static bool checkStatus(AccountPaymentDetails status)
        {
            if (status != null)
            {
                try
                {
                    status.ID.ToString();
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

        public static BankAccountPaymentDetails CreditCard
        {
            get
            {
                return AccountPaymentDetailsRepository.Get(GetDetailId(SystemPaymentDetailTypes.CreditCard)) as BankAccountPaymentDetails;
            }
        }

        public static PayPalAccountPaymentDetails PayPal
        {
            get
            {
                return AccountPaymentDetailsRepository.Get(GetDetailId(SystemPaymentDetailTypes.PayPal)) as PayPalAccountPaymentDetails;
       
            }
        }
    }
}
