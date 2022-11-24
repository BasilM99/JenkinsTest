using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;

namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account.PMP
{
   
    public class PMPDealCriteria 
    {
   
        public IList<int> Geographies { get; set; }

      
        public IList<int> AdFormats { get; set; }
        public List<int> ExchangeFiltred { get; set; }
        public IList<int> AdSizes { get; set; }
        public string PublisherName { get; set; }
        public bool? Archived { get; set; }
        public int AccountId { get; set; }
        public int? PublisherId { get; set; }
        public int? AdvertiserId { get; set; }


        public int? AdvertiserAccountId { get; set; }
        public int? ExchangeId { get; set; }
        public int? userId { get; set; }
        public bool IsPrimaryUser { get; set; }

        public bool IsGlobal { get; set; }

        public bool OnlyGlobal { get; set; }

        public bool ShowAdvertiser { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DealType Type { get; set; }
        //public int? StatusId { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public int? AppSiteId { get; set; }
        public string Name { get; set; }
        public bool OnlyMyGlobal { get; set; }

        public PMPDealCriteria()
        {
          //  Type = DealType.PrivateAuction;
        }
 
    }

}
