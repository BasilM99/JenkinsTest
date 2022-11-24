using Noqoush.Framework.Persistence;
using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
 

    public class ReportSchedulerCriteria : CriteriaBase<Model.Core.ReportScheduler>
    {
        
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public int AccountId { get; set; }

        public int? UserId { get; set; }

        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Core.ReportSchedulerCriteria  Commoncr)
        {
            DateFrom= Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;
            Page = Commoncr.Page;
            Size = Commoncr.Size;
            AccountId = Commoncr.AccountId;
            UserId = Commoncr.UserId;
            ReportSectionType = Commoncr.ReportSectionType;
        }
        public  ReportSectionType ReportSectionType { get; set; }
        public override Expression<Func<Model.Core.ReportScheduler, bool>> GetExpression()
        {
            
            Expression<Func<Model.Core.ReportScheduler, bool>> filter = (c => c.IsDeleted == false
                  && (c.ReportSectionType == ReportSectionType)
                  && (c.Account.ID == AccountId) /*&& (!UserId.HasValue || c.User.ID == UserId)*/
                   && (!DateFrom.HasValue || c.CreationDate >= DateFrom) && (!DateTo.HasValue || c.CreationDate <= DateTo));
            return filter;
        }
        public override Func<Model.Core.ReportScheduler, bool> GetWhere()
        {
            //Func<Model.Campaign.AdGroup, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId));
            Func<Model.Core.ReportScheduler, bool> filter = (c => c.IsDeleted == false);
            return filter;
        }
    }


    public class ReportJsonCriteria : CriteriaBase<Model.Core.ReportCriteria>
    {

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public int AccountId { get; set; }

        public int? UserId { get; set; }
        public ReportSectionType ReportSectionType { get; set; }



        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Core.ReportJsonCriteria Commoncr)
        {
            DateFrom = Commoncr.DateFrom;
            DateTo = Commoncr.DateTo;
            Page = Commoncr.Page;
            Size = Commoncr.Size;
            AccountId = Commoncr.AccountId;
            UserId = Commoncr.UserId;
            ReportSectionType = Commoncr.ReportSectionType;
        }
        public override Expression<Func<Model.Core.ReportCriteria, bool>> GetExpression()
        {

            Expression<Func<Model.Core.ReportCriteria, bool>> filter = (c => c.IsDeleted == false
                  && (c.SectionType == ReportSectionType)
                  && (c.Account.ID == AccountId) /*&& (!UserId.HasValue || c.User.ID == UserId)*/
                   && (!DateFrom.HasValue || c.CreationDate >= DateFrom) && (!DateTo.HasValue || c.CreationDate <= DateTo));
            return filter;
        }
        public override Func<Model.Core.ReportCriteria, bool> GetWhere()
        {
            //Func<Model.Campaign.AdGroup, bool> filter = (c => c.IsDeleted == false && (!StatusId.HasValue || c.Status.ID == StatusId));
            Func<Model.Core.ReportCriteria, bool> filter = (c => c.IsDeleted == false);
            return filter;
        }
    }
}
