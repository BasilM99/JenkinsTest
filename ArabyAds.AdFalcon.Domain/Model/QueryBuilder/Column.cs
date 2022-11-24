using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.QueryBuilder
{
    public class ColumnQB : TreeQB
    {
        public virtual string Source { set; get; }

        public virtual string FkSelector { set; get; }
        public virtual string TableName { set; get; }
        public virtual bool IsSql { set; get; }
        public virtual bool IsDuplicated { set; get; }
        public virtual string Alias { set; get; }
        public virtual string homeIdSelector { set; get; }
        public virtual string formatSQL { set; get; }
        public virtual bool SupportedByAdvertiser { set; get; }
        public virtual bool SupportedByPublisher { set; get; }
        public virtual int minWidth { set; get; }

        
    }
}
