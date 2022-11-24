using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;

using System.Linq;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Account.SSP;
namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account.SSP
{


    public class DealCampaignMappingCriteria 
    {
        public string  CampaignName { get; set; }
        public int PartnerId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public string DealIdName { get; set; }
        public int? SiteId { get; set; }
        
    }
}
