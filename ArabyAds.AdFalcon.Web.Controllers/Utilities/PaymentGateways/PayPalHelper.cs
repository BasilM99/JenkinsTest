using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Exceptions.Fund;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund;
using Noqoush.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Utilities.PaymentGateways
{
    class PayPalHelper : IPaymentGatewayHelper
    {
        private IFundTransactionService _fundTransactionService;

        public PayPalHelper(PgwDto payPalPgwDTO)
        {
            PaymentGatewayDTO = payPalPgwDTO;
            _fundTransactionService = IoC.Instance.Resolve<IFundTransactionService>();
            PaymentGatewayDTO.Settings = FixKeysUpperCase(PaymentGatewayDTO.Settings);
        }

        public PgwDto PaymentGatewayDTO
        {
            get;
            set;
        }

        public ActionResult RedirectToGateWay(decimal amount, int transactionId)
        {
            NameValueCollection requestTokenValues = GetRequestNameValueCollection("set");

            requestTokenValues["PAYMENTREQUEST_0_CUSTOM"] = transactionId.ToString();
            requestTokenValues["returnurl"] = string.Format(PaymentGatewayDTO.ReturnPageUrl, Config.CurrentLanguage, transactionId);
            requestTokenValues["PAYMENTREQUEST_0_AMT"] = amount.ToString(CultureInfo.InvariantCulture);
            requestTokenValues["cancelurl"] = string.Format(requestTokenValues["cancelurl"], Config.CurrentLanguage, transactionId);
            var requestTokenResult = CallIntegrationUrl(requestTokenValues);

            string ack = requestTokenResult["ACK"].ToLower();

            if (ack == "success" || ack == "successwithwarning")
            {
                return new RedirectResult(String.Format(PaymentGatewayDTO.Settings["set_paymenturl"], requestTokenResult["TOKEN"]));
            }
            else
            {
                throw new Exception(requestTokenResult["L_LONGMESSAGE0"]);
            }
        }

        public int InitiateTransaction(int amount, decimal vatamount)
        {
            var transData = new FundTransactionDto()
               {
                   // current user account id
                   AccountId = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.HasValue
                                   ? OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value : 1,
                   // input from user
                   Amount = Convert.ToDecimal(amount),
                   VATAmount= Convert.ToDecimal(vatamount),
                // current user id
                CreatedById = OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalUserId.HasValue
                                   ? OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalUserId.Value : 0,
                   // PGW ID, this provided by the UI, for the time being we have one PGW (MiGS)
                   AccountFundPgwId = PaymentGatewayDTO.ID,
                   // this is setting somewhere, for the time being it is always "AdFalcon".
                   Payee = "AdFalcon",
                   FundTransStatus = new Services.Interfaces.DTOs.Account.Fund.AccountFundTransStatusDto { ID = (int)AccountFundTransStatusIds.Pending },
                   // provided by UI, fund transaction type (PGW=1, Bank Transfer, Check, Cash ...etc)
                   FundTransType = new Services.Interfaces.DTOs.Account.Fund.AccountFundTransTypeDto { ID = (int)AccountFundTransTypeIds.PayPal },
                   FundType = new Services.Interfaces.DTOs.Account.Fund.AccountFundTypeDto { ID = (int)AccountFundTypeIds.Fund },
                   // transaction date, usually current date.
                   TransactionDate = Framework.Utilities.Environment.GetServerTime(),
               };

            // return the generated transaction ID to be used as refernce for this transaction.
            return _fundTransactionService.InitiateFundTransaction(transData);
        }

        public bool ValidateTransaction(NameValueCollection queryStrings)
        {
            return true;
        }

        public PaymentStatus CompletePayment(NameValueCollection queryStrings)
        {
            FundTransactionDto fundDto = _fundTransactionService.GetFundTransactionById(int.Parse(queryStrings["transid"]));

            if (fundDto.FundTransStatus.ID == (int)AccountFundTransStatusIds.Pending)
            {
                var doExpressCheckoutResult = new NameValueCollection();
                UrlHelper urlHelper = new UrlHelper(new HttpRequestWrapper(HttpContext.Current.Request).RequestContext);
                NameValueCollection getExpressCheckoutResult = GetExpressCheckout(queryStrings);

                string buyerEmail = getExpressCheckoutResult["Email"];

                try
                {
                  doExpressCheckoutResult = DoExpressCheckout(getExpressCheckoutResult);
                }
                catch (PaymentGatewayException)
                {
                    CloseTransaction(queryStrings["transid"], null, AccountFundTransStatusIds.Failed);
                    return new PaymentStatus() { IsCompleted = false, Message = string.Format(ResourcesUtilities.GetResource("UnsuccessfulTransaction", "PGW"), queryStrings["transid"]) };
                }

                var closeParametersCollection = new NameValueCollection(doExpressCheckoutResult);
                closeParametersCollection.Add("Email", buyerEmail);

                CloseTransaction(queryStrings["transid"], closeParametersCollection, AccountFundTransStatusIds.Committed);

                return new PaymentStatus() { IsCompleted = true, TransationID = int.Parse(queryStrings["transid"]), Amount = decimal.Parse(doExpressCheckoutResult["PAYMENTINFO_0_AMT"]), Message = string.Format(ResourcesUtilities.GetResource("SuccessfullyMsg", "PGW"), decimal.Parse(doExpressCheckoutResult["PAYMENTINFO_0_AMT"]), urlHelper.Action("AccountHistory")) };
            }

            return new PaymentStatus() { IsCompleted = false, Message = string.Format(ResourcesUtilities.GetResource("TransactionAlreadyClosedBR", "Global"), fundDto.ID) };
        }

        public PaymentStatus ClosePayment(NameValueCollection queryStrings)
        {
            return CloseTransaction(queryStrings["transid"], null, AccountFundTransStatusIds.Failed);
        }

        private PaymentStatus CloseTransaction(string transid, NameValueCollection queryStrings, AccountFundTransStatusIds AccountFundTansStatus)
        {
            PaymentStatus status = null;

            try
            {
                var paypalFundResponseDto = new PayPalFundTransactionResponseDto()
                {
                    ID = int.Parse(transid),
                    TransactionId = queryStrings != null ? queryStrings["PAYMENTINFO_0_TRANSACTIONID"] : null,
                    FundTransStatus = new AccountFundTransStatusDto() { ID = (int)AccountFundTansStatus },
                    EmailAddress = queryStrings != null ? queryStrings["Email"] : null,
                    Amount = queryStrings != null ? decimal.Parse(queryStrings["PAYMENTINFO_0_AMT"].ToString()) : 0
                };

                _fundTransactionService.CloseFundTransaction(paypalFundResponseDto);


            }
            catch (TransactionAlreadyClosedExpectation x)
            {
                status = new PaymentStatus() { IsCompleted = false, Message = x.Errors.First().Message };
            }

            return status;
        }


        #region Private Members

        private Dictionary<string, string> FixKeysUpperCase(Dictionary<string, string> requestTokenValues)
        {
            Dictionary<string, string> fixedNameValueCollection = new Dictionary<string, string>();

            foreach (var kvp in PaymentGatewayDTO.Settings)
            {
                fixedNameValueCollection.Add(kvp.Key.ToLower(), kvp.Value);
            }

            return fixedNameValueCollection;
        }

        private NameValueCollection GetRequestNameValueCollection(string keyStartName)
        {
            NameValueCollection requestTokenValues = new NameValueCollection();

            foreach (var keyValue in PaymentGatewayDTO.Settings.Where(p => p.Key.StartsWith(keyStartName) || p.Key.StartsWith("general")))
            {
                if (keyValue.Key.StartsWith(keyStartName + "_"))
                {
                    requestTokenValues.Add(keyValue.Key.Substring(keyStartName.Length + 1), keyValue.Value.ToString());
                }
                else
                {
                    requestTokenValues.Add(keyValue.Key.Substring(8), keyValue.Value.ToString());
                }
            }

            return requestTokenValues;
        }

        private NameValueCollection CallIntegrationUrl(NameValueCollection values)
        {
            string data = String.Join("&", values.Cast<string>()
              .Select(key => String.Format("{0}={1}", key, HttpUtility.UrlEncode(values[key]))));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
               PaymentGatewayDTO.IntegrationPageUrl);

            request.Method = "POST";
            request.ContentLength = data.Length;

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(data);
            }

            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                return HttpUtility.ParseQueryString(reader.ReadToEnd());
            }
        }

        private NameValueCollection GetExpressCheckout(NameValueCollection queryStrings)
        {
            NameValueCollection requestTokenValues = GetRequestNameValueCollection("get");

            requestTokenValues.Add("token", queryStrings["token"]);

            var paymentDetailsResult = CallIntegrationUrl(requestTokenValues);

            string ack = paymentDetailsResult["ACK"].ToLower();

            if (ack == "success" || ack == "successwithwarning")
            {
                return paymentDetailsResult;
            }
            else
            {
                throw new PaymentGatewayException(paymentDetailsResult["L_LONGMESSAGE0"]);
            }
        }

        private NameValueCollection DoExpressCheckout(NameValueCollection getExpressCheckoutResult)
        {
            NameValueCollection requestTokenValues = GetRequestNameValueCollection("do");

            requestTokenValues.Add("token", getExpressCheckoutResult["token"]);
            requestTokenValues.Add("PAYERID", getExpressCheckoutResult["PAYERID"]);
            requestTokenValues["PAYMENTREQUEST_0_AMT"] = getExpressCheckoutResult["PAYMENTREQUEST_0_AMT"];

            var doExpressCheckoutResult = CallIntegrationUrl(requestTokenValues);

            string ack = doExpressCheckoutResult["ACK"].ToLower();

            if (ack == "success" || ack == "successwithwarning")
            {
                return doExpressCheckoutResult;
            }
            else
            {
                throw new PaymentGatewayException(doExpressCheckoutResult["L_LONGMESSAGE0"]);
            }
        }

     
        #endregion

    }
}
