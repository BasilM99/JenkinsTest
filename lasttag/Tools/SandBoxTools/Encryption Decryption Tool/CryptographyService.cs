using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryption
{
    public class CryptographyService
    {
        static void Main()
        {
            string opt = "", EncryptionWord = "", DecryptionWord = "", key = "";
            while (true)
            {
                try
                {
                    Console.Write("choose 1 for Encrypt and 2 for Decrypt :");
                    opt = Console.ReadLine();
                    switch (opt)
                    {
                        case "1":
                            Console.Write("\n Insert a string : ");
                            EncryptionWord = Console.ReadLine();

                            if (EncryptionWord != "")
                            {
                                Console.Write(" Insert a Key or press enter if u dont want to : ");
                                key = Console.ReadLine();

                                if (string.IsNullOrEmpty(key))
                                    Console.WriteLine("\t Result is : " + Encrypt(EncryptionWord));
                                else
                                    Console.WriteLine("\t Result is : " + Encrypt(EncryptionWord, key));
                            }
                            Console.WriteLine("\n---------------------------------------------------------------------");
                            Console.WriteLine("---------------------------------------------------------------------\n");
                            break;
                        case "2":
                            Console.Write("\n Insert a string : ");
                            DecryptionWord = Console.ReadLine();

                            if (DecryptionWord != "")
                            {
                                Console.Write(" Insert a Key or press enter if u dont want to : ");
                                key = Console.ReadLine();

                                if (string.IsNullOrEmpty(key))
                                    Console.WriteLine("\t Result is : " + Decrypt(DecryptionWord));
                                else
                                    Console.WriteLine("\t Result is : " + Decrypt(DecryptionWord, key));


                            }
                            Console.WriteLine("\n---------------------------------------------------------------------");
                            Console.WriteLine("---------------------------------------------------------------------\n");

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
                    if (e.InnerException != null)
                        Console.WriteLine("\n" + e.InnerException.Message);

                    else
                        Console.WriteLine("\n" + e.Message);

                    Console.WriteLine("\n---------------------------------------------------------------------");
                    Console.WriteLine("---------------------------------------------------------------------\n");
                }


            }

        }
        static string Encrypt(string Value, string key = null)
        {
            string EncryptedValue;
            try
            {
                EncryptedValue = Noqoush.Framework.Utilities.Cryptography.Encrypt(Value, true, key);

            }
            catch (Exception e)
            {

                throw e;
            }

            return EncryptedValue;

        }

        static string Decrypt(string Value, string key = null)
        {
            string DecryptedValue;
            try
            {
                DecryptedValue = Noqoush.Framework.Utilities.Cryptography.Decrypt(Value, true, key);

            }
            catch (Exception e)
            {

                throw e;
            }
            return DecryptedValue;

        }

    }
}
