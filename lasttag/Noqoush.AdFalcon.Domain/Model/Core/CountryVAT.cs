using System;
using System.Collections.Generic;
using System.Net;
using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Core
{


    public class CountryVAT : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual decimal VATValue { get; set; }
        public virtual Country Country { get; set; }
        public virtual string GetDescription()
        {
            return Country.GetDescription() + VATValue;
        }
        public virtual bool IsDeleted {get;set;}


        public virtual string TaxNoRegistrationExpression { get; set; }
    }



}