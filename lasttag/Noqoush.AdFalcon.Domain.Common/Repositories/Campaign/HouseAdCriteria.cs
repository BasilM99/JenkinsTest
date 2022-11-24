using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Domain.Common.Repositories.Campaign
{
   
    public class HouseAdCriteria
    {
        public int AccountId { get; set; }
        public int? UserId { get; set; }
        public bool IsPrimaryUser { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
    
    }
}