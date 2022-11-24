using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Web.Controllers.Utilities.PaymentGateways
{
    public class PaymentGatewayException : ApplicationException
    {
        public PaymentGatewayException()
            :base()
        {

        }

        public PaymentGatewayException(string message)
            : base(message)
        {

        }
    }
}
