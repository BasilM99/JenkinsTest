using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Report
{
    public class ColumnViewModel
    {
        public List<metriceColumnDto> Columns { get; set; }
        public int SelectableColumns { get; set; }
    }
}
