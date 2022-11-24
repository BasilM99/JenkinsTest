﻿using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Objective
{
   
        public class AdActionCostModelWrapper : IEntity<int>
        {
        public virtual bool IsDeleted { get; set; }
        public virtual AppScope Scope { get; set; }
        public virtual int ID { get; protected set; }
        public virtual CostModelWrapper CostModelWrapper { get; set; }
        public virtual AdActionTypeBase AdAction { get; set; }
        public virtual string GetDescription()
        {
            return AdAction.GetDescription();
        }
    }
}
