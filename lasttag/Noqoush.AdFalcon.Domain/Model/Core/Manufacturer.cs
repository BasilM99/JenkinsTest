﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;

namespace Noqoush.AdFalcon.Domain.Model.Core
{
    public class Manufacturer : ManagedLookupBase//LookupBase<Manufacturer, int>
    {
        public virtual int Order
        {
            get;
            set;
        }
        public virtual IEnumerable<Device> Devices
        {
            get;
            set;
        }
    }
}
