using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    public class metriceColumnReportCriteria :  IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual metriceColumn metriceColumn { get; set; }
        public virtual ReportCriteria ReportCriteria { get; set; }
        public virtual bool IsDeleted { get; set; }
   
        public virtual string GetDescription()
        {
            return ToString();
        }
    }
}
