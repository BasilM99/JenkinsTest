using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund;
using ArabyAds.Framework;
using PayPalTester.Utilities.PaymentGateways;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PayPalTester
{
    class Program
    {
        private static IFundTransactionService _fundTransactionService;

        private static IPaymentGatewayHelperFactory paymentHelperFactory = new PaymentGatewayHelperFactory();

        static void Main(string[] args)
        {
            
            _fundTransactionService = IoC.Instance.Resolve<IFundTransactionService>();

            //ICollection<PgwDto> paymentGateWays = _fundTransactionService.GetRegistredPGWs();

            // foreach (var gateway in paymentGateWays)
            // {
            //     IPaymentGatewayHelper helper = paymentHelperFactory.CreatePaymentHelper(gateway.Code);
            //     helper.ResolvePendingTransactions();
            // }

            IPaymentGatewayHelper helper = paymentHelperFactory.CreatePaymentHelper("migs");
            helper.ResolvePendingTransactions();

        }

   


    }
}
