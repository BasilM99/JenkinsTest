using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using NHibernate;
namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class AdCreativeRepository : RepositoryBase<Domain.Model.Campaign.AdCreative, int>, IAdCreativeRepository
    {
        public AdCreativeRepository(RepositoryImplBase<Domain.Model.Campaign.AdCreative, int> repository)
            : base(repository)
        {
        }

        public string GetObjectName(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<Domain.Model.Campaign.AdCreative, Domain.Model.Campaign.AdCreative> rootQuery = nhibernateSession.QueryOver<Domain.Model.Campaign.AdCreative>();

            //joins


            rootQuery.Where(
                        p => p.ID == Id);


            return rootQuery.Select(M => M.Name).SingleOrDefault<string>();


        }
    }



    public class AdExtensionRepository : RepositoryBase<Domain.Model.Campaign.AdExtension, int>, IAdExtensionRepository
    {
        public AdExtensionRepository(RepositoryImplBase<Domain.Model.Campaign.AdExtension, int> repository)
            : base(repository)
        {
        }


    }


}
