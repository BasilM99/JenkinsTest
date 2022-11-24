using Noqoush.AdFalcon.Web.Controllers.Model.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Tree
{
    public class LookupItem
    {
        public string Id { get; set; }
        public string DispalValue { get; set; }
        public Dictionary<string, string> Info { get; set; }

    }

    public class TreeSelectedValue
    {
        public string Id { get; set; }
        public string Key { get; set; }
    }

    public class TreeViewModel
    {
        public TreeViewModel()
        {
            SelectedItems = new List<LookupItem>();
        }
        public string Id { get; set; }

        public string ReportTempName { get; set; }
        public string Name { get; set; }
        public bool IsAjax { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Url { get; set; }
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
