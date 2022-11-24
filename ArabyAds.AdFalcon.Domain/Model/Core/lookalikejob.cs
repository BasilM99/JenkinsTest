using System;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using System.Linq;
using System.Collections.Generic;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using ArabyAds.Framework.DomainServices;

namespace ArabyAds.AdFalcon.Domain.Model.Core
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
