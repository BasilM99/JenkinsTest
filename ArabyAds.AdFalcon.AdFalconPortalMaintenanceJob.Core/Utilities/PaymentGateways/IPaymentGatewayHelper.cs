using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Maintenance.PGWTracker.Utilities.PaymentGateways
{
    interface IPaymentGatewayHelper
    {
        PgwDto PaymentGatewayDTO { get; set; }

        void ResolvePendingTransactions();
    }
}
