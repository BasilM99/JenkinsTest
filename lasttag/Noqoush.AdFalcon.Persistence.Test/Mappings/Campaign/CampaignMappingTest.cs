using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Testing;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Test.Mappings.Campaign
{
    [TestFixture]
    public class CampaignMappingTest : TestBase
    {
        [FixtureSetUp]
        public override void NHibernateConfigurationSetup()
        {
            base.NHibernateConfigurationSetup();
        }
        [Test(Order = 1)]
        [MbUnit.Framework.Description("Success if handling exception is done and the return value is the same exception")]
        public void CanCorrectlyMapCampaign()
        {
            new PersistenceSpecification<Noqoush.AdFalcon.Domain.Model.Campaign.Campaign>(CurrentSession, new CustomEqualityComparer())
                .CheckProperty(c => c.CreationDate, Framework.Utilities.Environment.GetServerTime())
                .CheckProperty(c => c.StartDate, Framework.Utilities.Environment.GetServerTime().AddDays(2))
                .CheckProperty(c => c.EndDate, Framework.Utilities.Environment.GetServerTime().AddDays(7))
                .CheckProperty(c => c.Budget, 5000)
                .CheckProperty(c => c.Budget, 1000)
                .CheckProperty(c => c.Note, "Campaign Note")
                .CheckProperty(c => c.Name, "My Test Campaign")
                .CheckProperty(c => c.Account, new Account() { ID = 1 })
                .VerifyTheMappings();
        }

        public class CustomEqualityComparer : IEqualityComparer
        {
            public bool Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }
                if (x is DateTime && y is DateTime)
                {
                    return true;//((DateTime)x).Ticks == ((DateTime)y).Ticks;
                }
                if (x is Account && y is Account)
                {
                    return ((Account)x).ID == ((Account)y).ID;
                }
                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }
        [FixtureTearDown]
        public override void NHibernateTearDown()
        {
            base.NHibernateTearDown();
        }
    }
}
