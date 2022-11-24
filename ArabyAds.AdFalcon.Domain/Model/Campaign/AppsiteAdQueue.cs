﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Exceptions;
using ArabyAds.AdFalcon.Exceptions.Core;
using ArabyAds.Framework.DataAnnotations;
using System.Linq;
using ArabyAds.Framework.DomainServices;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    //[DataContract()]
    //public enum Include
    //{
    //    [EnumMember]
    //    [EnumText("Include", "Global")]
    //    Include = 1,
    //    [EnumMember]
    //    [EnumText("Exclude", "Global")]
    //    Exclude = 0
    //}
    public class AppSiteAdQueue : IEntity<int>
    {
        public virtual AdCreative Ad { get; set; }
        public virtual AppSite.AppSite AppSite { get; set; }
        public virtual decimal Bid { get; set; }
        public virtual int ID { get; protected set; }
        public virtual bool IsDeleted { get; set; }
        public virtual Include Include { get; set; }
        public virtual string GetDescription()
        {
            return string.Format("{0}:{1}",AppSite.Name, Ad.Name);
        }
        public virtual AppSiteAdQueue Copy()
        {
            var cloneObj = new AppSiteAdQueue();
            cloneObj.Ad = this.Ad;
            cloneObj.AppSite = this.AppSite;
            cloneObj.Bid = this.Bid;
            cloneObj.IsDeleted = this.IsDeleted;
            return cloneObj;
        }

        //public override bool Equals(object obj)
        //{
        //    var other = obj as AppSiteAdQueue;

        //    if (ReferenceEquals(null, other)) return false;
        //    if (ReferenceEquals(this, other)) return true;
        //    return other.ID == ID && other.AppSite.ID == AppSite.ID && other.Ad.ID== Ad.ID;
        //}

        //public override int GetHashCode()
        //{
        //    unchecked
        //    {
        //        int hash = ID;
        //        // Suitable nullity checks etc, of course :)
        //        hash = hash * 23 + AppSite.GetHashCode();
        //        hash = hash * 23 + Ad.GetHashCode();
               
        //        return hash;
        //    }
        //}
    }
}

