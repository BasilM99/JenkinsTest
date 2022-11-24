using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;


namespace Noqoush.AdFalcon.Domain.Common.Repositories
{
    public class AppSiteCriteriadp
    {
        public int AccountId { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public int? Type { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
 
    }
}
