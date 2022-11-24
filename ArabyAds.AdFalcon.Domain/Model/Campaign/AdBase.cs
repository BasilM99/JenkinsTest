using System;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class AdBase<TEntity, TStatus> : IEntity<int>
        where TEntity : class, new()
        where TStatus : class, new()
    {
        public virtual int ID { get;  set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string Name
        {
            get;
            set;
        }
        public virtual TStatus Status{get { return null; } set{}}
        public virtual DateTime CreationDate
        {
            get;
            set;
        }
        public virtual bool ChangeStatus(TStatus status)
        {
            throw new System.NotImplementedException();
        }
        public virtual bool Pause()
        {
            return true;
        }
        public virtual bool Stop()
        {
            throw new System.NotImplementedException();
        }
        public virtual bool Resume()
        {
            return true;
        }
        public virtual bool Delete()
        {
            this.IsDeleted = true;
            return true;
        }

        public virtual TEntity Clone()
        {
            return null;
        }

        public virtual string GetDescription()
        {
            return ToString();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

