using Noqoush.AdFalcon.Web.Controllers.Utilities.PaymentGateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Web.Controllers.Utilities.PaymentGateways
{
    interface IPaymentGatewayHelperFactory
    {
        IPaymentGatewayHelper CreatePaymentHelper(string paymentType);
    }
}
