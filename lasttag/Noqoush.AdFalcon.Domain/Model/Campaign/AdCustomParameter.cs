using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class AdCustomParameter : IEntity<int>
    {
        public virtual int ID { get; protected set; }
        public virtual AdCreative AdCreative { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
        public virtual bool IsMandatory { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual string GetDescription()
        {
            return this.Name;
        }

       
    }
}
