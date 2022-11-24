
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;
using Noqoush.AdFalcon.Domain.Repositories.Account.SSP;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;
using NHibernate.Linq;
using Noqoush.AdFalcon.Domain.Model.Core;
using System.Linq;
namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class CountryRepository : RepositoryBase<Country, int>, ICountryRepository
    {
        public CountryRepository(RepositoryImplBase<Country, int> repository)
            : base(repository)
        {


        }
    }
    public class CountryVATRepository : RepositoryBase<CountryVAT, int>, ICountryVATRepository
    {
        public CountryVATRepository(RepositoryImplBase<CountryVAT, int> repository)
            : base(repository)
        {


        }


        public decimal GetVATValueByCountryID(int id)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            CountryVAT countryVATAlias = null;
            IQueryOver<CountryVAT, CountryVAT> rootQuery = nhibernateSession.QueryOver<CountryVAT>(() => countryVATAlias);


            rootQuery.Where(M => M.Country.ID == id);

            var countryVAT= rootQuery.SingleOrDefault();

            if (countryVAT!=null )
            {

                return countryVAT.VATValue;
            }

            return 0;

        }

        public string GetTaxRegistrationExpressionByCountryID(int id)
        {
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            CountryVAT countryVATAlias = null;
            IQueryOver<CountryVAT, CountryVAT> rootQuery = nhibernateSession.QueryOver<CountryVAT>(() => countryVATAlias);


            rootQuery.Where(M => M.Country.ID == id);

            var countryVAT = rootQuery.SingleOrDefault();

            if (countryVAT != null)
            {

                return countryVAT.TaxNoRegistrationExpression;
            }

            return string.Empty;

        }
    }


}
