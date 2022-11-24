using Noqoush.AdFalcon.Web.Controllers.Model.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Report;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Noqoush.AdFalcon.Web.Controllers.Model.QueryBuilder
{
    public class TreeViewMode
    {
        public string Code { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Id { get; set; }
        public bool IsHidden { get; set; }
        public string Name { get; set; }

        public string Url { get; set; }
        public string OptionalParameter { get; set; }

        public TreeViewMode()
        {
            SelectedItems = new List<LookupItem>();
        }
      

        public string ReportTempName { get; set; }
   
        public bool IsAjax { get; set; }
  
    
        public IList<TreeSelectedValue> SelectedValues { get; set; }
        public IList<LookupItem> SelectedItems { get; set; }
        public int IsAll { get; set; }
        public bool IsSelectAll { get; set; }
        public bool IsSubLevel { get; set; }

        public bool ShowAudienceSegmentUsage { get; set; }
        public CampaignReportSchedulingViewModel CampaignReportSchaduling { get; set; }
        public ColumnViewModel ColumnViewModel { get; set; }

    }
}