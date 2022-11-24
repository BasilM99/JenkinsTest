using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Model
{
    public class AutoComplete
    {
        public AutoComplete()
        {
            CurrentText = string.Empty;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string CurrentText { get; set; }
        public string Culture { get; set; }
        public bool IsAjax { get; set; }
        public string ActionName { get; set; }
        public string PlaceHolder { get; set; }
        public string ControllerName { get; set; }
        public string ChangeCallBack { get; set; }
        public string LabelExpression { get; set; }
        public string ValueExpression { get; set; }
        public string Style { get; set; }
        public bool IsRequired { get; set; }
        public bool UseValueId { get; set; }
        public string ValueIdName { get; set; }
        public bool UseName { get; set; }
    }
}
