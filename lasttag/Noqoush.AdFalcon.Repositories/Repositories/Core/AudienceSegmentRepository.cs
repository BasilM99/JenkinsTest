using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Criterion.Lambda;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{

    public class AudienceSegmentRepository : RepositoryBase<AudienceSegment, int>, IAudienceSegmentRepository
    {
        public AudienceSegmentRepository(RepositoryImplBase<AudienceSegment, int> repository)
            : base(repository)
        {


        }


        public int GetCode(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            AudienceSegment AudienceSegmentAlias = null;
            IQueryOver<AudienceSegment, AudienceSegment> rootQuery = nhibernateSession.QueryOver<AudienceSegment>(() => AudienceSegmentAlias);

            rootQuery.Where(M => M.ID == Id);
            rootQuery.Select(M => M.Code);
            return rootQuery.SingleOrDefault<int>();

        }

        public int GetSegmentIdByCode(int Code)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            AudienceSegment AudienceSegmentAlias = null;
            IQueryOver<AudienceSegment, AudienceSegment> rootQuery = nhibernateSession.QueryOver<AudienceSegment>(() => AudienceSegmentAlias);

            rootQuery.Where(M => M.Code == Code);
            rootQuery.Select(M => M.ID);
            return rootQuery.SingleOrDefault<int>();

        }
        public AudienceSegment GetSegmentById(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            AudienceSegment AudienceSegmentAlias = null;
            IQueryOver<AudienceSegment, AudienceSegment> rootQuery = nhibernateSession.QueryOver<AudienceSegment>(() => AudienceSegmentAlias);

            rootQuery.Where(M => M.ID == Id);

            return rootQuery.SingleOrDefault<AudienceSegment>();

        }
        public string GetSegmentDataProvider(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            AudienceSegment AudienceSegmentAlias = null;
            IQueryOver<AudienceSegment, AudienceSegment> rootQuery = nhibernateSession.QueryOver<AudienceSegment>(() => AudienceSegmentAlias);

            rootQuery.Where(M => M.ID == Id);

            var segment= rootQuery.SingleOrDefault<AudienceSegment>();
            
            return segment.Provider.Name;

        }

        public int  GeMaxCode()
        {

            AudienceSegment segAlias = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AudienceSegment, AudienceSegment> rootQuery = nhibernateSession.QueryOver<AudienceSegment>(() => segAlias);

            rootQuery.Where(M => M.IsDeleted==false);
     
;




            rootQuery.SelectList(L => L.SelectMax(M => M.Code));


            return rootQuery.SingleOrDefault<int>();
        }
        public AudienceSegment GetSegmentByCode(int Code)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            AudienceSegment AudienceSegmentAlias = null;
            IQueryOver<AudienceSegment, AudienceSegment> rootQuery = nhibernateSession.QueryOver<AudienceSegment>(() => AudienceSegmentAlias);

            rootQuery.Where(M => M.Code == Code);
           
            return rootQuery.SingleOrDefault<AudienceSegment>();

        }
    }
}
