using System;
using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.AdServer.Integration.Services.Interfaces;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Persistence.Reports.Repositories;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard;
using Noqoush.Framework.EventBroker;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Protocols;
using System.Net;
using System.Net.Security;

namespace Noqoush.AdFalcon.AdServer.Integration.Services
{
    public static class MessagesEventNames
    {
  
        public const string CampaignStarted = "CampaignStarted";
        public const string CampaignCompleted = "CampaignCompleted";
        public const string CampaignInactive = "CampaignInactive";
        public const string CampaignBudgetConsumed = "CampaignBudgetConsumed";
        public const string PauseCampaign = "Pause Campaign";


    }
    public class MessagesEventBrokerService : IMessagesEventBrokerService
    {
        private ICampaignRepository campaignRepository;
        private IAudienceSegmentRepository audienceSegmentRepository;
        private ISummaryRepository _summaryRepository;
        public MessagesEventBrokerService(ICampaignRepository campaignRepository, IAudienceSegmentRepository AudienceSegmentRepository, ISummaryRepository SummaryRepository)
        {
            this.campaignRepository = campaignRepository;

            this.audienceSegmentRepository = AudienceSegmentRepository;

            this._summaryRepository = SummaryRepository;
        }
        public void HandelEvent(EventArgsBase args)
        {
            if (args.EventName == "CampaignDialyBudgetConsumed" ||  args.EventName == "FundAboutTobeConsumed")
                return;
            if (args.EventName== "AudienceListCheck")
            {
                 HandleAudinceListFilterEvent(args);
                return;
            }
            var item = campaignRepository.Get(Convert.ToInt32(args.InstanceId));
            List<int> SegmentsIdRunning = new List<int>();
            var itemsListSegments = item.GetAudienceSegmentsForExternal();

            Framework.ApplicationContext.Instance.Logger.DebugFormat("{0}:Client  MessagesEventBroker handling Event {1} , MessagesEventBroker for Id: {2}", "Event Broker", args.EventName, args.InstanceId);
            if (itemsListSegments!=null && itemsListSegments.Count>0)
            {
                if (args.EventName== "CampaignStarted" || args.EventName == "CampaignResumed"   )
                {
                    //do activate the list
                    foreach (var itemId in itemsListSegments)
                    {

                        var SegmItem = this.audienceSegmentRepository.Get(itemId);
                        var dataProvider= SegmItem.Provider;
                        var IntegrationId = SegmItem.OperatorSegmentCode;
                        var integrations=  IntegrationId.Split(new char[] { ':' });
                        var Items=integrations[1];
                        int audienceListId = 0;
                        int.TryParse(Items, out audienceListId);


                        callResetApiFunctionAsync(dataProvider.APISiteProviderURL, audienceListId,  true, dataProvider.APIKey, dataProvider.APISecret, dataProvider.CertPath, dataProvider.CertPass, GetCampaignUnixEndTime(item));

                    }
                    return;
                }
                else
                {
                    var advertiseraccountObjId = item.AdvertiserAccount.ID;
                    var performance = new PerformanceCriteria
                    {
                      
                        Ids =new List<int> { advertiseraccountObjId }
                    };

                  


                    var idStatus = _summaryRepository.GetAdsByAdvertiser(performance);


                    var statusAdvString = _summaryRepository.CalculateAdvertiserStatus(idStatus);
                    if (statusAdvString == Framework.Resources.ResourceManager.Instance.GetResource("StatusNotActive", "PMPDeals"))
                    {
                        //Do Dectivte


                        foreach (var itemId in itemsListSegments)
                        {

                            var SegmItem = this.audienceSegmentRepository.Get(itemId);
                            var dataProvider = SegmItem.Provider;
                            var IntegrationId = SegmItem.OperatorSegmentCode;
                            var integrations = IntegrationId.Split(new char[] { ':' });
                            var Items = integrations[1];
                            int audienceListId = 0;
                            int.TryParse(Items, out audienceListId);


                            callResetApiFunctionAsync(dataProvider.APISiteProviderURL, audienceListId, false, dataProvider.APIKey, dataProvider.APISecret, dataProvider.CertPath, dataProvider.CertPass,null);

                        }
                        return;
                    }
                    else
                    {
                      var campaings=  campaignRepository.Query(M => M.AdvertiserAccount.ID == advertiseraccountObjId && M.IsDeleted == false &&( M.EndDate==null  || M.EndDate >= Framework.Utilities.Environment.GetServerTime().AddHours(-12)));

                        foreach (var camp in campaings)
                        {
                            if (camp.Status== AdCampaignStatus.RunningWithAttentionActionNeeded || camp.Status ==  AdCampaignStatus.Running)
                            {
                                var tempres=camp.GetAudienceSegmentsForExternal();
                                if (tempres != null && tempres.Count > 0)
                                    SegmentsIdRunning.AddRange(tempres);
                            }

                        }
                        SegmentsIdRunning= SegmentsIdRunning.Distinct().ToList();

                        foreach (var id in itemsListSegments)
                        {

                            if (!SegmentsIdRunning.Contains(id))
                            {

                                var SegmItem = this.audienceSegmentRepository.Get(id);
                                var dataProvider = SegmItem.Provider;
                                var IntegrationId = SegmItem.OperatorSegmentCode;
                                var integrations = IntegrationId.Split(new char[] { ':' });
                                var Items = integrations[1];
                                int audienceListId = 0;
                                int.TryParse(Items, out audienceListId);


                                callResetApiFunctionAsync(dataProvider.APISiteProviderURL, audienceListId, false, dataProvider.APIKey, dataProvider.APISecret, dataProvider.CertPath, dataProvider.CertPass,null);
                                //do activate
                            }
                            //else
                            // {
                            //    var SegmItem = this.audienceSegmentRepository.Get(id);
                            //    var dataProvider = SegmItem.Provider;
                            //    var IntegrationId = SegmItem.OperatorSegmentCode;
                            //    var integrations = IntegrationId.Split(new char[] { ':' });
                            //    var Items = integrations[1];
                            //    int audienceListId = 0;
                            //    int.TryParse(Items, out audienceListId);


                            //    callResetApiFunction(dataProvider.APISiteProviderURL, audienceListId, false, dataProvider.APIKey, dataProvider.APISecret);

                            //}
                        }
                    }

                }


            }

            return;
        }

        public void HandleAudinceListFilterEvent(EventArgsBase args)
        {


            var item = campaignRepository.Get(Convert.ToInt32(args.InstanceId));
            List<int> SegmentsIdRunning = new List<int>();
            var itemsListSegmentsSt = (args.ExtraParameters[0])["AudienceList"] as string;
            var Action = (args.ExtraParameters[0])["Action"] as string;
            List<int> itemsListSegments = itemsListSegmentsSt.Split(',').Select(int.Parse).ToList();


            var advertiseraccountObjId = item.AdvertiserAccount.ID;
                var performance = new PerformanceCriteria
                {

                    Ids = new List<int> { advertiseraccountObjId }
                };

            Framework.ApplicationContext.Instance.Logger.DebugFormat("{0}:Client  MessagesEventBroker handling Event {1} , MessagesEventBroker for Id: {2}  -Action: {3}", "Event Broker", args.EventName, args.InstanceId,Action);

            if (Action == "Delete")
            {
                var idStatus = _summaryRepository.GetAdsByAdvertiser(performance);


                var statusAdvString = _summaryRepository.CalculateAdvertiserStatus(idStatus);
                if (statusAdvString == Framework.Resources.ResourceManager.Instance.GetResource("StatusNotActive", "PMPDeals"))
                {
                    //Do Dectivte


                    foreach (var itemId in itemsListSegments)
                    {

                        var SegmItem = this.audienceSegmentRepository.Get(itemId);
                        var dataProvider = SegmItem.Provider;
                        var IntegrationId = SegmItem.OperatorSegmentCode;
                        var integrations = IntegrationId.Split(new char[] { ':' });
                        var Items = integrations[1];
                        int audienceListId = 0;
                        int.TryParse(Items, out audienceListId);


                        callResetApiFunctionAsync(dataProvider.APISiteProviderURL, audienceListId, false, dataProvider.APIKey, dataProvider.APISecret, dataProvider.CertPath, dataProvider.CertPass,null);

                    }
                    return;
                }
                else
                {
                    var campaings = campaignRepository.Query(M => M.AdvertiserAccount.ID == advertiseraccountObjId && M.IsDeleted == false && (M.EndDate == null || M.EndDate >= Framework.Utilities.Environment.GetServerTime().AddHours(-12)));

                    foreach (var camp in campaings)
                    {
                        if (camp.Status == AdCampaignStatus.RunningWithAttentionActionNeeded || camp.Status == AdCampaignStatus.Running)
                        {
                            var tempres = camp.GetAudienceSegmentsForExternal();
                            if (tempres != null && tempres.Count > 0)
                                SegmentsIdRunning.AddRange(tempres);
                        }

                    }
                    SegmentsIdRunning = SegmentsIdRunning.Distinct().ToList();

                    foreach (var id in itemsListSegments)
                    {

                        if (!SegmentsIdRunning.Contains(id))
                        {

                            var SegmItem = this.audienceSegmentRepository.Get(id);
                            var dataProvider = SegmItem.Provider;
                            var IntegrationId = SegmItem.OperatorSegmentCode;
                            var integrations = IntegrationId.Split(new char[] { ':' });
                            var Items = integrations[1];
                            int audienceListId = 0;
                            int.TryParse(Items, out audienceListId);


                            callResetApiFunctionAsync(dataProvider.APISiteProviderURL, audienceListId, false, dataProvider.APIKey, dataProvider.APISecret, dataProvider.CertPath, dataProvider.CertPass,null);
                            //do activate
                        }
                        //else
                        // {
                        //    var SegmItem = this.audienceSegmentRepository.Get(id);
                        //    var dataProvider = SegmItem.Provider;
                        //    var IntegrationId = SegmItem.OperatorSegmentCode;
                        //    var integrations = IntegrationId.Split(new char[] { ':' });
                        //    var Items = integrations[1];
                        //    int audienceListId = 0;
                        //    int.TryParse(Items, out audienceListId);


                        //    callResetApiFunction(dataProvider.APISiteProviderURL, audienceListId, false, dataProvider.APIKey, dataProvider.APISecret);

                        //}
                    }
                }

            }
            else if (Action == "Add")
            {


                foreach (var id in itemsListSegments)
                {

                   // if (!SegmentsIdRunning.Contains(id))
                    //{

                        var SegmItem = this.audienceSegmentRepository.Get(id);
                        var dataProvider = SegmItem.Provider;
                        var IntegrationId = SegmItem.OperatorSegmentCode;
                        var integrations = IntegrationId.Split(new char[] { ':' });
                        var Items = integrations[1];
                        int audienceListId = 0;
                        int.TryParse(Items, out audienceListId);


                        callResetApiFunctionAsync(dataProvider.APISiteProviderURL, audienceListId, true, dataProvider.APIKey, dataProvider.APISecret, dataProvider.CertPath, dataProvider.CertPass, GetDefaultUnixEndTime());

                    //}


                }
            }
            else if (Action == "CampaignEndDateUpdateSegment")
            {
                //callResetApiFunctionAsync(dataProvider.APISiteProviderURL, audienceListId, true, dataProvider.APIKey, dataProvider.APISecret, dataProvider.CertPath, dataProvider.CertPass);

                foreach (var id in itemsListSegments)
                {

                    // if (!SegmentsIdRunning.Contains(id))
                    //{

                    var SegmItem = this.audienceSegmentRepository.Get(id);
                    var dataProvider = SegmItem.Provider;
                    var IntegrationId = SegmItem.OperatorSegmentCode;
                    var integrations = IntegrationId.Split(new char[] { ':' });
                    var Items = integrations[1];
                    int audienceListId = 0;
                    int.TryParse(Items, out audienceListId);


                    callResetApiFunctionAsync(dataProvider.APISiteProviderURL, audienceListId, true, dataProvider.APIKey, dataProvider.APISecret, dataProvider.CertPath, dataProvider.CertPass, GetCampaignUnixEndTime(item));

                    //}


                }
            }
        }


        public long GetCampaignUnixEndTime(Campaign item)
        {

            if (item.EndDate.HasValue)
            {

                var dt = item.EndDate.Value.ToUniversalTime();
                var unixTime = ((DateTimeOffset)dt).ToUnixTimeSeconds();
                return unixTime;
            }
            else
            {

                var dt = Framework.Utilities.Environment.GetServerTime().AddDays(90);
                var unixTime = ((DateTimeOffset)dt).ToUnixTimeSeconds();
                return unixTime;
            }
         

        }
        public long GetDefaultUnixEndTime()
        {


                var dt = Framework.Utilities.Environment.GetServerTime().AddDays(7);
                var unixTime = ((DateTimeOffset)dt).ToUnixTimeSeconds();
                return unixTime;
      


        }

        /*
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://url");
        httpWebRequest.ContentType = "application/json";
httpWebRequest.Method = "POST";
using (var streamWriter = new

StreamWriter(httpWebRequest.GetRequestStream()))
{
    string json = new JavaScriptSerializer().Serialize(new
    {
        Username = "myusername",
        Password = "password"
    });

        streamWriter.Write(json);
}
    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
{
    var result = streamReader.ReadToEnd();
}*/
        public async System.Threading.Tasks.Task<bool> callResetApiFunctionAsync(string baseURL , int idfromIntegration ,bool active, string apiKey , string apiSecret  , string certficatePath , string certficatePassord,long? enddateAct)
        {
            //var httpClientHandler = new HttpClientHandler { AllowAutoRedirect = true, UseDefaultCredentials = false };
            var handler = new WebRequestHandler { AllowAutoRedirect = true, UseDefaultCredentials = false };
            Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:callResetApiFunction for audience list Id {1} to be {2} success", "Event Broker", idfromIntegration, certficatePath);
            if (!string.IsNullOrEmpty(certficatePath))
            {
                if (!string.IsNullOrEmpty(certficatePassord))
                    handler.ClientCertificates.Add(new X509Certificate2(@certficatePath, certficatePassord, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet |
X509KeyStorageFlags.PersistKeySet));
                else
                    handler.ClientCertificates.Add(new X509Certificate2(@certficatePath));

                ServicePointManager.ServerCertificateValidationCallback =
           delegate (object sender, X509Certificate certificate, X509Chain
    chain, SslPolicyErrors sslPolicyErrors)
           {
               return true;
           };
            }

            using (var client = new HttpClient(handler))

            {
     
                  client.DefaultRequestHeaders.Accept.Clear();
                // Add an Accept header for JSON format.    
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Api-key", apiKey);
                client.DefaultRequestHeaders.Add("Api-Secret", apiSecret);

                //                var values = new Dictionary<string, string>
                //{
                //   { "audience_list_id", idfromIntegration.ToString()},
                //   { "active", active.ToString() }
                //};

                //                var content = new FormUrlEncodedContent(values);


                string myJson = string.Empty;
               // enddateAct = null;
                if (enddateAct.HasValue)
                {
                  
                    
                    myJson = "{\"audience_list_id\":" + idfromIntegration + ",\"active\":" + active.ToString().ToLower() + ",\"activation_end_date\":" + enddateAct.Value + "}";
               
                
                }
                else
                {
                    myJson = "{\"audience_list_id\":" + idfromIntegration + ",\"active\":" + active.ToString().ToLower() + "}";

                }
                client.BaseAddress = new Uri(baseURL);
       
                var url = "api/update-active";


        
                var response = await  client.PostAsync(url, new StringContent(myJson, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    if (!enddateAct.HasValue)
                    {
                        enddateAct = 0;
                    }
                    Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:callResetApiFunction for audience list Id {1} to be {2} success {3} time in sec", "Event Broker", idfromIntegration, active.ToString().ToLower() , enddateAct );
                    return true;
                }
                else {
                    if (!enddateAct.HasValue)
                    {
                        enddateAct = 0;
                    }
                    Framework.ApplicationContext.Instance.Logger.DebugFormat("{0}:callResetApiFunction for audience list Id {1} to be {2} failed , response:{3},{4} time in sec", "Event Broker", idfromIntegration, active.ToString().ToLower(), response.ReasonPhrase, enddateAct);
                    return false;

                }

            }



        }
    }

}
