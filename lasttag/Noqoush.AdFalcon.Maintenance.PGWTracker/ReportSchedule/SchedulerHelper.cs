using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.Framework;
using Noqoush.Framework.ConfigurationSetting;
using Noqoush.Framework.Utilities.EmailsSender;
using Noqoush.Framework.Resources;

using Quartz;
using Quartz.Impl;
using System.Globalization;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund;
using Noqoush.AdFalcon.AdServer.Integration.Services.Interfaces;

namespace Noqoush.AdFalcon.AdFalconPortalMaintenanceJob.ReportSchedule
{
  

    public static class SchedulerHelper
    {

        public static IEmailHandlerService EmailHandlerService;
        public static IMessagesEventBrokerService MessagesEventBrokerService;


        public static IReportService ReportService;
        public static IAccountService AccountService;
        public static IFundTransactionService FundTransactionService;
        public static IMailSender MailSender;
        public static IResourceService ResourceService;
       public static string DefaultLanguage;
        public static bool Initialized = false;
        public static IConfigurationSettingService ConfigurationManager;
        public static CultureInfo GetCulture()
        {
            string result = "en-US";
            switch (LanguageCode)
            {
                case "ar":
                    {
                        result = "ar-JO";
                    
                        break;
                    }
                case "en":
                    {
                        result = "en-US";
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(DefaultLanguage))
            {

                result = DefaultLanguage;
            }
           return new CultureInfo(result);
        }
        public static string  GetCultureStr()
        {
            string result = "en-US";
            switch (LanguageCode)
            {
                case "ar":
                    {
                        result = "ar-JO";

                        break;
                    }
                case "en":
                    {
                        result = "en-US";
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(DefaultLanguage))
            {

                result = DefaultLanguage;
            }
            return (result);
        }
        public static string LanguageCode;
        public static void InitializeSchedulerHelper()
        {
            ReportService = IoC.Instance.Resolve<IReportService>();

            FundTransactionService = IoC.Instance.Resolve<IFundTransactionService>();

            EmailHandlerService = IoC.Instance.Resolve<IEmailHandlerService>();
            MessagesEventBrokerService = IoC.Instance.Resolve<IMessagesEventBrokerService>();


            AccountService = IoC.Instance.Resolve<IAccountService>();
            MailSender = IoC.Instance.Resolve<IMailSender>();
            ResourceService = IoC.Instance.Resolve<Noqoush.Framework.Resources.IResourceService>();
            ConfigurationManager = Framework.IoC.Instance.Resolve<Noqoush.Framework.ConfigurationSetting.IConfigurationSettingService>();
            SchedulerFactory = new StdSchedulerFactory();
            DefaultLanguage = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"];
            Scheduler = SchedulerFactory.GetScheduler();
            Initialized = true;

        }
        static SchedulerHelper()
        {
            try
            {
                //incase not up
                InitializeSchedulerHelper();
                Initialized = true;
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
            }
        }
        public static DateTimeOffset? GetNextFireTime(IJobExecutionContext context)
        {
            var triggers = context.Scheduler.GetTriggersOfJob(context.JobDetail.Key);
            if (triggers != null)
            {
                var listUTC = triggers.Select(x => x.GetFireTimeAfter(Framework.Utilities.Environment.GetServerTime())).ToList();
                if (listUTC != null)
                {
                    var listrstl = listUTC.Where(X => X.HasValue).ToList();
                    if (listrstl != null)
                    {
                        var listVal = listrstl.Where(X => X.Value >= Framework.Utilities.Environment.GetServerTime()).ToList();
                        if (listVal != null)
                        {
                            return listVal.Min();
                        }
                    }
                }
            }
            return null;
        }
             public static ISchedulerFactory SchedulerFactory
        {

            get;
            set;

        }
        public static IScheduler Scheduler
        {

            get;
            set;

        }
        public static DateTimeOffset? GetNextFireTime(JobKey jobKey )
        {
            var triggers = Scheduler.GetTriggersOfJob(jobKey);
            if (triggers != null)
            {
                var listUTC = triggers.Select(x => x.GetFireTimeAfter(Framework.Utilities.Environment.GetServerTime())).ToList();
                if (listUTC != null)
                {
                    var listrstl = listUTC.Where(X => X.HasValue).ToList();
                    if (listrstl != null)
                    {
                        var listVal = listrstl.Where(X => X.Value >= Framework.Utilities.Environment.GetServerTime()).ToList();
                        if (listVal != null)
                        {
                            return listVal.Min();
                        }
                    }
                }
            }
            return null;

        }
     
}
}
