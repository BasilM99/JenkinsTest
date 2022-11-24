using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
    public class LookupCriteriaBase : CriteriaBase<ManagedLookupBase>
    {
        public string LookType { get; set; }

        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Core.LookupCriteriaBase Commoncr)
        {
            LookType = Commoncr.LookType;
        
        }
        public override Expression<Func<ManagedLookupBase, bool>> GetExpression()
        {
            Expression<Func<ManagedLookupBase, bool>> filter = (c => c.IsDeleted == false);
            return filter;
        }

        public override Func<ManagedLookupBase, bool> GetWhere()
        {
            Func<ManagedLookupBase, bool> filter = (c => c.IsDeleted == false);
            return filter;
        }
    }
    public class LookupGetCriteria : LookupCriteriaBase
    {
        public int Id { get; set; }

        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Core.LookupGetCriteria Commoncr)
        {
            Id = Commoncr.Id;
            LookType = Commoncr.LookType;
        }
        
        public override Expression<Func<ManagedLookupBase, bool>> GetExpression()
        {
            Expression<Func<ManagedLookupBase, bool>> filter = (c => c.IsDeleted == false);
            return filter;
        }

        public override Func<ManagedLookupBase, bool> GetWhere()
        {
            Func<ManagedLookupBase, bool> filter = (c => c.IsDeleted == false);
            return filter;
        }
    }

    [KnownType(typeof(DeviceLookupCriteria))]
    public class LookupCriteria : LookupGetCriteria
    {
        public string Name { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }

        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Core.LookupCriteria Commoncr)
        {
            Id = Commoncr.Id;
            LookType = Commoncr.LookType;


            Name = Commoncr.Name;
            Page = Commoncr.Page;
            Size = Commoncr.Size;


        }

        public override Expression<Func<ManagedLookupBase, bool>> GetExpression()
        {
            Expression<Func<ManagedLookupBase, bool>> filter = (c => c.IsDeleted == false && c.Name.Values.Any(v => v.Value.Contains(Name)));
            return filter;
        }

        public Expression<Func<TEntity, bool>> GetLookupExpression<TEntity>()
            where TEntity : ManagedLookupBase, new()
        {
            Expression<Func<TEntity, bool>> filter = (c => c.Name.Values.Any(v => v.Value.Contains(Name)));
            return filter;
        }
        public override Func<ManagedLookupBase, bool> GetWhere()
        {
            Func<ManagedLookupBase, bool> filter = (c => c.IsDeleted == false && c.Name.Values.Any(v => v.Value.Contains(Name)));
            return filter;
        }
    }

    public class DeviceLookupCriteria : LookupCriteria
    {
        public int? ManufacturerId { get; set; }
        public int? PlatformId { get; set; }



        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Core.DeviceLookupCriteria Commoncr)
        {
            Id = Commoncr.Id;
            LookType = Commoncr.LookType;


            Name = Commoncr.Name;
            Page = Commoncr.Page;
            Size = Commoncr.Size;
            PlatformId = Commoncr.PlatformId;
            ManufacturerId = Commoncr.ManufacturerId;



        }
        public Expression<Func<Device, bool>> GetDeviceExpression()
        {

            Expression<Func<Device, bool>> filter = (
                x =>
                    (!ManufacturerId.HasValue || x.Manufacturer.ID == ManufacturerId) &&
                    (!PlatformId.HasValue || x.Platform.ID == PlatformId) &&
                    (x.Name.Values.Any(v => v.Value.Contains(Name))));
            return filter;
        }

        public override Expression<Func<ManagedLookupBase, bool>> GetExpression()
        {

            Expression<Func<ManagedLookupBase, bool>> filter = (c => (c as Device).IsDeleted == false &&
                                                                (c as Device).Name.Value.Contains(Name) &&
                                                                (c as Device).Manufacturer.ID == ManufacturerId &&
                                                                 (c as Device).Platform.ID == PlatformId);
            return filter;
        }

        public override Func<ManagedLookupBase, bool> GetWhere()
        {
            Func<ManagedLookupBase, bool> filter = (c => c is Device && (c as Device).IsDeleted == false &&
                                                                (c as Device).Name.Value.Contains(Name) &&
                                                                (c as Device).Manufacturer.ID == ManufacturerId &&
                                                                 (c as Device).Platform.ID == PlatformId);
            return filter;
        }
    }
}
