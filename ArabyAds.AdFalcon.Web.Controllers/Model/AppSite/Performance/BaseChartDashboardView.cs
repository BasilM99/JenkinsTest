using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Web.Controllers.Model.AppSite.Performance
{
    public class BaseChartDashboardView
    {
        public string Name { get; set; }

        public object MetricValue { get; set; }

        public string MetricValueText
        {
            get
            {
                if (MetricValue is decimal)
                {
                    return string.Format("{0:#,###0.00}", decimal.Parse(MetricValue.ToString()));
                }
                else
                {
                    return string.Format("{0:#,###0}", long.Parse(MetricValue.ToString()));
                }
            }
        }
    }
}
