using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Repositories.Account.Payment;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.System;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Services.NSystem
{
    public class SystemAccountService : ISystemAccountService
    {

        private readonly IAccountPaymentDetailsRepository _accountPaymentDetailsRepository = null;
        public SystemAccountService(IAccountPaymentDetailsRepository accountPaymentDetailsRepository)
        {
            this._accountPaymentDetailsRepository = accountPaymentDetailsRepository;
        }

        public IList<PaymentDetailDto> GetSystemPaymentDetails(ValueMessageWrapper<PayemntAccountType> accountType)
        {
            var list = _accountPaymentDetailsRepository.Query(x => x.IsSystem && x.IsActive && x.AccountType == accountType.Value);
            return list.Select(x => MapperHelper.Map<PaymentDetailDto>(x)).ToList();
        }

        public ValueMessageWrapper<bool> Decypt()
        {
            string EncryptionKey = File.ReadAllText("C:\\EncryptionKey.txt");
            string EncryptionValue = File.ReadAllText("C:\\DecryptionValue.txt");
            string Result = ArabyAds.Framework.Utilities.Cryptography.Decrypt(EncryptionValue, true, EncryptionKey);
            File.WriteAllText("C:\\ResutDecryptionValue.txt", Result);

            return new ValueMessageWrapper<bool>() { Value = true };
        }
        public ValueMessageWrapper<bool> Encrypt()
        {
            string EncryptionKey = File.ReadAllText("C:\\EncryptionKey.txt");
            string EncryptionValue = File.ReadAllText("C:\\EncryptionValue.txt");
            string Result = ArabyAds.Framework.Utilities.Cryptography.Encrypt(EncryptionValue, true, EncryptionKey);
            File.WriteAllText("C:\\ResutEncryptionValue.txt", Result);

            return new ValueMessageWrapper<bool>() { Value = true };

        }

        public ValueMessageWrapper<bool> Unprotect()
        {
            string EncryptionKey = File.ReadAllText("C:\\EncryptionKey.txt");
            string EncryptionValue = File.ReadAllText("C:\\DecryptionValue.txt");
            string Result = ArabyAds.Framework.Utilities.Cryptography.Unprotect(EncryptionValue);
            File.WriteAllText("C:\\ResutDecryptionValue.txt", Result);

            return new ValueMessageWrapper<bool>() { Value = true };
        }
        public ValueMessageWrapper<bool> protect()
        {
            string EncryptionKey = File.ReadAllText("C:\\EncryptionKey.txt");
            string EncryptionValue = File.ReadAllText("C:\\EncryptionValue.txt");
            string Result = ArabyAds.Framework.Utilities.Cryptography.protect(EncryptionValue);
            File.WriteAllText("C:\\ResutEncryptionValue.txt", Result);

            return new ValueMessageWrapper<bool>() { Value = true };

        }
    }
    }
