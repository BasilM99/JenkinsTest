using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.AdServer.Integration.Services.Interfaces;
using Noqoush.AdFalcon.Base;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.DomainServices.Localization;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.Resources;
using Noqoush.Framework.Utilities.EmailsSender;

namespace Noqoush.AdFalcon.AdServer.Integration.Services
{
    public static class EventNames
    {
        public const string CampaignDialyBudgetConsumed = "CampaignDialyBudgetConsumed";
        public const string CampaignStarted = "CampaignStarted";
        public const string CampaignCompleted = "CampaignCompleted";
        public const string CampaignInactive = "CampaignInactive";
        public const string FundAboutTobeConsumed = "FundAboutTobeConsumed";
        public const string CampaignBudgetConsumed = "CampaignBudgetConsumed";


    }

    public class EmailHandlerService : IEmailHandlerService
    {
        private ICampaignRepository campaignRepository;
        protected const string emailSubject = "EmailSubject{0}";

        public EmailHandlerService(ICampaignRepository campaignRepository)
        {
            this.campaignRepository = campaignRepository;
        }
        public void HandelEvent(EventArgsBase args)
        {
            if (args.EventName.Equals(EventNames.CampaignDialyBudgetConsumed) || args.EventName.Equals("CampaignPaused") ||  args.EventName.Equals("CampaignResumed"))
                return;
            var item = campaignRepository.Get(Convert.ToInt32(args.InstanceId));
            Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:AdServer Email Sender handling Event {1}", "Event Broker", args.EventName);
            var emailBody = string.Empty;
            var emailAddress = string.Empty;

            var result = HandelCampaignEvent(args);
            emailBody = result[0];
            emailAddress = result[1];

            /* switch (args.EventName)
             {
                 case EventNames.CampaignDialyBudgetConsumed:
                     {
                         var result = HandelCampaignDialyBudgetConsumed(args);
                         emailBody = result[0];
                         emailAddress = result[1];
                         break;
                     }
                 case EventNames.CampaignStarted:
                     {
                         var result = HandelCampaignDialyBudgetConsumed(args);
                         emailBody = result[0];
                         emailAddress = result[1];
                         break;
                     }
                 case EventNames.CampaignCompleted:
                     {
                         var result = HandelCampaignDialyBudgetConsumed(args);
                         emailBody = result[0];
                         emailAddress = result[1];
                         break;
                     }
             }*/
            if (string.IsNullOrWhiteSpace(emailBody)) return;
            var subject = ResourceManager.Instance.GetResource(string.Format(emailSubject, args.EventName), "EventBroker_Emails", GetCulture(item.Account.PrimaryUser));
            var _mailSender = Framework.IoC.Instance.Resolve<IMailSender>();
            
            if (!Noqoush.AdFalcon.Domain.Configuration.SendAdServerEmails)
            {
                //send only to Event Broker Email
                emailAddress = Noqoush.AdFalcon.Domain.Configuration.EventBrokerEmail;
                _mailSender.SendEmail("", "", emailAddress, emailAddress, subject, emailBody);
            }
            else
            {
                //send to client with Event Broker email as bcc
                _mailSender.SendEmail("", "", emailAddress, emailAddress, subject, emailBody, Bcc: Noqoush.AdFalcon.Domain.Configuration.EventBrokerEmail);

            }
        }

        #region Helpers
        protected CultureInfo GetCulture(User user)
        {
            return user.Language.GetCulture();
        }
        protected string GelLocalizedString(User user, LocalizedString localizedString)
        {
            return localizedString.Get(user.Language.GetCultureName());
        }
        protected string[] GetEventStrs(string emailTemplateName, Campaign campaign)
        {
            var emailBody = new string[2];

            var accountObj = campaign.Account;
            var accountName = accountObj.GetDescription();

            var emailTemplate = ResourceManager.Instance.GetResource(emailTemplateName, "EventBroker_Emails", GetCulture(accountObj.PrimaryUser));
            emailBody[0] = emailTemplate
                .Replace("@AccountName", accountName)
                .Replace("@CampaignName", campaign.GetDescription())
                .Replace("@Date", Framework.Utilities.Environment.GetServerTime().ToString("dd/MM/yyy"));
            emailBody[1] = accountObj.PrimaryUser.EmailAddress;
            return emailBody;
        }

        #endregion
        #region Handlers
        private string[] HandelCampaignEvent(EventArgsBase args)
        {
            var item = campaignRepository.Get(Convert.ToInt32(args.InstanceId));
            return GetEventStrs(args.EventName, item);
        }

        /*private string[] HandelCampaignDialyBudgetConsumed(EventArgsBase args)
        {
            var item = campaignRepository.Get(Convert.ToInt32(args.InstanceId));
            return GetEventStrs("CampaignDialyBudgetConsumed", item);
        }
        private string[] HandelCampaignStarted(EventArgsBase args)
        {
            var item = campaignRepository.Get(Convert.ToInt32(args.InstanceId));
            return GetEventStrs("CampaignStarted", item);
        }
        private string[] HandelCampaignCompleted(EventArgsBase args)
        {
            var item = campaignRepository.Get(Convert.ToInt32(args.InstanceId));
            return GetEventStrs("CampaignCompleted", item);
        }
        private string[] HandelCampaignInactive(EventArgsBase args)
        {
            var item = campaignRepository.Get(Convert.ToInt32(args.InstanceId));
            return GetEventStrs("CampaignInactive", item);
        }
        private string[] HandelFundAboutTobeConsumed(EventArgsBase args)
        {
            var item = campaignRepository.Get(Convert.ToInt32(args.InstanceId));
            return GetEventStrs("FundAboutTobeConsumed", item);
        }*/
        #endregion
    }
}
