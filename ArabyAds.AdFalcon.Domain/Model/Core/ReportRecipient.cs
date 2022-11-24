﻿using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    public class ReportRecipient : IEntity<int>
    {
        public virtual int ID { get; protected set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string Email { get; set; }

        public virtual ReportScheduler ReportScheduler { get; set; }

        public virtual string GetDescription()
        {
            return this.Email;
        }
    }

}
