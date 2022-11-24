using ArabyAds.Framework.DomainServices.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.QueryBuilder
{
    public class Dimension : EntityQB
    {
        public virtual LocalizedString Name { set; get; }
        public virtual string Source { set; get; }
        public virtual string Attributes { set; get; }
        public virtual string FilterCol { set; get; }
        public virtual bool IsSql { set; get; }
        //public virtual ICollection<Column> Columns { set; get; }
        public virtual bool IsEnum { set; get; }

        public virtual string TableName { set; get; }
        public virtual string Selector { set; get; }

        public virtual bool IsGrouped { set; get; }
        public virtual string CustomGet { set; get; }

        public virtual bool IsScoped { set; get; }
        public virtual string ScopeTableName { set; get; }
        public virtual bool SupportedByAdvertiser { set; get; }

        public virtual int DimensionType { set; get; }
        public virtual bool SupportedByPublisher { set; get; }
        public virtual string DimensionTypeStr { set; get; }
    }
}
