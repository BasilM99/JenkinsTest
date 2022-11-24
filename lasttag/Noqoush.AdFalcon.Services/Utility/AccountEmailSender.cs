using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NHibernate;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.Payment;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework;
using Noqoush.Framework.ConfigurationSetting;
using Noqoush.Framework.DomainServices;
using Noqoush.Framework.DomainServices.AuditTrial;
using Noqoush.Framework.DomainServices.EventBroker;
using Noqoush.Framework.DomainServices.Localization;
using Noqoush.Framework.DomainServices.Localization.Repositories;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.Persistence;
using Noqoush.Framework.Resources;
using Noqoush.Framework.Utilities.EmailsSender;
using Noqoush.AdFalcon.Domain.Repositories.Account.Payment;
using Noqoush.AdFalcon.Base;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;

namespace Noqoush.AdFalcon.Services.Utility
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
            
            _mailSender.SendEmail("", "", emailInfo.Address, emailInfo.Address, emailInfo.Subject, emailInfo.Body, true, emailInfo.BCC);
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
                    result.Body= result.Body.Replace("@CampaignName", args.ExtraParameters[0]["CampaignName"].ToString());

                }
            }

            if (!(args.ExtraParameters.Count != 0 && args.ExtraParameters.Where(p => p.ContainsKey("NotifyUser") && Convert.ToBoolean(p["NotifyUser"]) == false).Count() != 0))
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


            if (!(args.ExtraParameters.Count != 0 && args.ExtraParameters.Where(p => p.ContainsKey("NotifyUser") && Convert.ToBoolean(p["NotifyUser"]) == false).Count() != 0))
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