using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Model.QueryBuilder
{
    public class Fact : EntityQB
    {
        public virtual string Name { set; get; }
        public virtual string DisplayName { set; get; }

        public virtual string WebDisplayName { set; get; }
        public virtual bool IsForWeb { set; get; }

        public virtual ICollection<Dimension> Dimensions { set; get; }
        public virtual ICollection<ColumnQB> Columns { set; get; }

        public virtual ICollection<Measure> Measures { set; get; }

    }
}
