using Noqoush.AdFalcon.Domain.Model.QueryBuilder;
using Noqoush.AdFalcon.Domain.Repositories.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Repositories.Core;

namespace Noqoush.AdFalcon.Persistence.Repositories.QueryBuilder
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
        {


        }
    }

    public class MeasureRepository : RepositoryBase<Measure, int>, IMeasureRepository
    {
        public MeasureRepository(RepositoryImplBase<Measure, int> repository)
            : base(repository)
        {


        }
    }

}
