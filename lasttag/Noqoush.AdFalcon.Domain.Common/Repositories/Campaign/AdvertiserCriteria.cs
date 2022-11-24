using Noqoush.AdFalcon.Domain.Common.Model.Campaign;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Repositories.Campaign
{
   
    public class AdvertiserCriteria 
    {
        public string Value { get; set; }
        public string Culture { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }

        
    }




    public class AdvertiserAccountCriteria 
    {
        public bool IsReadOnly  { get; set; }
        public int AccountId { get; set; }
        public string culture { get; set; }
        public int? userId { get; set; }
        public bool showActive { get; set; }
        public bool showArchived { get; set; }
        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
    
        public string Name { get; set; }
      
    }
}
