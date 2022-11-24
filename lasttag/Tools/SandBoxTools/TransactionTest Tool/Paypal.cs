
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;


using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;


namespace cSharpSignatureExamples
{
    public class Paypal
    {
        static string USER = Noqoush.Framework.Utilities.Cryptography.Decrypt(System.Configuration.ConfigurationManager.AppSettings["USER"], true);
        static string PWD = Noqoush.Framework.Utilities.Cryptography.Decrypt(System.Configuration.ConfigurationManager.AppSettings["PWD"], true);
        static string SIGNATURE = Noqoush.Framework.Utilities.Cryptography.Decrypt(System.Configuration.ConfigurationManager.AppSettings["SIGNATURE"], true);

        public static void GetTransaction(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    string endpoint = "https://api-3t.sandbox.paypal.com/nvp";
                    // This are the URLs the PayPal process uses. The endpoint URL is created using the NVP string generated below while the redirect url is where the page the user will navigate to when leaving PayPal plus the PayerID and the token the API returns when the request is made.

                    string NVP = string.Empty;

                    // API call method: add the desired checkout method. As I've mentioned above, we're using the express checkout.
                    NVP += "METHOD=GetTransactionDetails";
                    NVP += "&VERSION=94";
                    NVP += "&TRANSACTIONID=" + id;

                    // Credentials identifying you as the merchant
                    NVP += "&USER=" + USER;
                    NVP += "&PWD=" + PWD;
                    NVP += "&SIGNATURE=" + SIGNATURE;


                    // Make the API call to the PayPal Service
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                    request.Method = "POST";
                    request.ContentLength = NVP.Length;

                    string sResponse = string.Empty;
                    using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                    {
                        sw.Write(NVP);
                    }

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        sResponse = sr.ReadToEnd();
                    }


                    // string[] splitResponse = sResponse.Split('&');
                    if (sResponse.Length > 0)
                    {
                        if (sResponse.Contains("Success"))
                        {
                            Console.WriteLine("\t Success");
                        }
                        else if (sResponse.Contains("Failure"))
                        {
                            Console.WriteLine("\t Failed or Invalid Id");

                        }
                        Console.WriteLine("");
                        Console.WriteLine("---------------------------------------------------------------------");
                        Console.WriteLine("---------------------------------------------------------------------");
                        Console.WriteLine("");
                    }
                    else
                    {

                        Console.WriteLine("\t API retuned missing data , please make sure of the Id u entered !");
                        Console.WriteLine("");
                        Console.WriteLine("---------------------------------------------------------------------");
                        Console.WriteLine("---------------------------------------------------------------------");
                        Console.WriteLine("");
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            else
            {
                Console.WriteLine("Invalid Id !");
            }
        }

    }
}