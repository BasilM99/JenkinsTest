using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;

using System.Linq;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

using Noqoush.AdFalcon.Domain.Common.Model.Account;


namespace Noqoush.AdFalcon.Domain.Common.Repositories.Campaign
{
    public class AdvertiserAccountMasterAppSiteCriteria 
    {

        public int? AccountId { get; set; }
        public string culture { get; set; }
        public MasterAppSiteStatus  Status { get; set; }
        public MasterAppSiteType Type { get; set; }

        public int? userId { get; set; }
        public bool showActive { get; set; }
        public bool showArchived { get; set; }
        public bool showGlobalAndAccount { get; set; }
        public bool showAccountAndAdvertiser { get; set; }
        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public bool? GlobalScope { get; set; }
        public string Name { get; set; }
        public int? AdvAccountId { get; set; }
  
    }





    public class AudienceSegmentCriteria 
    {
        public string Value { get; set; }
        public string Culture { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }
        public int AdvAccountId { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public bool showArchived { get; set; }

      
    }
    public class AdvertiserAccountMasterAppSiteItemCriteria 
    {
        public int? AccountId { get; set; }
        public string culture { get; set; }
        public int? StatusId { get; set; }
        public int? userId { get; set; }
        public MasterAppSiteItemType Type { get; set; }
        public bool showActive { get; set; }
        public bool showArchived { get; set; }
        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }

        public string Name { get; set; }
        public string BundleId { get; set; }
        public string AppSiteId { get; set; }
        public string Domain { get; set; }
        public int MasterListId { get; set; }
   
    }


    public class PixelCriteria 
    {

        public int? AccountId { get; set; }
        public string culture { get; set; }
        public PixelStatus Status { get; set; }
        public MasterAppSiteType Type { get; set; }

        public int? userId { get; set; }
        public bool showActive { get; set; }
        public bool showArchived { get; set; }
        public bool showGlobalAndAccount { get; set; }
        public bool showAccountAndAdvertiser { get; set; }
        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public bool? GlobalScope { get; set; }
        public string Name { get; set; }

        public string Value { get; set; }
        public int? AdvAccountId { get; set; }
     
    }
}
