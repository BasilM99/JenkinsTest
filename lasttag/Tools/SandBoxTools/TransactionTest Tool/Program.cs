using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Noqoush.Framework;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Net;
using cSharpSignatureExamples;
namespace TransactionTestTool
{
    class Program
    {
        static string Key = System.Configuration.ConfigurationManager.AppSettings["MIGS"];
        static Dictionary<string, string> Settings;

        static Dictionary<string, string> Read()
        {
            if (Settings == null)
            {
                var decryptedConfigData = Noqoush.Framework.Utilities.Cryptography.Decrypt(Key, true);
                Settings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                if (!string.IsNullOrEmpty(decryptedConfigData))
                {
                    XElement dt = XElement.Parse(decryptedConfigData);
                    foreach (var c in dt.Elements())
                        Settings.Add(c.Attribute("key").Value, c.Attribute("value").Value);
                }
            }
            return Settings;
        }

        static void Main(string[] args)
        {

            string type = "";
            while (true)
            {
                try
                {
                    Console.Write("Please please enter 1 for Transaction or 2 for Paypal : ");
                    type = Console.ReadLine();
                    switch (type)
                    {
                        case "1":
                            handelTransaction();
                            break;
                        case "2":
                            handelPaypalInput();
                            break;
                        case "clear":
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("\t :/ Cant u read well !! , it's either 1 or 2 ! \n");
                            break;
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }

        }
        static void handelTransaction()
        {
            try
            {
                Console.Write("Please enter a TransactionID : ");
                string id = Console.ReadLine(), responsetxt = "";
                if (id != "")
                {
                    Console.WriteLine("");
                    Console.WriteLine("\t Result is : ");
                    Dictionary<string, string> Setting = Read();

                    var PgwResolvePendingTransactionsResult = Transaction.PgwResolvePendingTransactions(Setting, out responsetxt, id);
                    Transaction.PrintFundTransaction(PgwResolvePendingTransactionsResult, responsetxt);
                }
                else
                {
                    Console.WriteLine("Somthing went wrong ! please try again ");

                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        static void handelPaypalInput()
        {
            try
            {
                Console.Write("Please enter a PayPal TransactionID : ");
                string id = Console.ReadLine();
                if (id != "")
                {
                    Console.WriteLine("");
                    Console.WriteLine("\t Result is : ");
                    Paypal.GetTransaction(id);
                }
                else
                {
                    Console.WriteLine("Somthing went wrong ! please try again ");

                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

    }
}
