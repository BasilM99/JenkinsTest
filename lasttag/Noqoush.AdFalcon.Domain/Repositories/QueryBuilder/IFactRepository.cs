using Noqoush.AdFalcon.Domain.Model.QueryBuilder;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Noqoush.AdFalcon.Domain.Repositories.QueryBuilder
{
   

    public interface IFactRepository : IKeyedRepository<Fact, int>
    {
    }
    public interface IDimensionRepository : IKeyedRepository<Dimension, int>
    {
    }

    public interface IColumnQBRepository : IKeyedRepository<ColumnQB, int>
    {
    }

    public interface IMeasureRepository : IKeyedRepository<Measure, int>
    {
    }

  
}
