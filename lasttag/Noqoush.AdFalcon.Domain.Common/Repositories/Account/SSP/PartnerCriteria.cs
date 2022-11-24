using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;

using System.Linq;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Account.SSP;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP
{


    public class PartnerCriteria 
    {
        public string PartnerName { get; set; }
        public int ZoneId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public int TypeId { get; set; }
 
    }
}
