using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Utilities.PaymentGateways
{
    interface IPaymentGatewayHelper
    {
        PgwDto PaymentGatewayDTO { get; set; }

        int InitiateTransaction(int amount, decimal vatamount);

        ActionResult RedirectToGateWay(decimal amount, int transactionId);

        bool ValidateTransaction(NameValueCollection queryStrings);

        PaymentStatus CompletePayment(NameValueCollection queryStrings);

        PaymentStatus ClosePayment(NameValueCollection queryStrings);

    }
}
