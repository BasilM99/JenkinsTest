using System;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using System.Collections;
using System.Collections.Specialized;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund;
using ArabyAds.Framework.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

namespace ArabyAds.AdFalcon.Web.Controllers.Utilities.PaymentGateways
{
    public class MIGSHelper : IPaymentGatewayHelper
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
        //Old imp
        public ActionResult RedirectToGateWayOld(decimal amount, int transactionId)
        {
            string SECURE_SECRET = PaymentGatewayDTO.Settings["SECURE_SECRET"];

            // We use our own overloaded IComparer interface to ensure we do 
            // an Ordinal sort of the data.
            var transactionData = new SortedList(new VPCStringComparer());

            // create start of Query data
            string queryString = PaymentGatewayDTO.IntegrationPageUrl;
            transactionData.Add("vpc_Version", PaymentGatewayDTO.Settings["vpc_Version"]);
            transactionData.Add("vpc_Command", PaymentGatewayDTO.Settings["vpc_Pay_Command"]);
            transactionData.Add("vpc_AccessCode", PaymentGatewayDTO.Settings["vpc_AccessCode"]);
            transactionData.Add("vpc_Merchant", PaymentGatewayDTO.Settings["vpc_Merchant"]);
            transactionData.Add("vpc_Amount", amount);
            transactionData.Add("vpc_ReturnURL", string.Format(PaymentGatewayDTO.ReturnPageUrl, Config.CurrentLanguage));

            transactionData.Add("vpc_Locale", Config.CurrentLanguage);
            string rawHashData = SECURE_SECRET;
            string seperator = "?";
            StringBuilder sb = new StringBuilder();
            // as pgw expect amount in cent and the UI get it in dollar.
            transactionData["vpc_Amount"] = double.Parse(transactionData["vpc_Amount"].ToString()) * 100;
            // the returned ID will be the shared reference between adFalcon and the PGW provider.
            transactionData["vpc_MerchTxnRef"] = transactionId.ToString();
            // same as vpc_MerchTxnRef.
            transactionData["vpc_OrderInfo"] = transactionData["vpc_MerchTxnRef"];

            // Loop through all the data in the SortedList transaction data
            foreach (DictionaryEntry item in transactionData)
            {
                // build the query string, URL Encoding all keys and their values
                queryString += seperator + HttpUtility.UrlEncode(item.Key.ToString()) + "=" +
                               HttpUtility.UrlEncode(item.Value.ToString());
                seperator = "&";
                if (!string.IsNullOrEmpty(item.Value.ToString()))
                {
                    sb.Append(item.Key + "=" + item.Value.ToString() + "&");
                }
                // Collect the data required for the MD5 sugnature if required
                if (SECURE_SECRET.Length > 0)
                {
                    rawHashData += item.Value.ToString();
                }
            }
            sb.Remove(sb.Length - 1, 1);
            // Create the MD5 signature
            string signature = "";
            if (SECURE_SECRET.Length > 0)
            {
                // create the signature and add it to the query string
                signature = Hashing.CreateHMACSHA256Hash(SECURE_SECRET, sb.ToString()).ToUpper();
                queryString += seperator + "vpc_SecureHash=" + signature + "&vpc_SecureHashType=SHA256";
            }

            return new RedirectResult(queryString);
        }
        public ActionResult RedirectToGateWay(decimal amount, int transactionId)
        {

          string accessToken=  GetAccessToken(PaymentGatewayDTO.apiref, PaymentGatewayDTO.IntegrationPageUrl, PaymentGatewayDTO.Realm);


      

            var URL= CreateOrder(amount, transactionId, PaymentGatewayDTO.apiref, PaymentGatewayDTO.IntegrationPageUrl, accessToken, PaymentGatewayDTO.OutletId, PaymentGatewayDTO.Realm);
           
            if(!string.IsNullOrEmpty(URL))
            return new RedirectResult(URL);
            else
                return null;
        }


        public string RedirectToGateWayString(decimal amount, int transactionId)
        {

            string accessToken = GetAccessToken(PaymentGatewayDTO.apiref, PaymentGatewayDTO.IntegrationPageUrl, PaymentGatewayDTO.Realm);




            var URL = CreateOrder(amount, transactionId, PaymentGatewayDTO.apiref, PaymentGatewayDTO.IntegrationPageUrl, accessToken, PaymentGatewayDTO.OutletId, PaymentGatewayDTO.Realm);

            if (!string.IsNullOrEmpty(URL))
                return  URL;
            else
                return null;
        }

        //Old imp
        public bool ValidateTransactionOld(NameValueCollection queryStrings)
        {
            string rawHashData = "";
            string SECURE_SECRET = PaymentGatewayDTO.Settings["SECURE_SECRET"];
            bool hashValidated = false;
            StringBuilder sb = new StringBuilder();
            // If we have a SECURE_SECRET then validate the incoming data using the MD5 hash
            //included in the incoming data
            if (queryStrings["vpc_SecureHash"] != null && queryStrings["vpc_SecureHash"].Length > 0)
            {
                // loop through all the data in the Page.Request.Form
                foreach (string item in queryStrings)
                {
                    // Collect the data required for the MD5 sugnature if required
                    if (SECURE_SECRET.Length > 0 && !item.Equals("vpc_SecureHash") && !item.Equals("vpc_SecureHashType"))
                    {
                        rawHashData += queryStrings[item];

                        if (!string.IsNullOrEmpty(queryStrings[item].ToString()))
                        {
                            sb.Append(item + "=" + queryStrings[item].ToString() + "&");
                        }
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                string signature = "";
                if (SECURE_SECRET.Length > 0)
                {
                    // create the signature and add it to the query string
                    signature = Hashing.CreateHMACSHA256Hash(SECURE_SECRET, sb.ToString()).ToUpper();
                    hashValidated = queryStrings["vpc_SecureHash"].Equals(signature);
                }
                else
                {

                    hashValidated = true;
                }
            }
            return hashValidated;
        }
        public bool ValidateTransaction(NameValueCollection queryStrings)
        {


         

       
            bool hashValidated = false;
            StringBuilder sb = new StringBuilder();
            // If we have a SECURE_SECRET then validate the incoming data using the MD5 hash
            //included in the incoming data
            if (queryStrings["ref"] != null && queryStrings["ref"].Length > 0)
            {
                hashValidated = true;
            }
            return hashValidated;
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
                VATAmount = Convert.ToDecimal(vatamount),
                // current user id
                CreatedById = OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalUserId.HasValue
                                ? OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalUserId.Value : 0,

                AccountFundPgwId = PaymentGatewayDTO.ID,
                // this is setting somewhere, for the time being it is always "AdFalcon".
                Payee = "AdFalcon",
                // this should be a common Enum, and the below status is Initiated = 1.
                // other values are (committed= 0 and failed = 2)
                FundTransStatus = new Services.Interfaces.DTOs.Account.Fund.AccountFundTransStatusDto { ID = (int)AccountFundTransStatusIds.Pending },

                FundTransType = new Services.Interfaces.DTOs.Account.Fund.AccountFundTransTypeDto { ID = (int)AccountFundTransTypeIds.CreditCard },
                AccountFundType = new Services.Interfaces.DTOs.Account.Fund.AccountFundTypeDto { ID = (int)AccountFundTypeIds.Fund },
                // transaction date, usually current date.
                TransactionDate = Framework.Utilities.Environment.GetServerTime(),
            };

            // return the generated transaction ID to be used as refernce for this transaction.
            return _fundTransactionService.InitiateFundTransaction(transData).Value;
        }

        public PaymentStatus CompletePayment(NameValueCollection queryStrings,ActionContext contexts)
        {
            string refid= queryStrings["ref"];
            if(refid.IndexOf("&ref")>0)
            {
                refid = refid.Substring(0, refid.IndexOf("&ref"));
            }
            if (refid.IndexOf(",") > 0)
            {
                refid = refid.Substring(0, refid.IndexOf(","));
            }
            FundTransactionDto fundDto = _fundTransactionService.GetFundTransactionByref(refid);
            PaymentStatus paymentStatus;
            int _cancel=0;
            if (queryStrings["cancel"] != null && queryStrings["cancel"].Length > 0)
            {
             
                int.TryParse(queryStrings["cancel"], out _cancel);
            }
                
            string accessToken = GetAccessToken(PaymentGatewayDTO.apiref, PaymentGatewayDTO.IntegrationPageUrl, PaymentGatewayDTO.Realm);
            dynamic  api =CheckOrder(refid, PaymentGatewayDTO.apiref, PaymentGatewayDTO.IntegrationPageUrl, accessToken, PaymentGatewayDTO.OutletId, PaymentGatewayDTO.Realm);

            if (fundDto.FundTransStatus.ID == (int)AccountFundTransStatusIds.Pending)
            {
                decimal _amount;
                _amount = api._embedded.payment[0].amount.value;
              //  decimal.TryParse(api._embedded.payment[0].amount.value.toString(), out _amount);
                // amount is stored in dollar not cent.
                _amount = _amount / 100;
                var transData = new PgwFundTransactionResponseDto()
                {
                    // This is Transaction ID generated by our application
                    ID = api.merchantAttributes.merchantOrderReference,
                    // FundTransStatus as provided by the PGW
                    PgwStatus = api._embedded.payment[0].state,
                    ResponseDate = Framework.Utilities.Environment.GetServerTime(),
                    // PGW Transaction ID
                    TransactionId = api._embedded.payment[0].orderReference,
                    // The complete response as provided by the PGW
                    ExtraInfo = queryStrings.ToString(),
                    // amount in response will be used in response validation.
                    Amount = _amount,
                    // receipt number for reference.
                    ReceiptNumber = api._embedded.payment[0].orderReference
                };

                int status = -1;
                // the transaction is comitted, only if the transaction status is zero.
                if (transData.PgwStatus== "CAPTURED")
                {
                    transData.FundTransStatus = new AccountFundTransStatusDto() { ID = (int)AccountFundTransStatusIds.Committed };
                }
                else
                {
                    transData.FundTransStatus = new AccountFundTransStatusDto() { ID = (int)AccountFundTransStatusIds.Failed };
                }

                _fundTransactionService.CloseFundTransaction(transData);


                if (transData.FundTransStatus.ID == (int)AccountFundTransStatusIds.Committed)
                {
                    UrlHelper urlHelper = new UrlHelper(contexts);
                    paymentStatus = new PaymentStatus() { IsCompleted = true, TransationID = transData.ID, Amount = _amount, Message = string.Format(ResourcesUtilities.GetResource("SuccessfullyMsg", "PGW"), transData.Amount, urlHelper.Action("AccountHistory")) };
                }
                else
                {
                    if (transData.PgwStatus == "FAILED" || transData.PgwStatus == "AWAIT_3DS")
                    {
                        paymentStatus = new PaymentStatus() { IsCompleted = false, Message = string.Format(ResourcesUtilities.GetResource("UnsuccessfulTransaction", "PGW"), api.merchantAttributes.merchantOrderReference) };

                    }
                    else if (_cancel == 1)
                    {
                        paymentStatus = new PaymentStatus() { IsCompleted = false, Message = string.Format(ResourcesUtilities.GetResource("CanceledTransaction", "PGW"), api.merchantAttributes.merchantOrderReference) };
                    }
                    else
                    {

                        paymentStatus = new PaymentStatus() { IsCompleted = false, Message = string.Format(ResourcesUtilities.GetResource("UnsuccessfulTransaction", "PGW"), api.merchantAttributes.merchantOrderReference) };
                    }
                }

                return paymentStatus;
            }
            else
            {
                paymentStatus = new PaymentStatus() { IsCompleted = false, Message = string.Format(ResourcesUtilities.GetResource("TransactionAlreadyClosedBR", "Global"), fundDto.ID) };
            }

            return paymentStatus;
        }

        public PaymentStatus ClosePayment(NameValueCollection queryStrings)
        {
            throw new NotImplementedException();
        }

        public string GetAccessToken(string apiKey, string URL, string realmName)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "identity/auth/access-token");
            var request = new RestRequest("GetAccessToken", Method.Post);
            request.AddHeader("Accept", "application/vnd.ni-identity.v1+json");
            request.AddHeader("Authorization", "Basic " + apiKey);
            request.AddHeader("Content-type", "application/vnd.ni-identity.v1+json");
             var body= "{\"realmName\":\"" + realmName + "\" ,\"grant_type\":\"client_credentials\"}";
          //  var body = "{\"grant_type\":\"client_credentials\"}";
            request.AddParameter("application/vnd.ni-identity.v1+json", body, ParameterType.RequestBody);
            RestResponse response = client.ExecuteAsync(request).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                dynamic api = JObject.Parse(response.Content);
                return api.access_token;
            }
            else
            {
                ApplicationContext.Instance.Logger.Error(response.Content);
                return string.Empty;
            }
        }


        public dynamic CheckOrder( string orderdet, string apiKey, string URL , string accessToken, string outletid, string realmn , int count= 1)
        {



            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };



            var client = new RestClient(URL+"transactions/outlets/"+ outletid + "/orders/"+ orderdet);


            var request = new RestRequest("CheckOrder",Method.Post);

            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Accept", "application/vnd.ni-payment.v2+json");
            request.AddHeader("Content-type", "application/vnd.ni-payment.v2+json");

       
            RestResponse response = client.ExecuteAsync(request).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //CAPTURED
                dynamic api = JObject.Parse(response.Content);
                return api;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && count < 3)
            {
                accessToken = GetAccessToken(apiKey, URL, realmn);

                return CheckOrder(orderdet, apiKey, URL, accessToken, outletid, realmn,++count);
            }
            else
            {
                ApplicationContext.Instance.Logger.Error(response.Content);
                return null;
            }

        }

        public string CreateOrder(decimal amount, int transactionId, string apiKey, string URL, string accessToken, string outletid,string realmn,  int count = 1)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            int amountd = Convert.ToInt32(amount * 100) ;
           // var jsonbody = "{\"action\":\"SALE\",\"language\":\"" + Config.CurrentLanguage + "\",  \"merchantAttributes\":{\"merchantOrderReference\":" + transactionId + " , \"cancelUrl\":\"" + string.Format(PaymentGatewayDTO.ReturnPageUrl, Config.CurrentLanguage) + "&cancel=1\",  \"redirectUrl\":\"" + string.Format(PaymentGatewayDTO.ReturnPageUrl, Config.CurrentLanguage) + "\"}" + ",\"amount\":{\"currencyCode\":\"USD\",\"value\":" + amount + "}}";
            var jsonbody = "{\"action\":\"SALE\",\"amount\":{\"currencyCode\":\"USD\",\"value\":"+ amountd + "} ,\"emailAddress\":\""+ Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().EmailAddress + "\"  ,  \"language\":\"" + Config.CurrentLanguage + "\",  \"merchantAttributes\": {\"merchantOrderReference\":" + transactionId + ",\"skip3DS\":true,\"skipConfirmationPage\":false, \"redirectUrl\":\""+ string.Format(PaymentGatewayDTO.ReturnPageUrl, Config.CurrentLanguage) + "\"   ,\"cancelText\": \"Cancel\", \"cancelUrl\":\"" + string.Format(PaymentGatewayDTO.ReturnPageUrl, Config.CurrentLanguage)  + "?cancel=1" + "\"   } }";
            var client = new RestClient(URL + "transactions/outlets/" + outletid + "/orders");
            var request = new RestRequest("CreateOrder", Method.Post);
      


            request.AddHeader("Accept", "application/vnd.ni-payment.v2+json");
            request.AddHeader("Content-type", "application/vnd.ni-payment.v2+json");
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddParameter("application/vnd.ni-payment.v2+json", jsonbody, ParameterType.RequestBody);
            RestResponse response = client.ExecuteAsync(request).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                dynamic api = JObject.Parse(response.Content);
                string refrene = api.reference;
                _fundTransactionService.UpdateFundTransactionByref( new UpdateFundTransactionByrefRequest {  Id=  transactionId,  ReferenceId=refrene });
                return api._links.payment.href;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && count < 3)
            {
                accessToken = GetAccessToken(apiKey, URL, realmn);

                return CreateOrder(amount, transactionId, apiKey, URL, accessToken, outletid, realmn, ++count);
            }
            else
            {
                ApplicationContext.Instance.Logger.Error(response.Content);
            }
            return string.Empty;
        }
    }

    public class VPCStringComparer : IComparer
    {
        /*
         <summary>Customised Compare Class</summary>
         <remarks>
         <para>
         The Virtual Payment Client need to use an Ordinal comparison to Sort on 
         the field names to create the MD5 Signature for validation of the message. 
         This class provides a Compare method that is used to allow the sorted list 
         to be ordered using an Ordinal comparison.
         </para>
         </remarks>
         */

        public int Compare(Object a, Object b)
        {
            /*
             <summary>Compare method using Ordinal comparison</summary>
             <param name="a">The first string in the comparison.</param>
             <param name="b">The second string in the comparison.</param>
             <returns>An int containing the result of the comparison.</returns>
             */

            // Return if we are comparing the same object or one of the 
            // objects is null, since we don't need to go any further.
            if (a == b) return 0;
            if (a == null) return -1;
            if (b == null) return 1;

            // Ensure we have string to compare
            string sa = a as string;
            string sb = b as string;

            // Get the CompareInfo object to use for comparing
            System.Globalization.CompareInfo myComparer = System.Globalization.CompareInfo.GetCompareInfo("en-US");
            if (sa != null && sb != null)
            {
                // Compare using an Ordinal Comparison.
                return myComparer.Compare(sa, sb, System.Globalization.CompareOptions.Ordinal);
            }
            throw new ArgumentException("a and b should be strings.");
        }


    }
}