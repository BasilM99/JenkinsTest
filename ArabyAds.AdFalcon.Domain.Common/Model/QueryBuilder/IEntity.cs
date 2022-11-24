using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Model.QueryBuilder
{
    public interface IEntityQB
    {
        int Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
