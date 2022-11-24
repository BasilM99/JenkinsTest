using System.Collections.Generic;

namespace Noqoush.AdFalcon.Web.Controllers.Model.AppSite
{
    public class ListViewModelBase
    {
        public IEnumerable<Action> ToolTips { get; set; }
        public IEnumerable<Action> TopActions { get; set; }
        public IEnumerable<Action> BelowAction { get; set; }
        public IEnumerable<Action> FilterBar { get; set; }
    }
}