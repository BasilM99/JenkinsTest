using ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.ReportSchedule;
using ArabyAds.AdFalcon.EventDTOs;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Integration;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.Framework;
using ArabyAds.Framework.DistributedEventBroker.PubSub;
using ArabyAds.Framework.EventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.Utilities.Events
{
    public static class KafkaEvents
    {



        public static void CampaignInvoiced(CampaignOverspend evt)
        {
            try
            {
                ApplicationContext.Instance.Logger.Info($"Begin CampaignOverspend handled with key {evt.Id}  with Amount {evt.Amount} ");

                if (evt.Amount > 0)
                {
                    SchedulerHelper.FundTransactionService.AddOverBudgetReturnFundFromCampaign(new AddOverBudgetReturnFundRequest { Id = Convert.ToInt32(evt.Id), InvoiceAmount = evt.Amount });
                    ApplicationContext.Instance.Logger.Info($"CampaignInvoiced handled with key {evt.Id}  with Amount {evt.Amount} ");
                }
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.ErrorFormat("{0}:error in  CampaignInvoiced handling Event {1} , AddOverBudgetReturnFundFromCampaign , \n message:{2}", "Event Broker", evt.Id, ex.ToString());
               // throw new PersistentEventHandlingException();
            }

        }

        public static void CampaignBillingInfoChangedProcessed(CampaignBillingInfoChangedAck evt)
        {
            try{ 
            ApplicationContext.Instance.Logger.Info($"Begin CampaignBillingInfoChangedAck handled with key {evt.Id}  with Amount {evt.CommittedAmount}  with requested Amount {evt.RequestedAmount}");
            if (evt.RequestedAmount != evt.CommittedAmount)
            {
                SchedulerHelper.FundTransactionService.SendCampaignbillingInfoacknowledgment(new SendBillingInfoacknowledgmentRequest { Id = evt.Id, FieldToChange = "Budget", RequestedAmount = evt.RequestedAmount, CommittedAmount = evt.CommittedAmount, ModifiedOn = evt.ModifiedOn });
                ApplicationContext.Instance.Logger.Info($"CampaignBillingInfoChangedProcessed handled with key {evt.Id}  with Amount {evt.CommittedAmount}  with requested Amount {evt.RequestedAmount}");
            }

        }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.ErrorFormat("{0}:error in  CampaignBillingInfoChangedProcessed handling Event {1} , SendCampaignbillingInfoacknowledgment , \n message:{2}", "Event Broker", evt.Id, ex.ToString());
               // throw new PersistentEventHandlingException();
    }

}


        public static void AdGroupBillingInfoChangedProcessed(AdGroupBillingInfoChangedAck evt)
        {
            try
            {
                ApplicationContext.Instance.Logger.Info($"Begin AdGroupBillingInfoChangedAck handled with key {evt.Id}  with Amount {evt.CommittedAmount}  with requested Amount {evt.RequestedAmount}");
                if (evt.RequestedAmount != evt.CommittedAmount)
                {
                    SchedulerHelper.FundTransactionService.SendAdGroupbillingInfoacknowledgment(new SendBillingInfoacknowledgmentRequest { Id = evt.Id, FieldToChange = "Budget", RequestedAmount = evt.RequestedAmount, CommittedAmount = evt.CommittedAmount, ModifiedOn = evt.ModifiedOn });
                    ApplicationContext.Instance.Logger.Info($"AdGroupBillingInfoChangedProcessed handled with key {evt.Id}  with Amount {evt.CommittedAmount}  with requested Amount {evt.RequestedAmount}");
                }
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.ErrorFormat("{0}:error in  AdGroupBillingInfoChangedProcessed handling Event {1} , SendAdGroupbillingInfoacknowledgment , \n message:{2}", "Event Broker", evt.Id, ex.ToString());
                //throw new PersistentEventHandlingException();
            }
        }

        public static void FundOverSpendProcessed(FundOverSpend evt)
        {

            //SchedulerHelper.FundTransactionService.SendAdGroupbillingInfoacknowledgment(evt.Id, "Budget", evt.RequestedAmount, evt.CommittedAmount, evt.ModifiedOn);
            ApplicationContext.Instance.Logger.Info($"FundOverSpendProcessed handled with key {evt.Id}  with Amount {evt.NetOverSpendAmount}");

        }


        public static void CampaignStatusChangedProcessed(CampaignStatusChanged evt)
        {
            ApplicationContext.Instance.Logger.Info($"Begin CampaignStatusChanged  handled with key {evt.CampaignId}  with status {evt.NewStatus.ToString()} ");
            try
            {
                SchedulerHelper.EmailHandlerService.HandelEvent( new EventArgsBase(GetEventsString(evt.NewStatus), evt.CampaignId.ToString()));
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.ErrorFormat("{0}:error in  CampaignStatusChangedProcessed handling Event {1} , EmailHandlerService , \n message:{2}", "Event Broker", evt.CampaignId, ex.ToString());
               // throw new PersistentEventHandlingException();
            }

        
           try
            {

                SchedulerHelper.MessagesEventBrokerService.HandelEvent(new EventArgsBase(GetEventsString(evt.NewStatus), evt.CampaignId.ToString()));
             ApplicationContext.Instance.Logger.Info($"CampaignStatusChanged handled with key {evt.CampaignId}  with status {evt.NewStatus.ToString()} ");

            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.ErrorFormat("{0}:error in  CampaignStatusChanged handling Event {1} , HandelEvent , \n message:{2}", "Event Broker", evt.CampaignId, ex.ToString());
                //throw new PersistentEventHandlingException();
            }

            }
        public static void CampaignFundAboutTobeConsumedProcessed(CampaignFundAboutTobeConsumed evt)
        {
            try
            {
                ApplicationContext.Instance.Logger.Info($"Begin CampaignFundAboutTobeConsumed  handled with key {evt.CampaignId}");


                SchedulerHelper.EmailHandlerService.HandelEvent(new EventArgsBase("FundAboutTobeConsumed", evt.CampaignId.ToString()));
                ApplicationContext.Instance.Logger.Info($"CampaignFundAboutTobeConsumed handled with key {evt.CampaignId}");
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.ErrorFormat("{0}:error in  CampaignFundAboutTobeConsumed handling Event {1} , HandelEvent , \n message:{2}", "Event Broker", evt.CampaignId, ex.ToString());
               // throw new PersistentEventHandlingException();
            }


        }
        public static void AudienceListProcessed(AudienceListCheck  evt)
        {
            try
            {
                ApplicationContext.Instance.Logger.Info($"Begin AudienceListProcessed  handled with key {evt.CampaignId}");
                IList<Dictionary<string, object>> ars = new List<Dictionary<string, object>>();
                Dictionary<string, object> ids = new Dictionary<string, object>();
                ids.Add("AudienceList", string.Join<int>(",", evt.Ids) );
                ids.Add("Action", evt.Action);
                ars.Add(ids);
                SchedulerHelper.MessagesEventBrokerService.HandelEvent(new EventArgsBase("AudienceListCheck", evt.CampaignId.ToString(),null,ars));
                ApplicationContext.Instance.Logger.Info($"AudienceListProcessed handled with key {evt.CampaignId}");
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.ErrorFormat("{0}:error in  AudienceListProcessed handling Event {1} , HandelEvent , \n message:{2}", "Event Broker", evt.CampaignId, ex.ToString());
                // throw new PersistentEventHandlingException();
            }


        }
        public static string GetEventsString(CampaignStatus status)
        {

            if (status == CampaignStatus.Resumed)
            {
                return "CampaignResumed";
            }
            else if (status == CampaignStatus.Paused)
            {

                return "CampaignPaused";
            }
            else if (status == CampaignStatus.Started)
            {

                return "CampaignStarted";
            }

            else if (status == CampaignStatus.Inactive)
            {

                return "CampaignInactive";
            }
            else if (status == CampaignStatus.Completed)
            {

                return "CampaignCompleted";
            }

            else if (status == CampaignStatus.DailyBudgetConsumed)
            {

                return "CampaignDialyBudgetConsumed";
            }

            else if (status == CampaignStatus.BudgetConsumed)
            {

                return "CampaignBudgetConsumed";
            }
            return string.Empty;
        }

    }
}
