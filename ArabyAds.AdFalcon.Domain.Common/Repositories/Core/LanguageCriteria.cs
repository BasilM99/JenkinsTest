using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Core
{
    

    [ProtoContract]
    public class LanguageCriteria 
    {
        [ProtoMember(1)]
        public string Value { get; set; }
        [ProtoMember(2)]
        public string Culture { get; set; }
 
    }
}
