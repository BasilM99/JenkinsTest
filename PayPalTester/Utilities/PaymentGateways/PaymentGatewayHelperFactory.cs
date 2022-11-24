using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund;
using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalTester.Utilities.PaymentGateways
{
    class PaymentGatewayHelperFactory : IPaymentGatewayHelperFactory
    {
         private IFundTransactionService _fundTransactionService;

        public PaymentGatewayHelperFactory()
        {
            _fundTransactionService = IoC.Instance.Resolve<IFundTransactionService>();
        }

        public IPaymentGatewayHelper CreatePaymentHelper(string paymentType)
        {
            PgwDto paymentGatewayDTO = _fundTransactionService.GetPgwInfoByCode(paymentType);

            switch (paymentType.ToLower())
            {
                case "migs":
                    return new MIGSHelper(paymentGatewayDTO);
                case "paypal":
                    return new PayPalHelper(paymentGatewayDTO);
                    break;
                default:
                    return new MIGSHelper(paymentGatewayDTO);
            }
        }
    }
}
