using Noqoush.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting
{
    public class AdRequestType : LookupBase<AdRequestType, int>
    {
        public virtual string Code { get; set; }
    }


}
