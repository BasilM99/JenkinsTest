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
    public class AdGroupCriteria 
    {

        //   private IAccountAdPermissionsRepository AdPermissionsRepository = IoC.Instance.Resolve<IAccountAdPermissionsRepository>();

        public int CampaignId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public int AccountId { get; set; }

        public int? AppSiteId { get; set; }

        public List<int> Permissions
        {
            get;
            set;
        }

      
    }
}
