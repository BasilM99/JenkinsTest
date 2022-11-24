using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund;
using Noqoush.Framework;
using Noqoush.Framework.ExceptionHandling;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PayPalTester.Utilities.PaymentGateways
{
    class MIGSHelper : IPaymentGatewayHelper
    {
        private IFundTransactionService _fundTransactionService;

        public MIGSHelper(PgwDto migsPgwDTO)
        {
            PaymentGatewayDTO = migsPgwDTO;
            _fundTransactionService = IoC.Instance.Resolve<IFundTransactionService>();
        }

        public PgwDto PaymentGatewayDTO
        {
            get;
            set;
        }

        public void ResolvePendingTransactions()
        {
            try
            {
                ApplicationContext.Instance.Logger.Info("Start Checking MIGS Pending Transactions");

                // get pending transaction for this pgw, do not get new transactions, since they are still in processes. 
                var pendingTrans = _fundTransactionService.GetPendingFundTransactions((int)AccountFundTransTypeIds.CreditCard,
                                        Noqoush.Framework.Utilities.Environment.GetServerTime().AddMinutes(int.Parse(ConfigurationManager.AppSettings["ReverseTimeInMinutes"])));

                // consult pgw to get pending transactions statuses.
                foreach (var trans in pendingTrans.Select(p=>p as PgwFundTransactionResponseDto))
                {
                    if (trans.ID == 14446)
                    {

                   

                    string responseText;
                    var responseColl = PgwResolvePendingTransactions(trans, PaymentGatewayDTO, out responseText);
                    // update the transaction to reflect the correct status.
                    UpdateFundTransaction(responseColl, responseText);
                    }
                }

                ApplicationContext.Instance.Logger.Info("Finish Checking MIGS Pending Transactions");

            }
            catch (Exception x)
            {
                ExceptionPolicy.HandleException(x, ExceptionHandlingPolicies.WinServices);
            }

        }

        /// <summary>
        /// Resolves all pending transactions for a given pgw, this by calling API/WS/Page provided by the pgw
        /// </summary>
        /// <param name="pendingTrans">list of pending transactions</param>
        /// <param name="pgw"> pgw object</param>
        private NameValueCollection PgwResolvePendingTransactions(PgwFundTransactionResponseDto trans, PgwDto pgw, out string responsetxt)
        {

            var webclient = new WebClient();
            // prepare the needed parameters for the requested page
            NameValueCollection inputs = new NameValueCollection();
            inputs.Add("vpc_Version", pgw.Settings["vpc_Version"]);
            inputs.Add("vpc_Command", pgw.Settings["vpc_Track_Command"]);
            inputs.Add("vpc_AccessCode", pgw.Settings["vpc_AccessCode"]);
            inputs.Add("vpc_Merchant", pgw.Settings["vpc_Merchant"]);
            inputs.Add("vpc_MerchTxnRef", trans.ID.ToString());
            inputs.Add("vpc_User", pgw.Settings["vpc_User"]);
            inputs.Add("vpc_Password", pgw.Settings["vpc_Password"]);
            webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            // Consult the pgw to get the actuall status.
            var response = webclient.UploadValues(pgw.Settings["tracker_Url"], "POST", inputs);

            // response as text.
            responsetxt = Encoding.Default.GetString(response);

            // response as name value collection
            NameValueCollection responseColl = GetResponseData(responsetxt);

            // if the pending transaction does not exist in the pgw, 
            //  then set the reference number as it does not return with response.
            if (responseColl["vpc_DRExists"].ToUpper() == "N")
                responseColl["vpc_MerchTxnRef"] = trans.ID.ToString();

            return responseColl;


        }

        /// <summary>
        /// converets response string to name value collection.
        /// </summary>
        /// <param name="responsetxt"></param>
        /// <returns></returns>
        private NameValueCollection GetResponseData(string responsetxt)
        {
            NameValueCollection data = new NameValueCollection();
            if (responsetxt.Length > 0)
            {
                var items = responsetxt.Split('&');
                foreach (var item in items)
                {
                    var part = item.Split('=');
                    data.Add(part[0], part[1]);

                }
            }
            return data;
        }

        /// <summary>
        /// Commit or Cancel Transaction.
        /// </summary>
        /// <param name="transObj"></param>
        private bool UpdateFundTransaction(NameValueCollection transObj, string responseData)
        {
            var _fundTrns = IoC.Instance.Resolve<IFundTransactionService>();

            decimal _amount;
            decimal.TryParse(transObj["vpc_Amount"], out _amount);
            // amount is stored in dollar not cent.
            _amount = _amount / 100;
            var transData = new PgwFundTransactionResponseDto()
            {
                // This is Transaction ID generated by our application
                ID = Int32.Parse(transObj["vpc_MerchTxnRef"]),
                // FundTransStatus as provided by the PGW
                PgwStatus = transObj["vpc_TxnResponseCode"],
                ResponseDate = Noqoush.Framework.Utilities.Environment.GetServerTime(),
                // PGW Transaction ID
                TransactionId = transObj["vpc_TransactionNo"] == "0" ? null : transObj["vpc_TransactionNo"],
                // The complete response as provided by the PGW
                ExtraInfo = transObj.ToString(),
                // amount in response will be used in response validation.
                Amount = _amount,
                // receipt number for reference.
                ReceiptNumber = transObj["vpc_ReceiptNo"]
            };

            int status = -1;
            // the transaction is comitted, only if the transaction status is zero.
            if (Int32.TryParse(transData.PgwStatus, out status) && status == 0)
            {
                transData.FundTransStatus = new AccountFundTransStatusDto() { ID = (int)AccountFundTransStatusIds.Committed };
            }
            else
            {
                transData.FundTransStatus = new AccountFundTransStatusDto() { ID = (int)AccountFundTransStatusIds.Failed };
            }


            try
            {
                bool result = _fundTrns.CloseFundTransaction(transData);
                return result;
            }
            catch (BusinessException x)
            {
                string errorMessage = string.Empty;

                foreach (var error in x.Errors)
                {
                    errorMessage = errorMessage + Environment.NewLine;
                }

                Noqoush.Framework.ApplicationContext.Instance.Logger.Warn(string.Format("Business exception on TransactionID: {0}, Exception Message: {1}", transData.ID, errorMessage));
            }

            return false;

        }
    }
}
