using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund;
using ArabyAds.Framework;
using ArabyAds.Framework.ExceptionHandling;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PayPalTester.Utilities.PaymentGateways
{
    class PayPalTransaction
    {
        public string PayPalTransactionID { get; set; }
        public int? AdFalconTransactionID { get; set; }
        public string Email { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
    }

    class PayPalHelper : IPaymentGatewayHelper
    {
        private IFundTransactionService _FundTransactionService;
        private int _ReverseInterval;

        public PgwDto PaymentGatewayDTO
        {
            get;
            set;
        }

        public PayPalHelper(PgwDto paypalPgwDTO)
        {
            PaymentGatewayDTO = paypalPgwDTO;
            _FundTransactionService = IoC.Instance.Resolve<IFundTransactionService>();
            _ReverseInterval = int.Parse(ConfigurationManager.AppSettings["ReverseTimeInMinutes"]);
        }

        public void ResolvePendingTransactions()
        {
            try
            {
                ApplicationContext.Instance.Logger.Info("Start Checking Paypal Pending Transactions");

                // Get pending Paypal transactions from our system before specific date.
                var pendingTrans = _FundTransactionService.GetPendingFundTransactions((int)AccountFundTransTypeIds.PayPal,
                                            ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMinutes(_ReverseInterval));

                List<PayPalFundTransactionResponseDto> paypalPendingTransactions = pendingTrans.Select(p => p as PayPalFundTransactionResponseDto).ToList();

                if (paypalPendingTransactions != null && paypalPendingTransactions.Count != 0)
                {
                    DateTime oldestPendingTransactionDate = pendingTrans.Last().TransactionDate;
                    List<PayPalTransaction> paypalTransactions = GetTransactionsFromPayPal(oldestPendingTransactionDate);

                    HandlePendingTransactions(paypalPendingTransactions, paypalTransactions);
                }

                ApplicationContext.Instance.Logger.Info("Finish Checking Paypal Pending Transactions");
            }
            catch (Exception x)
            {
                ExceptionPolicy.HandleException(x, ExceptionHandlingPolicies.WinServices);
            }
        }

        private void HandlePendingTransactions(List<PayPalFundTransactionResponseDto> pendingTransactions, List<PayPalTransaction> paypalTransactions)
        {
            foreach (var transaction in pendingTransactions)
            {
                PayPalTransaction paypalMatchedTransaction = paypalTransactions.Where(p => p.AdFalconTransactionID == transaction.ID).SingleOrDefault();

                try
                {
                    // If there is match in Payapl, check the paypal status for this transaction and take action accordingly
                    if (paypalMatchedTransaction != null)
                    {
                        switch (paypalMatchedTransaction.Status.ToLower())
                        {
                            case "completed":
                                CloseTransaction(transaction.ID, paypalMatchedTransaction, AccountFundTransStatusIds.Committed);
                                break;
                            case "denied":
                                CloseTransaction(transaction.ID, null, AccountFundTransStatusIds.Failed);
                                break;
                            default:
                                // for other statuses, like pending or processing, keep the transaction as is.
                                break;
                        }
                    }
                    else
                    {
                        // If there is no match in Payapl, close the transaction as failed.
                        CloseTransaction(transaction.ID, null, AccountFundTransStatusIds.Failed);
                    }
                }
                catch (BusinessException x)
                {
                    string errorMessage = string.Empty;

                    foreach (var item in x.Errors)
                    {
                        errorMessage += item.Message + Environment.NewLine;
                    }

                    ArabyAds.Framework.ApplicationContext.Instance.Logger.Warn(string.Format("Business exception on TransactionID: {0}, Exception Message: {1}", transaction.ID, errorMessage));
                }
            }
        }

        /// <summary>
        /// Close the transaction by changing the status from pending to failed or Committed
        /// </summary>
        /// <param name="transid"></param>
        /// <param name="transaction"></param>
        /// <param name="AccountFundTansStatus"></param>
        private void CloseTransaction(int transid, PayPalTransaction transaction, AccountFundTransStatusIds AccountFundTansStatus)
        {
            var paypalFundResponseDto = new PayPalFundTransactionResponseDto()
            {
                ID = transid,
                TransactionId = transaction != null ? transaction.PayPalTransactionID : null,
                FundTransStatus = new AccountFundTransStatusDto() { ID = (int)AccountFundTansStatus },
                EmailAddress = transaction != null && transaction.Email != null ? transaction.Email : null,
                Amount = transaction != null && transaction.Amount != null ? decimal.Parse(transaction.Amount.ToString()) : 0
            };

            _FundTransactionService.CloseFundTransaction(paypalFundResponseDto);
        }

        /// <summary>
        /// Get Paypal transactions from Paypal using "TransactionSearch" method in the API
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        private List<PayPalTransaction> GetTransactionsFromPayPal(DateTime startDate)
        {
            NameValueCollection requestTokenValues = GetRequestNameValueCollection("search");
            

            requestTokenValues.Add("ENDDATE", ArabyAds.Framework.Utilities.Environment.GetServerTime().AddMinutes(_ReverseInterval).ToString("o"));
            requestTokenValues.Add("STARTDATE", startDate.ToUniversalTime().ToString("o"));

            NameValueCollection paypalTransactionsNameValues = CallIntegrationUrl(requestTokenValues);

            return ConvertSearchResultToPayPalTransactions(paypalTransactionsNameValues);
        }

        /// <summary>
        /// Convert "TransactionSearch" method result from NameValueCollection to List of PayPalTransaction
        /// </summary>
        /// <param name="paypalTransactionsNameValues"></param>
        /// <returns></returns>
        private List<PayPalTransaction> ConvertSearchResultToPayPalTransactions(NameValueCollection paypalTransactionsNameValues)
        {
            List<PayPalTransaction> paypalTransactions = new List<PayPalTransaction>();

            // If the request result to paypal is success and nothing wrong has happened
            if (paypalTransactionsNameValues["ack"].ToLower() == "success")
            {
                // Count how many transactions have been returned from the API.
                int counter = paypalTransactionsNameValues.AllKeys.Where(p => p.StartsWith("L_TRANSACTIONID")).Count();

                for (int i = 0; i < counter; i++)
                {
                    PayPalTransaction transaction = ConvertNameValueToPayPalTransaction(i, paypalTransactionsNameValues);

                    // Add only the transactions that we can match with our system
                    if (transaction.AdFalconTransactionID.HasValue)
                    {
                        paypalTransactions.Add(transaction);
                    }
                }
            }

            
            return paypalTransactions;
        }

        private PayPalTransaction ConvertNameValueToPayPalTransaction(int itemIndex, NameValueCollection paypalTransactionsNameValues)
        {
            PayPalTransaction transaction = new PayPalTransaction()
            {
                PayPalTransactionID = paypalTransactionsNameValues["L_TRANSACTIONID" + itemIndex],
                Status = paypalTransactionsNameValues["L_STATUS" + itemIndex] == null ? string.Empty : paypalTransactionsNameValues["L_STATUS" + itemIndex],
                Amount = paypalTransactionsNameValues["L_AMT" + itemIndex],
                Email = paypalTransactionsNameValues["L_EMAIL" + itemIndex]
            };

            // Get Adfalcon transactionid for this transaction using "GetTransactionDetails" method in the API
            transaction.AdFalconTransactionID = GetCustomValueForTransaction(transaction.PayPalTransactionID);

            return transaction;
        }

        /// <summary>
        /// Get our transactionid for this transaction from paypal base on their transactionid using GetTransactionDetails method in the API
        /// </summary>
        /// <param name="transactionID">Paypal transactionid</param>
        /// <returns></returns>
        private int? GetCustomValueForTransaction(string transactionID)
        {
            NameValueCollection requestTokenValues = GetRequestNameValueCollection("gettransaction");
            requestTokenValues.Add("transactionid", transactionID);

            NameValueCollection transactionDetailsNameValue = CallIntegrationUrl(requestTokenValues);

            if (transactionDetailsNameValue["ack"].ToLower() == "success")
            {
                string customValue = transactionDetailsNameValue["CUSTOM"];
                if (customValue != null)
                {
                    int adfalconTransactionId;
                    bool parseResult = int.TryParse(customValue, out adfalconTransactionId);

                    if (parseResult)
                    {
                        return adfalconTransactionId;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Prepare the request parameters based on the method name from setting of the PaymentGateway
        /// </summary>
        /// <param name="keyStartName"></param>
        /// <returns></returns>
        private NameValueCollection GetRequestNameValueCollection(string keyStartName)
        {
            NameValueCollection requestTokenValues = new NameValueCollection();

            foreach (var keyValue in PaymentGatewayDTO.Settings.Where(p => p.Key.StartsWith(keyStartName) || p.Key.StartsWith("general")))
            {
                if (keyValue.Key.StartsWith(keyStartName + "_"))
                {
                    // Remove the method name from the start of the key
                    requestTokenValues.Add(keyValue.Key.Substring(keyStartName.Length + 1), keyValue.Value.ToString());
                }
                else
                {
                    // Remove "general_" from the start of the key
                    requestTokenValues.Add(keyValue.Key.Substring(8), keyValue.Value.ToString());
                }
            }

            return requestTokenValues;
        }

        /// <summary>
        /// Perform the actual request to API
        /// </summary>
        /// <param name="values">request parameters</param>
        /// <returns></returns>
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

    }
}
