using LumenWorks.Framework.IO.Csv;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Server;
using System.Net;
using WcfServiceLibrary1;

namespace MigrationAdvertiser
{


    class Program
    {

        static void Main()
        {

            UpdateAdv();
            UpdateCamp();
        }

        static void UpdateAdv()
        {

            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionStringVal"];
            // MySql Connection Object
            MySqlConnection conn = new MySqlConnection(connStr);

            //  csv file path
            string file = System.Configuration.ConfigurationManager.AppSettings["FilePathAdv"];
            string ServicePath = System.Configuration.ConfigurationManager.AppSettings["ServicePath"];

            //Get an instance of the service
            ServiceFactory<Noqoush.AdFalcon.Server.CommunicationService.Services.ICommunicationService> serviceFactory = new ServiceFactory<Noqoush.AdFalcon.Server.CommunicationService.Services.ICommunicationService>();
            Noqoush.AdFalcon.Server.CommunicationService.Services.ICommunicationService service = serviceFactory.GetService(ServicePath);
            //  csv file path


            if (file != "")
            {
                string path = file;
                string Id = "";
                string Url = "";
                List<string> objects = new List<string>();
                using (CachedCsvReader csv = new
         CachedCsvReader(new StreamReader(path), true, ','))
                {
                    // missing fields will not throw an exception,
                    // but will instead be treated as if there was a null value
                    csv.MissingFieldAction = MissingFieldAction.ReplaceByNull;
                    // to replace by "" instead, then use the following action:
                    //csv.MissingFieldAction = MissingFieldAction.ReplaceByEmpty;
                    int fieldCount = csv.FieldCount;
                    string[] headers = csv.GetFieldHeaders();
                    while (csv.ReadNextRecord())
                    {
                        Id = csv[0].ToString();
                        Url = csv[1].ToString();

                        if ((objects.Count > 0 && !objects.Contains(Id)) || objects.Count == 0)
                            if (Id != "" && Url != "")
                            {

                                MySqlConnection con = new MySqlConnection(connStr);
                                con.Open();
                                string test = string.Format("UPDATE advertisers SET DomainURL='{0}' WHERE Id='{1}';", Url, Id);
                                MySqlCommand icmmd = new MySqlCommand(test, con);
                                service.BroadcastEntityUpdates(Noqoush.AdFalcon.Server.CommunicationService.Services.EntityType.Advertiser, Id.ToString());

                                icmmd.ExecuteNonQuery();
                                con.Close();
                            }
                        objects.Add(Id);

                    }
                }

            }


        }
        static void UpdateCamp()
        {

            string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionStringVal"];
            // MySql Connection Object
            MySqlConnection conn = new MySqlConnection(connStr);

            //  csv file path
            string file = System.Configuration.ConfigurationManager.AppSettings["FilePathCamp"];
            //string ServicePath = System.Configuration.ConfigurationManager.AppSettings["ServicePath"];

            ////Get an instance of the service
            //ServiceFactory<Noqoush.AdFalcon.Server.CommunicationService.Services.ICommunicationService> serviceFactory = new ServiceFactory<Noqoush.AdFalcon.Server.CommunicationService.Services.ICommunicationService>();
            //Noqoush.AdFalcon.Server.CommunicationService.Services.ICommunicationService service = serviceFactory.GetService(ServicePath);
            ////  csv file path


            if (file != "")
            {
                string path = file;
                string Id = "";
                string Url = "";
                List<string> objects = new List<string>();
                using (CachedCsvReader csv = new
         CachedCsvReader(new StreamReader(path), true, ','))
                {
                    // missing fields will not throw an exception,
                    // but will instead be treated as if there was a null value
                    csv.MissingFieldAction = MissingFieldAction.ReplaceByNull;
                    // to replace by "" instead, then use the following action:
                    //csv.MissingFieldAction = MissingFieldAction.ReplaceByEmpty;
                    int fieldCount = csv.FieldCount;
                    string[] headers = csv.GetFieldHeaders();
                    while (csv.ReadNextRecord())
                    {
                        Url = csv[0].ToString();
                        Id = csv[1].ToString();

                        if ((objects.Count > 0 && !objects.Contains(Url)) || objects.Count == 0)
                            if (Id != "" && Url != "")
                            {

                                MySqlConnection con = new MySqlConnection(connStr);
                                con.Open();
                                string test = string.Format("UPDATE campaigns SET AdvertiserId ='{0}' WHERE DomainURL='{1}' and AdvertiserId ='';", Id, Url);
                                MySqlCommand icmmd = new MySqlCommand(test, con);
                                //service.BroadcastEntityUpdates(Noqoush.AdFalcon.Server.CommunicationService.Services.EntityType.Advertiser, Id.ToString());

                                icmmd.ExecuteNonQuery();
                                con.Close();
                            }

                        objects.Add(Url);
                    }
                }

            }


        }
    }


}
