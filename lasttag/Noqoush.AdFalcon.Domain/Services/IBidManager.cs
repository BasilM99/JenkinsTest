using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Services
{
    public class BidParameter
    {
        public int[] Operators { get; set; }
        public int[] Geographies { get; set; }
        public int[] Manufacturers { get; set; }
        public int[] Platforms { get; set; }
        public int[] Keywords { get; set; }
        public int[] Models { get; set; }
        public int? Demographic { get; set; }
        public int DeviceTargetingTypeId { get; set; }
        public int ActionType { get; set; }
        public int? AdTypeId { get; set; }
        public int[] DeviceCapabilities { get; set; }
    }
    public class ReturnBid
    {
        

        public IDictionary<int, decimal> CostModelsWrappersBidValues { get; set; }

        public ReturnBid(IDictionary<int, int> costModelsWrappers)
        {
            CostModelsWrappersBidValues = new Dictionary<int, decimal>();

            foreach (var item in costModelsWrappers)
            {
                CostModelsWrappersBidValues.Add(item.Key, GetValue(item.Value));
            }
        }

       public void Add(IDictionary<int, int> values)
       {
           foreach (var item in values)
           {
               var costModelValue = CostModelsWrappersBidValues.Where(p => p.Key == item.Key).Single();
               decimal costModelBidValue = costModelValue.Value;

               costModelBidValue += GetValue(item.Value);
               CostModelsWrappersBidValues[costModelValue.Key] = costModelBidValue;
           }
           
       }

       public decimal GetValue(int value)
       {
           return (value / (decimal)100.0);
       }
    }
    public interface IBidManager
    {
        ReturnBid GetBid(BidParameter parameters);
    }
}
