using ArabyAds.AdFalcon.Domain.Model.QueryBuilder;
using ArabyAds.AdFalcon.Domain.Repositories.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using System.Linq.Expressions;
using NHibernate.Linq;

namespace ArabyAds.AdFalcon.Persistence.Repositories.QueryBuilder
{
   
    public class FactRepository : RepositoryBase<Fact, int>, IFactRepository
    {
        public FactRepository(RepositoryImplBase<Fact, int> repository)
            : base(repository)
        {


        }
    }

    public class DimensionRepository : RepositoryBase<Dimension, int>, IDimensionRepository
    {
        public DimensionRepository(RepositoryImplBase<Dimension, int> repository)
            : base(repository)
        {


        }
    }


    public class ColumnQBRepository : RepositoryBase<ColumnQB, int>, IColumnQBRepository
    {
        public ColumnQBRepository(RepositoryImplBase<ColumnQB, int> repository)
            : base(repository)
        {}

        public override IEnumerable<ColumnQB> Query(Expression<Func<ColumnQB, bool>> filter)
        {
            return UnitOfWork.Current.EntitySet<ColumnQB>().WithOptions(op => op.SetCacheable(true)).Where(filter).ToList();
        }
    }

    public class MeasureRepository : RepositoryBase<Measure, int>, IMeasureRepository
    {
        public MeasureRepository(RepositoryImplBase<Measure, int> repository)
            : base(repository) { }

        public override IEnumerable<Measure> Query(Expression<Func<Measure, bool>> filter)
        {
            return UnitOfWork.Current.EntitySet<Measure>().WithOptions(op => op.SetCacheable(true)).Where(filter).ToList();
        }

    }

}
