using System;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using System.Linq;
using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;
using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Core
{
    
        public class lookalikejob 
        {
            public virtual int ID { get; set; }

            public virtual int  SeedAudienceListCode { get; set; }
            public virtual bool IsDeleted { get; set; }
            public virtual int LookalikeAudienceListCode { get; set; }
            public virtual string PopulationCountryFilter { get; set; }
            public virtual float LookalikePercentage { get; set; }
            public virtual string GetDescription()
            {
                return string.Empty;
            }
        }
}
