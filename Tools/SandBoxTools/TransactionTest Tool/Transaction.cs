using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TransactionTestTool
{
    public class Transaction
    {
        #region transaction
        /// <summary>
        /// Resolves all pending transactions for a given pgw, this by calling API/WS/Page provided by the pgw
        /// </summary>
        /// <param name="pendingTrans">list of pending transactions</param>
        /// <param name="pgw"> pgw object</param>
        static public NameValueCollection PgwResolvePendingTransactions(Dictionary<string, string> Settings, out string responsetxt, string ID)
        {

            NameValueCollection responseColl = new NameValueCollection();
            responsetxt = "";
            try
            {
                var webclient = new WebClient();
                // prepare the needed parameters for the requested page
                NameValueCollection inputs = new NameValueCollection();
                inputs.Add("vpc_Version", Settings["vpc_Version"]);
                inputs.Add("vpc_Command", Settings["vpc_Track_Command"]);
                inputs.Add("vpc_AccessCode", Settings["vpc_AccessCode"]);
                inputs.Add("vpc_Merchant", Settings["vpc_Merchant"]);
                inputs.Add("vpc_MerchTxnRef", ID.ToString());
                inputs.Add("vpc_User", Settings["vpc_User"]);
                inputs.Add("vpc_Password", Settings["vpc_Password"]);
                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                // Consult the pgw to get the actuall status.
                var response = webclient.UploadValues(Settings["tracker_Url"], "POST", inputs);

                // response as text.
                responsetxt = Encoding.Default.GetString(response);

                // response as name value collection
                responseColl = GetResponseData(responsetxt);

                // if the pending transaction does not exist in the pgw, 
                //  then set the reference number as it does not return with response.
                if (responseColl["vpc_DRExists"].ToUpper() == "N")
                    responseColl["vpc_MerchTxnRef"] = ID.ToString();

            }
            catch (Exception e)
            {

                throw e;
            }

            return responseColl;
        }

        /// <summary>
        /// converets response string to name value collection.
        /// </summary>
        /// <param name="responsetxt"></param>
        /// <returns></returns>
        static private NameValueCollection GetResponseData(string responsetxt)
        {
            NameValueCollection data = new NameValueCollection();
            try
            {
                if (responsetxt.Length > 0)
                {
                    var items = responsetxt.Split('&');
                    foreach (var item in items)
                    {
                        var part = item.Split('=');
                        data.Add(part[0], part[1]);

                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return data;
        }


        /// <summary>
        /// Commit or Cancel Transaction.
        /// </summary>
        /// <param name="transObj"></param>
        static public void PrintFundTransaction(NameValueCollection transObj, string responseData)
        {
            try
            {
                decimal _amount;
                decimal.TryParse(transObj["vpc_Amount"], out _amount);
                // amount is stored in dollar not cent.
                _amount = _amount / 100;


                Console.WriteLine("\t vpc_TxnResponseCode : " + transObj["vpc_TxnResponseCode"]);
                Console.WriteLine("\t vpc_MerchTxnRef : " + transObj["vpc_MerchTxnRef"]);

                Console.WriteLine("\t vpc_TransactionNo : " + transObj["vpc_TransactionNo"]);

                Console.WriteLine("\t _amount : " + _amount);

                Console.WriteLine("\t vpc_ReceiptNo : " + transObj["vpc_ReceiptNo"]);
                Console.WriteLine("");
                Console.WriteLine("---------------------------------------------------------------------");
                Console.WriteLine("---------------------------------------------------------------------");
                Console.WriteLine("");
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        #endregion
    }
}
