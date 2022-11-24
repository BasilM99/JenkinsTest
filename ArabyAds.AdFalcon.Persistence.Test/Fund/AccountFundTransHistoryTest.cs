using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Testing;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework;
using System.Reflection;
using ArabyAds.Framework.Persistence.NHibernate;
using ArabyAds.Framework.Persistence;
using System.Transactions;

namespace ArabyAds.AdFalcon.Persistence.Test.Mappings.Campaign
{
    [TestFixture]
    public class AccountFundTransHistoryTest : TestBase
    {
        [FixtureSetUp]
        public override void NHibernateConfigurationSetup()
        {
            base.NHibernateConfigurationSetup();
            UnitOFWorkSetup();
        }

        [Test(Order = 1)]
        [MbUnit.Framework.Description("Success if handling exception is done and the return value is the same exception")]
        public void SaveTransHistory()
        {
            var rep = IoC.Instance.Resolve<IAccountFundTransHistoryRepository>();
          //  var typeRep = IoC.Instance.Resolve<IAccountFundTransTypeRepository>();

           // CurrentSession.Transaction.Begin();
            var history = new AccountFundTransHistory();
            history.Amount = 200;
           // history.Status = 1;
            history.AccountId = 3;
            history.CreatedById = 1;
            history.Currency =AdFalcon.Domain.Model.Core.Currency.USD;
            history.CreationDate = Framework.Utilities.Environment.GetServerTime();
            //history.Payee = "adFalcon";
            history.TransactionDate = Framework.Utilities.Environment.GetServerTime();
            history.FundTransType = new AccountFundTransType() {ID = 1};
            rep.Save(history);
            UnitOfWork.Current.Commit();
            var insertedHistory = rep.Get(history.ID);
            MbUnit.Framework.Assert.IsTrue(insertedHistory.ID == history.ID);
           // CurrentSession.Transaction.Commit();
            
        }

        [Test(Order = 2)]
        [MbUnit.Framework.Description("Success if handling exception is done and the return value is the same exception")]
        public void GetAllTransHistory()
        {
            var rep = IoC.Instance.Resolve<IAccountFundTransHistoryRepository>();
            var x = rep.GetAll();
            MbUnit.Framework.Assert.IsNotNull(x);
        }

        [Test(Order = 3)]
        public void GetAllPgws()
        {
            var rep = IoC.Instance.Resolve<IAccountFundPgwRepository>();
            var x = rep.Get(1);
            var y = x.Settings;
            MbUnit.Framework.Assert.IsNotNull(x);
        }



        [FixtureTearDown]
        public override void NHibernateTearDown()
        {
            UnitOFWorkTearDown();
            base.NHibernateTearDown();
        }
    }
}
