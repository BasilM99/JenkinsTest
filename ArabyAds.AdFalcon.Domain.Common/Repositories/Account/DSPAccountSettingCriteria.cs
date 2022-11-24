using ArabyAds.AdFalcon.Domain.Common.Model.Account;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Account
{
   

    public class DSPAccountSettingCriteria 
    {

        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public int AccountId { get; set; }
   
    }
}
