using NHibernate;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using NHibernate.Linq;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign
{
    public class TrackingEventRepository : RepositoryBase<TrackingEvent, int>, ITrackingEventRepository
    {
        public TrackingEventRepository(RepositoryImplBase<TrackingEvent, int> repository)
            : base(repository)
        {}

        //public override IEnumerable<TrackingEvent> Query(Expression<Func<TrackingEvent, bool>> filter)

        //{
        //    return UnitOfWork.Current.EntitySet<TrackingEvent>().Fetch(M => M.CostModelWrapper).Where(filter).ToList();
        //}

        public override IEnumerable<TrackingEvent> GetAll()
        {
            return UnitOfWork.Current.EntitySet<TrackingEvent>().WithOptions(op => op.SetCacheable(true)).ToList();
        }
    }
}
