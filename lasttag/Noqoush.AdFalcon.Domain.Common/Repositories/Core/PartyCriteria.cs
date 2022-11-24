using System;

using Noqoush.AdFalcon.Domain.Common.Model.Core;

using System.Collections;
using System.Collections.Generic;

using System.Linq.Expressions;
namespace Noqoush.AdFalcon.Domain.Common.Repositories.Core
{
    public class PartyCriteria 
    {
        public List<int> notInclud { get; set; }
        public PartyType? Type { get; set; }
        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public string Code { get; set; }
        public bool Visible { get; set; }
        public bool ShowArchive { get; set; }

    }
    public class DPPartnerCriteria 
    {
        public List<int> notInclud { get; set; }
        public PartyType? Type { get; set; }
        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public string Code { get; set; }
        public bool Visible { get; set; }

  

    }

}
