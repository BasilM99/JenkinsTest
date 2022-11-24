using Noqoush.AdFalcon.Domain.Common.Model.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Noqoush.AdFalcon.Domain.Common.Model.QueryBuilder
{
    public interface ITreeQB : IEntityQB
    {
        string Name { set; get; }
        string Attribute { set; get; }
        DataTypeQB DataType { set; get; }
        int? ParentId { get; set; }
        int OrderNumber { get; set; }

    }
}