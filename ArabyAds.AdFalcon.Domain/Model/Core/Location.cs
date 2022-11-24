﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    //public enum LocationType
    //{
    //    Continent = 1,
    //    Country = 2,
    //    State = 3,
    //    City = 4,
    //}

    public class LocationBase : ManagedLookupBase//LookupBase<LocationBase, int>
    {
        public virtual int Type { get; set; }
        public virtual LocationBase Parent
        {
            get;
            set;
        }

        public virtual IEnumerable<LocationBase> Locations
        {
            get;
            set;
        }

        public virtual string TwoLettersCode { get; set; }

        public virtual string ThreeLettersCode { get; set; }

        public virtual string MobileCountryCode { get; set; }

        public virtual List<string> MobileCountryCodeList
        {
            get
            {
                if (!string.IsNullOrEmpty(MobileCountryCode))
                {
                    MobileCountryCode.Split(',').ToList();
                }

                return new List<string>();
            }
        }

        public virtual string CodeAlpha2 { get; set; }
       
    }
    public class Location<TLocation, TParent, TChild> : LocationBase
        where TLocation : class
        where TParent : class
        where TChild : class
    {
       

    }
}
