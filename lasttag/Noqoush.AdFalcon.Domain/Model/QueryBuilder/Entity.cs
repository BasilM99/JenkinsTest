using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.QueryBuilder
{
    public class EntityQB : IEntityQB
    {
        public virtual int Id { get; set; }
        public virtual bool IsDeleted { get; set; }

    }
}
