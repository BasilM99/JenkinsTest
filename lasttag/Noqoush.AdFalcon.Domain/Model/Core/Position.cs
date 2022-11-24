using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Core
{
  /* public class Position:IEntity<int>
    {
       public virtual string Name { get; set; }
       public virtual int ID { get; }
       public virtual string GetDescription() { throw new NotImplementedException(); }
       public virtual bool IsDeleted{get; set; }
    }*/
    public class Position : LookupBase<Position, int>
    {

    }
}
