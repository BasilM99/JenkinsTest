using ArabyAds.AdFalcon.Domain.Common.Model.QueryBuilder;
using ArabyAds.AdFalcon.Domain.Model.QueryBuilder;
using ArabyAds.Framework.DomainServices.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArabyAds.AdFalcon.Domain.Model.QueryBuilder
{
    public class TreeQB : EntityQB, ITreeQB
    {
        public virtual string Name { set; get; }
        public virtual LocalizedString DisplayName { set; get; }

        public virtual string Attribute { set; get; }
        public virtual DataTypeQB DataType { set; get; }
        public virtual int? ParentId { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual bool IsHidden { get; set; }

        public virtual string SubstituteAttribute { set; get; }

        public virtual string RawAttribute { set; get; }
        public virtual string requestsmapping { set; get; }
        public virtual string dealsrequestsmapping { set; get; }

    }
}