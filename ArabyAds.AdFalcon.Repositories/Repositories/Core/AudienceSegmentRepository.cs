using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Criterion.Lambda;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    
    public class SegmentRepository : RepositoryBase<Segment, int>, ISegmentRepository
    {
        public SegmentRepository(RepositoryImplBase<Segment, int> repository)
               : base(repository)
        {


        }

    }
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

            var segment = rootQuery.SingleOrDefault<AudienceSegment>();

            return segment.Provider.Name;

        }

        public int GeMaxCode()
        {

            AudienceSegment segAlias = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<AudienceSegment, AudienceSegment> rootQuery = nhibernateSession.QueryOver<AudienceSegment>(() => segAlias);

            rootQuery.Where(M => M.IsDeleted == false);

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

    public class ContextualSegmentRepository : RepositoryBase<ContextualSegment, int>, IContextualSegmentRepository
    {
        public ContextualSegmentRepository(RepositoryImplBase<ContextualSegment, int> repository)
          : base(repository)
        {


        }



        public int GetCode(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ContextualSegment AudienceSegmentAlias = null;
            IQueryOver<ContextualSegment, ContextualSegment> rootQuery = nhibernateSession.QueryOver<ContextualSegment>(() => AudienceSegmentAlias);

            rootQuery.Where(M => M.ID == Id);
            rootQuery.Select(M => M.Code);
            return rootQuery.SingleOrDefault<int>();

        }

        public int GetSegmentIdByCode(int Code)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ContextualSegment AudienceSegmentAlias = null;
            IQueryOver<ContextualSegment, ContextualSegment> rootQuery = nhibernateSession.QueryOver<ContextualSegment>(() => AudienceSegmentAlias);

            rootQuery.Where(M => M.Code == Code);
            rootQuery.Select(M => M.ID);
            return rootQuery.SingleOrDefault<int>();

        }
        public ContextualSegment GetSegmentById(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ContextualSegment AudienceSegmentAlias = null;
            IQueryOver<ContextualSegment, ContextualSegment> rootQuery = nhibernateSession.QueryOver<ContextualSegment>(() => AudienceSegmentAlias);

            rootQuery.Where(M => M.ID == Id);

            return rootQuery.SingleOrDefault<ContextualSegment>();

        }
        public string GetSegmentDataProvider(int Id)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ContextualSegment AudienceSegmentAlias = null;
            IQueryOver<ContextualSegment, ContextualSegment> rootQuery = nhibernateSession.QueryOver<ContextualSegment>(() => AudienceSegmentAlias);

            rootQuery.Where(M => M.ID == Id);

            var segment = rootQuery.SingleOrDefault<ContextualSegment>();

            return segment.Provider.Name;

        }

        public int GeMaxCode()
        {

            ContextualSegment segAlias = null;
            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();

            IQueryOver<ContextualSegment, ContextualSegment> rootQuery = nhibernateSession.QueryOver<ContextualSegment>(() => segAlias);

            rootQuery.Where(M => M.IsDeleted == false);

            ;




            rootQuery.SelectList(L => L.SelectMax(M => M.Code));


            return rootQuery.SingleOrDefault<int>();
        }
        public ContextualSegment GetSegmentByCode(int Code)
        {

            ISession nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();
            ContextualSegment AudienceSegmentAlias = null;
            IQueryOver<ContextualSegment, ContextualSegment> rootQuery = nhibernateSession.QueryOver<ContextualSegment>(() => AudienceSegmentAlias);

            rootQuery.Where(M => M.Code == Code);

            return rootQuery.SingleOrDefault<ContextualSegment>();

        }
    }
}
