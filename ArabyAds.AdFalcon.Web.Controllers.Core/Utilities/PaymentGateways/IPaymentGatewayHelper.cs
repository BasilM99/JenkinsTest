using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace ArabyAds.AdFalcon.Web.Controllers.Utilities.PaymentGateways
{
    interface IPaymentGatewayHelper
    {
        PgwDto PaymentGatewayDTO { get; set; }

        int InitiateTransaction(int amount, decimal vatamount);
        string RedirectToGateWayString(decimal amount, int transactionId);
        ActionResult RedirectToGateWay(decimal amount, int transactionId);

        bool ValidateTransaction(NameValueCollection queryStrings);

        PaymentStatus CompletePayment(NameValueCollection queryStrings, ActionContext contexts);

        PaymentStatus ClosePayment(NameValueCollection queryStrings);

    }
}
