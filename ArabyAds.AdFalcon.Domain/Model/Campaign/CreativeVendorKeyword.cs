﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class CreativeVendorKeyword : IEntity<int>
    {
        public virtual string GetDescription()
        {
            return this.Keyword;
        }
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string Keyword { get; set; }
        public virtual CreativeVendor Vendor { get; set; }
    }
}