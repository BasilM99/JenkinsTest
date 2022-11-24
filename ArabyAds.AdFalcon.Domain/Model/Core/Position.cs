using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.DomainServices;

namespace ArabyAds.AdFalcon.Domain.Model.Core
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
