using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.Account.DPP;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;


namespace Noqoush.AdFalcon.Domain.Common.Repositories.Campaign
{
    public class ImpressionLogCriteria 
    {
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public int DataFromInt { get; set; }
        public int DataToInt { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }

        public ImpressionLogType Type { get; set; }
        public string Name { get; set; }
        public int? DataProviderId { get; set; }

     
    
    }

}
