
using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Domain.Common.Repositories.Core
{
 

    public class ReportSchedulerCriteria 
    {
        
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public int AccountId { get; set; }

        public int? UserId { get; set; }
        public  ReportSectionType ReportSectionType { get; set; }

    }


    public class ReportJsonCriteria 
    {

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public int AccountId { get; set; }

        public int? UserId { get; set; }
        public ReportSectionType ReportSectionType { get; set; }
    
    }
}
