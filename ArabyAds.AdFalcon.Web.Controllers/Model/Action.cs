using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Web.Controllers.Model
{
    public enum ActionType
    {
        Submit,
        Link,
        ajax

    }
    public enum AjaxType
    {
        clone,
        rename,
        Download

    }
    public class Action
    {
        public string Code { get; set; }
        public string ClassName { get; set; }
        public string ControllerName { get; set; }
        public string URL { get; set; }
        public string ActionName { get; set; }
        public string style { get; set; }

        public object ExtraPrams { get; set; }
        public object ExtraPrams2 { get; set; }
        public object ExtraPrams3 { get; set; }
        public string onClickEvent { get; set; }    
        public object ExtraPrams4 { get; set; }
        public string DisplayText { get; set; }
        public ActionType Type { get; set; }
        public AjaxType AjaxType { get; set; }
        public bool IsSelected { get; set; }
        public string CustomAction { get; set; }
        public string CallBack { get; set; }
    }
}
