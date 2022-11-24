
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using ArabyAds.AdFalcon.Domain.Repositories.Account.SSP;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using NHibernate.Linq;
using ArabyAds.AdFalcon.Domain.Model.Core;
using System.Linq;
namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
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
