using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NHibernate;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.Payment;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Objective;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework;
using ArabyAds.Framework.ConfigurationSetting;
using ArabyAds.Framework.DomainServices;
using ArabyAds.Framework.DomainServices.AuditTrial;
using ArabyAds.Framework.DomainServices.EventBroker;
using ArabyAds.Framework.DomainServices.Localization;
using ArabyAds.Framework.DomainServices.Localization.Repositories;
using ArabyAds.Framework.EventBroker;
using ArabyAds.Framework.Persistence;
using ArabyAds.Framework.Resources;
using ArabyAds.Framework.Utilities.EmailsSender;
using ArabyAds.AdFalcon.Domain.Repositories.Account.Payment;
//using ArabyAds.AdFalcon.Base;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;

namespace ArabyAds.AdFalcon.Services.Utility
{

    public class AccountEmailSender : EmailSender
    {
        public AccountEmailSender()
        {
            CurrentEntityAccount = null;
        }

        protected Account CurrentEntityAccount { get; set; }

        #region Helpers
        protected CultureInfo GetCulture(User user)
        {
            return user.Language.GetCulture();
        }
        protected string GelLocalizedString(User user, LocalizedString localizedString)
        {
            return localizedString.Get(user.Language.GetCultureName());
        }
        #endregion

        #region Implementation of ISubscriberHandler
        private static IPaymentTypeRepository _paymentTypeRepository = null;
        private static IPaymentTypeRepository PaymentTypeRepository
        {
            get
            {
                if (_paymentTypeRepository == null)
                {
                    _paymentTypeRepository = Framework.IoC.Instance.Resolve<IPaymentTypeRepository>();
                }
                return _paymentTypeRepository;
            }
        }
        //public void HandleEvent<T>(EventArgsBase<T> args) where T : class, new()
        public override void HandleEvent(EventArgsBase args)
        {
            //Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:Email Sender handling Event {1}", "Event Broker", args.EventName);
            var emailInfo = new EmailInfo();
            switch (args.EventName)
            {
                case EventNames.Add_Payment:
                    {
                        emailInfo = HandelAddPayment(args);
                        break;
                    }
                case EventNames.Add_Fund:
                    {
                        emailInfo = HandelAddFund(args);
                        break;
                    }
                case EventNames.AppSiteApprove:
                    {
                        emailInfo = HandelAppSiteApproval(args);
                        break;
                    }
            }
            //Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:Event {1} email Body:{2}", "Event Broker", args.EventName, emailBody);
            if (emailInfo == null) return;
            //Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:Event {1} email subject:{2}", "Event Broker", args.EventName, subject);
            var _mailSender = Framework.IoC.Instance.Resolve<IMailSender>();
            
            _mailSender.SendEmail("", Domain.Configuration.NoReplyTenant, emailInfo.Address, emailInfo.Address, emailInfo.Subject, emailInfo.Body, true, emailInfo.BCC);
        }



        #region Handlers
        #region Admin
        private EmailInfo HandelAddFund(EventArgsBase args)
        {
            var result = new EmailInfo();
            IList<EntityEventData> eventData = args.Data.Select(item => item as EntityEventData).ToList();
            IList<AccountFundTransHistory> fundObjs =
                (from data in eventData
                 where data.Entity is AccountFundTransHistory
                 select data.Entity as AccountFundTransHistory).ToList();

            eventData.Select(item => item.Entity as Account).ToList();
            var fundObj = fundTransRepository.Get(fundObjs.First().ID);//fundObjs.First();;
            var accountObj = accountRepository.Get(fundObj.AccountId);
            CurrentEntityAccount = accountObj;
            var accountSummaryObj = accountObj.AccountSummary;

            var accountName = accountObj.GetDescription();
            var accountTotal = accountSummaryObj.Funds;
            if (fundObj.FundTransStatus.ID == AccountFundTransStatus.Committed.ID)
            {
                var emailTemplate = string.Empty;
                if (fundObj.VATAmount > 0)
                    emailTemplate = ResourceManager.Instance.GetResource("AddFundAdminVAT_Account", "EventBroker_Emails", GetCulture(accountObj.PrimaryUser));
                else
                {
                    if(!fundObj.ObjectRelatedId.HasValue)
                    emailTemplate = ResourceManager.Instance.GetResource("AddFundAdmin_Account", "EventBroker_Emails", GetCulture(accountObj.PrimaryUser));

                    else
                        emailTemplate = ResourceManager.Instance.GetResource("KafkaEventOverBudgetAck", "EventBroker_Emails", GetCulture(accountObj.PrimaryUser));
                }

                result.Body = emailTemplate
                    .Replace("@AccountName", accountName)
                    .Replace("@Fund", FormatDecimal(fundObj.Amount))
                    .Replace("@VATValue", FormatDecimal(fundObj.VATAmount))
                            .Replace("@FundTotal", FormatDecimal(fundObj.VATAmount + fundObj.Amount))
                    .Replace("@Type", GelLocalizedString(accountObj.PrimaryUser, fundObj.FundTransType.Name)) //GetFundPaymentDetailDesc(fundObj))
                    .Replace("@Date", fundObj.TransactionDate.ToString("dd/MM/yyy"))
                    .Replace("@TotalFund", FormatDecimal(accountTotal));
                if (fundObj.ObjectRelatedId.HasValue)
                {
                    result.Body= result.Body.Replace("@CampaignName", args.ExtraParameters[0].Value["CampaignName"].ToString());

                }
            }

            if (!(args.ExtraParameters.Count != 0 && args.ExtraParameters.Where(p => p.Value.ContainsKey("NotifyUser") && Convert.ToBoolean(p.Value["NotifyUser"]) == false).Count() != 0))
            {
                result.Address = accountObj.PrimaryUser.EmailAddress;
                result.BCC = Domain.Configuration.FinanceEmail;
            }
            else
            {
                result.Address = Domain.Configuration.FinanceEmail;
            }
            if (fundObj.FundTransType!=null && fundObj.FundTransType.ID == AccountFundTransType.OverBudgetRefund.ID)
            {
                result.Subject = ResourceManager.Instance.GetResource("KafkaEventOverBudgetSubject", "EventBroker_Emails", GetCulture(CurrentEntityAccount.PrimaryUser));
            }
            else
            {
                result.Subject = ResourceManager.Instance.GetResource(string.Format(emailSubject, args.EventName), "EventBroker_Emails", GetCulture(CurrentEntityAccount.PrimaryUser));

            }
            return result;
        }
        private EmailInfo HandelAddPayment(EventArgsBase args)
        {
            var result = new EmailInfo();
            IList<EntityEventData> eventData = args.Data.Select(item => item as EntityEventData).ToList();
            IList<Payment> paymentObjs =
                (from data in eventData
                 where data.Entity is Payment
                 select data.Entity as Payment).ToList();

            eventData.Select(item => item.Entity as Account).ToList();
            var paymentObj = paymentObjs.First(); //fundTransRepository.Get(fundObjs.First().ID);
            var accountObj = accountRepository.Get(paymentObj.Account.ID);
            CurrentEntityAccount = accountObj;
            var accountSummaryObj = accountObj.AccountSummary;

            var accountName = accountObj.GetDescription();
            var accountTotal = accountSummaryObj.Earning;
            var emailTemplate = string.Empty;
            if (paymentObj.VATAmount>0)
                emailTemplate = ResourceManager.Instance.GetResource("AddPaymentVAT_Account", "EventBroker_Emails", GetCulture(accountObj.PrimaryUser));
            else
                  emailTemplate = ResourceManager.Instance.GetResource("AddPayment_Account", "EventBroker_Emails", GetCulture(accountObj.PrimaryUser));
            result.Body = emailTemplate
                .Replace("@AccountName", accountName)
                .Replace("@Payment", FormatDecimal(paymentObj.Amount))
                    .Replace("@VATValue", FormatDecimal(paymentObj.VATAmount))
                         .Replace("@PaymentTotal", FormatDecimal(paymentObj.VATAmount + paymentObj.Amount))
                //.Replace("@Type", GetPaymentDetailDesc(paymentObj))
                .Replace("@Type", paymentObj.Type != null ? GelLocalizedString(accountObj.PrimaryUser, PaymentTypeRepository.Get(paymentObj.Type.ID).Name): string.Empty)
                .Replace("@Date", paymentObj.ForMonth.HasValue ? paymentObj.ForMonth.Value.ToString("dd/MM/yyy") : string.Empty)
                .Replace("@TotalEarnings", FormatDecimal(accountTotal));


            if (!(args.ExtraParameters.Count != 0 && args.ExtraParameters.Where(p => p.Value.ContainsKey("NotifyUser") && Convert.ToBoolean(p.Value["NotifyUser"]) == false).Count() != 0))
            {
                result.Address = accountObj.PrimaryUser.EmailAddress;
                result.BCC = Domain.Configuration.FinanceEmail;
            }
            else
            {
                result.Address = Domain.Configuration.FinanceEmail; 
            }

            result.Subject = ResourceManager.Instance.GetResource(string.Format(emailSubject, args.EventName), "EventBroker_Emails", GetCulture(CurrentEntityAccount.PrimaryUser));
            return result;
        }
        private EmailInfo HandelAppSiteApproval(EventArgsBase args)
        {
            var result = new EmailInfo();
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            EntityEventData eventData = null;
            foreach (var eventDataItem in eventDataList)
            {
                if (eventDataItem.Entity is AppSite)
                {
                    eventData = eventDataItem;
                    break;
                }
            }

            if (eventData == null)
            {

                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));
            }

            var appObj = appSiteRepository.Get((eventData.Entity as IEntity<int>).ID);
            CurrentEntityAccount = appObj.Account;
            var appName = appObj.GetDescription();
            var emailTemplate = string.Empty;

            if (appObj.Status.ID == (int)AppSiteStatusEnum.Active)
            {
                emailTemplate = ResourceManager.Instance.GetResource("ApproveAppSite_Account", "EventBroker_Emails");
                result.Subject = ResourceManager.Instance.GetResource("ApproveAppSiteEmailSubject", "EventBroker_Emails", GetCulture(CurrentEntityAccount.PrimaryUser));
            }
            else
            {
                emailTemplate = ResourceManager.Instance.GetResource("RejectAppSite_Account", "EventBroker_Emails");
                emailTemplate = emailTemplate.Replace("@Comments", appObj.LastAdminComment);
                result.Subject = ResourceManager.Instance.GetResource("RejectAppSiteEmailSubject", "EventBroker_Emails", GetCulture(CurrentEntityAccount.PrimaryUser));

            }

            result.Body = emailTemplate
                .Replace("@AppSiteName", appName);
            result.Address = CurrentEntityAccount.PrimaryUser.EmailAddress;

            return result;
        }
        #endregion
        #endregion
        #endregion

    }
}